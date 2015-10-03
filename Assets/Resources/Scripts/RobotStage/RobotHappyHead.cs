/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class RobotHappyHead : RobotHead {

  public float danceScale = 0.5f;
  public AudioSource beatSource;
  public float dancesPerLoop = 1.0f;
  public float danceDutyCycle = 0.2f;

  public Texture leftHappyEye;
  public Texture rightHappyEye;
  public Texture leftScaredEye;
  public Texture rightScaredEye;

  public Renderer[] leftEyes;
  public Renderer[] rightEyes;

  public float loverRadius = 1.0f;
  public float runSpeed = 3.0f;

  private Vector3 resting_anchor_;

  private bool running_ = false;

  void Start() {
    SetFaceAlpha(0.0f);
  }

  public override void BootUp() {
    base.BootUp();
    SetFaceAlpha(1.0f);
    SpringJoint bounce = robot_body_.feet.GetComponent<SpringJoint>();
    resting_anchor_ = bounce.connectedAnchor;
  }

  public override void ShutDown() {
    base.ShutDown();
    SetFaceAlpha(0.0f);
    SpringJoint bounce = robot_body_.feet.GetComponent<SpringJoint>();
    bounce.connectedAnchor = resting_anchor_;
  }

  void RunAway(Transform chaser) {
    Vector3 direction = (transform.position - chaser.transform.position).normalized;
    Vector3 torque = runSpeed * Vector3.Cross(direction, Vector3.up);
    GetBody().feet.GetComponent<Rigidbody>().AddTorque(torque);
  }

  void SetEyes(Texture left_eye, Texture right_eye) {
    for (int i = 0; i < leftEyes.Length; ++i)
      leftEyes[i].material.mainTexture = left_eye;

    for (int i = 0; i < rightEyes.Length; ++i)
      rightEyes[i].material.mainTexture = right_eye;
  }

  void Update() {
    if (GetBody() != null) {
      SpringJoint bounce = robot_body_.feet.GetComponent<SpringJoint>();

      float progress = (1.0f * beatSource.timeSamples) / beatSource.clip.samples;
      float dance_phase = dancesPerLoop * progress;
      dance_phase -= (int)dance_phase;

      if (dance_phase < danceDutyCycle && GetBody().feet.IsUpright())
        bounce.connectedAnchor = (1 - danceScale) * resting_anchor_;
      else
        bounce.connectedAnchor = resting_anchor_;

      if (GetBody().feet.IsUpright()) {
        int head_mask = LayerMask.GetMask("BoxHead");    
        Collider[] heads = Physics.OverlapSphere(transform.position, loverRadius, head_mask);
        Transform chaser = null;
        foreach (Collider head in heads) {
          RobotHead lovey_head = head.GetComponent<RobotLoveyHead>();
          if (lovey_head != null && lovey_head.GetBody() != null) {
            chaser = head.transform;
          }
        }

        if (chaser != null) {
          RunAway(chaser);
          if (!running_)
            SetEyes(leftScaredEye, rightScaredEye);
        }
        else if (running_) {
          SetEyes(leftHappyEye, rightHappyEye);
        }
        running_ = chaser != null;
      }
    }
  }
}
