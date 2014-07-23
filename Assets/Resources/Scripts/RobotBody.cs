using UnityEngine;
using System.Collections;

public class RobotBody : MonoBehaviour {

  private const float Y_PERLIN_OFFSET = 10.0f;
  private const float Z_PERLIN_OFFSET = 20.0f;

  public Vector3 headAttachmentOffset;
  public float headMagnetRadius = 1.0f;
  public float lightningPerUnit = 20.0f;
  public float lightningChangeFrequency = 1.0f;
  public float lightningJaggedness = 5.0f;

  private float lightning_noise_phase_ = 0.0f;

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
    int head_mask = LayerMask.GetMask("BoxHead");
    Collider[] heads = Physics.OverlapSphere(neck_position, headMagnetRadius, head_mask);

    Collider closest_head = null;
    float closest_distance = headMagnetRadius;

    foreach (Collider head in heads) {
      Vector3 distance = head.transform.position - neck_position;
      if (distance.magnitude < closest_distance) {
        closest_head = head;
        closest_distance = distance.magnitude;
      }
    }

    if (closest_head != null)
      DrawLine(neck_position, closest_head.transform.position);
  }

	void FixedUpdate () {
    FixedJoint head_joint = GetComponent<FixedJoint>();
    if (head_joint == null)
      LookForHead();
	}
}
