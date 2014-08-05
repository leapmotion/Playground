using UnityEngine;
using System.Collections;

public class RobotSadHead : RobotHead {

  public ParticleSystem cloud;
  public ParticleSystem rain;
  public float rainDelay = 3.0f;
  public float cloudHeight = 0.8f;
  public float cloudSpeed = 0.02f;

  private float time_alive_ = 0.0f;
  private bool alive_ = false;

  void Start() {
    ShutDown();
  }

  public override void BootUp() {
    cloud.Play();
    SetFaceAlpha(1.0f);
    time_alive_ = 0.0f;
    alive_ = true;
  }

  public override void ShutDown() {
    cloud.Stop();
    rain.Stop();
    SetFaceAlpha(0.0f);
    alive_ = false;
  }

  void Update() {
    if (!alive_)
      return;

    if (time_alive_ < rainDelay) {
      time_alive_ += Time.deltaTime;
      if (time_alive_ >= rainDelay) {
        rain.Play();
      }
    }
    cloud.rigidbody.velocity = cloudSpeed * (transform.position + cloudHeight * Vector3.up -
                                             cloud.transform.position) / Time.deltaTime;
    rain.rigidbody.velocity = cloudSpeed * (transform.position + cloudHeight * Vector3.up -
                                            rain.transform.position) / Time.deltaTime;
  }
}
