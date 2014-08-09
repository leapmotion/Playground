using UnityEngine;
using System.Collections;

public class RobotSadHead : RobotHead {

  public ParticleSystem cloud;
  public ParticleSystem rain;
  public float rainDelay = 3.0f;
  public float cloudHeight = 0.8f;
  public float cloudSpeed = 0.02f;
  public float danceFrequency = 1.0f;
  public float danceForce = 1.0f;
  public float maxArmBend = 100.0f;

  private float time_alive_ = 0.0f;
  private int dance_moves_ = 0;

  void Start() {
    ShutDown();
  }

  public override void BootUp() {
    cloud.Play();
    SetFaceAlpha(1.0f);
    time_alive_ = 0.0f;
  }

  public override void ShutDown() {
    cloud.Stop();
    rain.Stop();
    SetFaceAlpha(0.0f);
    dance_moves_ = 0;
    if (GetBody() != null) {
      foreach (HingeJoint arm_joint in GetBody().armJoints) {
        arm_joint.useSpring = false;
      }
    }
  }

  void Update() {
    if (GetBody() == null)
      return;

    if (time_alive_ < rainDelay) {
      if (time_alive_ + Time.deltaTime >= rainDelay) {
        rain.Play();
      }
    }
    time_alive_ += Time.deltaTime;
    cloud.rigidbody.velocity = cloudSpeed * (transform.position + cloudHeight * Vector3.up -
                                             cloud.transform.position) / Time.deltaTime;
    rain.rigidbody.velocity = cloudSpeed * (transform.position + cloudHeight * Vector3.up -
                                            rain.transform.position) / Time.deltaTime;

    int dances = (int)(time_alive_ * danceFrequency);
    if (dances > dance_moves_) {
      dance_moves_ = dances;
      foreach (HingeJoint arm_joint in GetBody().armJoints) {
        dance_moves_ = dances;
        arm_joint.useSpring = true;
        JointSpring arm_spring = arm_joint.spring;
        arm_spring.spring = danceForce;
        arm_spring.targetPosition = Random.Range(-maxArmBend, maxArmBend);
        arm_joint.spring = arm_spring;
      }
    }
  }
}
