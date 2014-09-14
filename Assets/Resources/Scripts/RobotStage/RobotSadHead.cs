using UnityEngine;
using System.Collections;

public class RobotSadHead : RobotHead {

  public ParticleSystem cloud;
  public ParticleSystem rain;
  public float rainDelay = 3.0f;
  public float cloudHeight = 0.8f;
  public float cloudSpeed = 0.02f;
  public AudioSource beatSource;
  public float dancesPerLoop = 1.0f;
  public float danceForce = 1.0f;
  public float maxArmBend = 100.0f;

  private float time_alive_ = 0.0f;
  private float last_dance_phase_ = 0.0f;

  void Start() {
    cloud.Stop();
    rain.Stop();
    SetFaceAlpha(0.0f);
  }

  public override void BootUp() {
    base.BootUp();
    cloud.Play();
    SetFaceAlpha(1.0f);
    time_alive_ = 0.0f;
  }

  public override void ShutDown() {
    base.ShutDown();
    cloud.Stop();
    rain.Stop();
    SetFaceAlpha(0.0f);
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

    float progress = (1.0f * beatSource.timeSamples) / beatSource.clip.samples;
    float dance_phase = dancesPerLoop * progress;
    dance_phase -= (int)dance_phase;

    if (last_dance_phase_ > dance_phase) {
      foreach (HingeJoint arm_joint in GetBody().armJoints) {
        arm_joint.useSpring = true;
        JointSpring arm_spring = arm_joint.spring;
        arm_spring.spring = danceForce;
        arm_spring.targetPosition = Random.Range(-maxArmBend, maxArmBend);
        arm_joint.spring = arm_spring;
      }
    }
    last_dance_phase_ = dance_phase;
  }
}
