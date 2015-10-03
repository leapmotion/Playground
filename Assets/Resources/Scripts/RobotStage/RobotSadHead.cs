/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

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
  public float headTiltAngle = 5.0f;

  private float time_alive_ = 0.0f;

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
    cloud.GetComponent<Rigidbody>().velocity = cloudSpeed * (transform.position + cloudHeight * Vector3.up -
                                             cloud.transform.position) / Time.deltaTime;
    rain.GetComponent<Rigidbody>().velocity = cloudSpeed * (transform.position + cloudHeight * Vector3.up -
                                            rain.transform.position) / Time.deltaTime;

    float progress = (1.0f * beatSource.timeSamples) / beatSource.clip.samples;
    float dance_phase = dancesPerLoop * progress;
    dance_phase -= (int)dance_phase;
    
    float angle = headTiltAngle * Mathf.Cos(dance_phase * 2.0f * Mathf.PI);
    transform.rotation = Quaternion.AngleAxis(angle, face.transform.right) * transform.rotation;
  }
}
