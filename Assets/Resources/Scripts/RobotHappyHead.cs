using UnityEngine;
using System.Collections;

public class RobotHappyHead : RobotHead {

  public float danceScale = 0.5f;
  public float danceFrequency = 0.0f;
  public float danceDutyCycle = 0.2f;

  private Vector3 resting_anchor_;
  private float dance_phase_ = 0.0f;

  void Start() {
    SetFaceAlpha(0.0f);
  }

  public override void BootUp() {
    SetFaceAlpha(1.0f);
    SpringJoint bounce = robot_body_.feet.GetComponent<SpringJoint>();
    resting_anchor_ = bounce.connectedAnchor;
  }

  public override void ShutDown() {
    SetFaceAlpha(0.0f);
    SpringJoint bounce = robot_body_.feet.GetComponent<SpringJoint>();
    bounce.connectedAnchor = resting_anchor_;
  }

  void Update() {
    if (robot_body_ != null) {
      SpringJoint bounce = robot_body_.feet.GetComponent<SpringJoint>();

      dance_phase_ += danceFrequency * Time.deltaTime;
      dance_phase_ -= (int)dance_phase_;
      if (dance_phase_ < danceDutyCycle)
        bounce.connectedAnchor = (1 - danceScale) * resting_anchor_;
      else
        bounce.connectedAnchor = resting_anchor_;
    }
  }
}
