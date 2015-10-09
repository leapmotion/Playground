/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

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
  public float lightningAmplitude = 1.0f;
  public float minimumHeadHeightDifference = 0.0f;
  public float maxMagnetForce = 10.0f;
  public float attachRadius = 0.3f;
  public float breakForce = 10.0f;
  public float breakTorque = 10.0f;
  public float uprightAngle = 60.0f;
  public float uprightForce = 1.0f;
  public float fallenWaitTime = 2.0f;
  public float upsideDownAngle = 100.0f;

  public Roller feet;

  private float lightning_noise_phase_ = 0.0f;
  private RobotHead robot_head_ = null;
  private bool is_upright_ = true;
  private float fallen_time_ = 0.0f;

  void Start() {
    GetComponent<Rigidbody>().maxAngularVelocity = 20;
  }

  void RemoveLine() {
    LineRenderer line_renderer = GetComponent<LineRenderer>();
    if (line_renderer != null)
      line_renderer.SetVertexCount(0);
  }

  public bool IsUpright() {
    return is_upright_;
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

      Vector3 offset = lightningAmplitude * phase * (1 - phase) *
                       new Vector3(offset_x, offset_y, offset_z);
      line_renderer.SetPosition(i, normal_position + offset);
    }

    lightning_noise_phase_ += Time.deltaTime * lightningChangeFrequency;
  }

  void DisconnectHead() {
    if (robot_head_ != null) {
      robot_head_.ShutDown();
      robot_head_.SetBody(null);
      robot_head_ = null;
    }
  }

  void AttachHead(Collider new_head) {
    SpringJoint spring = GetComponent<SpringJoint>();
    RemoveLine();        
    if (spring != null)
      Destroy(spring);
    
    Vector3[] faces = {new_head.transform.up, -new_head.transform.up,
                       new_head.transform.right, -new_head.transform.right,
                       new_head.transform.forward, -new_head.transform.forward};

    float greatest_dot = 0.0f;
    Vector3 attach_face = Vector3.zero;
    for (int i = 0; i < faces.Length; ++i) {
      float dot = Vector3.Dot(-transform.up, faces[i]);
      if (dot > greatest_dot) {
        attach_face = faces[i];
        greatest_dot = dot;
      }
    }

    new_head.transform.rotation = Quaternion.FromToRotation(attach_face, -transform.up) *
                                  new_head.transform.rotation;

    greatest_dot = 0.0f;
    Vector3 robot_face_vector = Vector3.zero;
    for (int i = 0; i < faces.Length; ++i) {
      float dot = Vector3.Dot(transform.forward, faces[i]);
      if (dot > greatest_dot) {
        robot_face_vector = faces[i];
        greatest_dot = dot;
      }
    }

    FixedJoint head_joint = gameObject.AddComponent<FixedJoint>();
    head_joint.connectedBody = new_head.GetComponent<Rigidbody>();
    head_joint.anchor = headCenterOffset;
    head_joint.autoConfigureConnectedAnchor = false;
    head_joint.connectedAnchor = Vector3.zero;
    head_joint.breakForce = breakForce;
    head_joint.breakTorque = breakTorque;

    if (robot_head_ != new_head.GetComponent<RobotHead>()) {
      robot_head_ = new_head.GetComponent<RobotHead>();        
      robot_head_.SetBody(this);
      robot_head_.SetOrientation(transform.up, robot_face_vector);
      robot_head_.BootUp();
    }
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
      RobotHead robot_head = head.GetComponent<RobotHead>();
      if (distance.magnitude < closest_distance &&
          transform.InverseTransformDirection(distance).y >= minimumHeadHeightDifference &&
          robot_head != null && (robot_head.GetBody() == null || robot_head.GetBody() == this)) {
        closest_head = head;
        closest_distance = distance.magnitude;
      }
    }

    SpringJoint spring = GetComponent<SpringJoint>();
    if (closest_head == null) {
      DisconnectHead();
      RemoveLine();
      if (spring != null)
        Destroy(spring);
      GetComponent<AudioSource>().Stop();
    }
    else {
      if (closest_head.GetComponent<RobotHead>() != robot_head_)
        DisconnectHead();

      if ((head_center_position - closest_head.transform.position).magnitude < attachRadius) {
        AttachHead(closest_head);
        GetComponent<AudioSource>().Stop();
      }
      else {
        DisconnectHead();
        DrawLine(neck_position, closest_head.transform.position);
        if (spring == null) {
          spring = gameObject.AddComponent<SpringJoint>();
          GetComponent<AudioSource>().Play();
        }

        spring.connectedBody = closest_head.GetComponent<Rigidbody>();
        spring.anchor = headCenterOffset;
        float magnet_force = maxMagnetForce *
                             Mathf.Clamp((1 - closest_distance / headMagnetRadius), 0, 1);
        spring.spring = magnet_force * magnet_force;
        spring.maxDistance = 0.0f;
        spring.autoConfigureConnectedAnchor = false;
        spring.connectedAnchor = Vector3.zero;
      }
    }
  }

	void FixedUpdate () {
    FixedJoint head_joint = GetComponent<FixedJoint>();
    if (head_joint == null)
      LookForHead();

    if (robot_head_ != null) {
      Vector3[] faces = {robot_head_.transform.up, -robot_head_.transform.up,
                         robot_head_.transform.right, -robot_head_.transform.right,
                         robot_head_.transform.forward, -robot_head_.transform.forward};
      float greatest_dot = 0.0f;
      Vector3 robot_face_vector = Vector3.zero;
      for (int i = 0; i < faces.Length; ++i) {
        float dot = Vector3.Dot(transform.forward, faces[i]);
        if (dot > greatest_dot) {
          robot_face_vector = faces[i];
          greatest_dot = dot;
        }
      }

      robot_head_.SetOrientation(transform.up, robot_face_vector);
    }

    Quaternion uprightness = Quaternion.FromToRotation(Vector3.up, transform.up);
    float upright_angle = 0;
    Vector3 upright_axis = Vector3.up;
    uprightness.ToAngleAxis(out upright_angle, out upright_axis);
    is_upright_ = upright_angle < uprightAngle;

    if (!is_upright_) {
      fallen_time_ += Time.deltaTime;
      if (upright_angle < upsideDownAngle && fallen_time_ > fallenWaitTime)
        GetComponent<Rigidbody>().AddTorque(-uprightForce * upright_axis);
    }
    else
      fallen_time_ = 0.0f;
	}
}
