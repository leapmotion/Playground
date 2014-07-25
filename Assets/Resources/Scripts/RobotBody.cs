using UnityEngine;
using System.Collections;

public class RobotBody : MonoBehaviour {

  private const float Y_PERLIN_OFFSET = 10.0f;
  private const float Z_PERLIN_OFFSET = 20.0f;

  public Vector3 headAttachmentOffset;
  public Vector3 headCenterOffset;
  public float headMagnetRadius = 1.0f;
  public float lightningPerUnit = 20.0f;
  public float lightningChangeFrequency = 1.0f;
  public float lightningJaggedness = 5.0f;
  public float minimumHeadHeightDifference = 0.0f;
  public float maxMagnetForce = 10.0f;
  public float attachRadius = 0.3f;
  public float breakForce = 10.0f;
  public float breakTorque = 10.0f;

  private float lightning_noise_phase_ = 0.0f;

  void RemoveLine() {
    LineRenderer line_renderer = GetComponent<LineRenderer>();
    if (line_renderer != null)
      line_renderer.SetVertexCount(0);
  }

  void DrawLine(Vector3 start, Vector3 end) {
    LineRenderer line_renderer = GetComponent<LineRenderer>();
    if (line_renderer == null)
      return;

    float distance = (start - end).magnitude;
    int number_points = (int)(2 + distance * lightningPerUnit);
    line_renderer.SetVertexCount(number_points);

    for (int i = 0; i < number_points; ++i) {
      float phase = i / (number_points - 1.0f);
      Vector3 normal_position = start + phase * (end - start);

      float offset_x = Mathf.PerlinNoise(lightning_noise_phase_,
                                         phase * lightningJaggedness) - 0.5f;
      float offset_y = Mathf.PerlinNoise(Y_PERLIN_OFFSET + lightning_noise_phase_,
                                         phase * lightningJaggedness) - 0.5f;
      float offset_z = Mathf.PerlinNoise(Z_PERLIN_OFFSET + lightning_noise_phase_,
                                         phase * lightningJaggedness) - 0.5f;

      Vector3 offset = phase * (1 - phase) * new Vector3(offset_x, offset_y, offset_z);
      line_renderer.SetPosition(i, normal_position + offset);
    }

    lightning_noise_phase_ += Time.deltaTime * lightningChangeFrequency;
  }

  void LookForHead() {
    Vector3 neck_position = transform.TransformPoint(headAttachmentOffset);
    Vector3 head_center_position = transform.TransformPoint(headCenterOffset);
    int head_mask = LayerMask.GetMask("BoxHead");
    Collider[] heads = Physics.OverlapSphere(neck_position, headMagnetRadius, head_mask);

    Collider closest_head = null;
    float closest_distance = headMagnetRadius;

    foreach (Collider head in heads) {
      Vector3 distance = head.transform.position - neck_position;
      if (distance.magnitude < closest_distance &&
          transform.InverseTransformDirection(distance).y >= minimumHeadHeightDifference) {
        closest_head = head;
        closest_distance = distance.magnitude;
      }
    }

    SpringJoint spring = GetComponent<SpringJoint>();
    if (closest_head != null) {
      
      if ((head_center_position - closest_head.transform.position).magnitude < attachRadius) {
        RemoveLine();        
        if (spring != null)
          Destroy(spring);

        FixedJoint head_joint = gameObject.AddComponent<FixedJoint>();
        head_joint.connectedBody = closest_head.rigidbody;
        head_joint.anchor = headCenterOffset;
        head_joint.autoConfigureConnectedAnchor = false;
        head_joint.connectedAnchor = Vector3.zero;
        head_joint.breakForce = breakForce;
        head_joint.breakTorque = breakTorque;
      }
      else {
        DrawLine(neck_position, closest_head.transform.position);
        if (spring == null)
          spring = gameObject.AddComponent<SpringJoint>();

        spring.connectedBody = closest_head.rigidbody;
        spring.anchor = headCenterOffset;
        float magnet_force = maxMagnetForce *
                             Mathf.Clamp((1 - closest_distance / headMagnetRadius), 0, 1);
        spring.spring = magnet_force * magnet_force;
        spring.maxDistance = 0.0f;
        spring.autoConfigureConnectedAnchor = false;
        spring.connectedAnchor = Vector3.zero;
      }
    }
    else {
      RemoveLine();
      if (spring != null)
        Destroy(spring);
    }
  }

	void FixedUpdate () {
    FixedJoint head_joint = GetComponent<FixedJoint>();
    if (head_joint == null)
      LookForHead();
	}
}
