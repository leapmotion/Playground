/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class RobotDizzyHead : RobotHead {

  public Transform leftEye;
  public Transform rightEye;
  public float leftEyeRotate = 1.0f;
  public float rightEyeRotate = 1.0f;

  public float drunkednessScale = 1.0f;
  public float drunkednessJerkiness = 1.0f;
  public float forceTowardCenter = 0.5f;
  public Vector3 center;

  public ParticleSystem stars;

  private float perlin_noise_increment_ = 0.0f;

  void Start() {
    SetFaceAlpha(0.0f);
    stars.Stop();
  }

  public override void BootUp() {
    base.BootUp();
    stars.Play();
    SetFaceAlpha(1.0f);
  }

  public override void ShutDown() {
    base.ShutDown();
    stars.Stop();
    SetFaceAlpha(0.0f);
  }

  void FixedUpdate() {
    leftEye.localRotation *= Quaternion.AngleAxis(leftEyeRotate, Vector3.forward);
    rightEye.localRotation *= Quaternion.AngleAxis(rightEyeRotate, Vector3.forward);

    if (GetBody() != null && GetBody().feet.IsUpright()) {
      float x_torque = 0.48f - Mathf.PerlinNoise(perlin_noise_increment_, 0.0f);
      float z_torque = 0.47f - Mathf.PerlinNoise(perlin_noise_increment_, 100.0f);
      float torque_scale = Time.deltaTime * drunkednessScale;
      robot_body_.feet.GetComponent<Rigidbody>().AddTorque(torque_scale * new Vector3(x_torque, 0, z_torque));
      perlin_noise_increment_ += drunkednessJerkiness * Time.deltaTime;

      Vector3 direction = (center - transform.position).normalized;
      Vector3 torque = Time.deltaTime * forceTowardCenter * Vector3.Cross(direction, Vector3.up);
      GetBody().feet.GetComponent<Rigidbody>().AddTorque(torque);
    }
  }
}
