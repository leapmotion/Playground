using UnityEngine;
using System.Collections;

public class RobotDizzyHead : RobotHead {

  public Transform leftEye;
  public Transform rightEye;
  public float leftEyeRotate = 1.0f;
  public float rightEyeRotate = 1.0f;

  public float drunkednessScale = 1.0f;
  public float drunkednessJerkiness = 1.0f;

  private float perlin_noise_increment_ = 0.0f;

  void Start() {
    SetFaceAlpha(0.0f);
    GetComponent<ParticleSystem>().Stop();
  }

  public override void BootUp() {
    GetComponent<ParticleSystem>().Play();
    SetFaceAlpha(1.0f);
  }

  public override void ShutDown() {
    GetComponent<ParticleSystem>().Stop();
    SetFaceAlpha(0.0f);
  }

  void Update() {
    leftEye.localRotation *= Quaternion.AngleAxis(leftEyeRotate, Vector3.forward);
    rightEye.localRotation *= Quaternion.AngleAxis(rightEyeRotate, Vector3.forward);

    if (robot_body_ != null) {
      float x_torque = 0.48f - Mathf.PerlinNoise(perlin_noise_increment_, 0.0f);
      float z_torque = 0.48f - Mathf.PerlinNoise(perlin_noise_increment_, 100.0f);
      robot_body_.feet.AddTorque(drunkednessScale * new Vector3(x_torque, 0, z_torque));
      perlin_noise_increment_ += drunkednessJerkiness * Time.deltaTime;
    }
  }
}
