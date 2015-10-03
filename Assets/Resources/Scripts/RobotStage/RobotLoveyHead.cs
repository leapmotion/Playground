/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class RobotLoveyHead : RobotHead {

  public Texture normalLeftEye;
  public Texture normalRightEye;
  public Texture[] lovingLeftEyes;
  public Texture[] lovingRightEyes;

  public Renderer[] leftEyes;
  public Renderer[] rightEyes;

  public float eyeChangeFrequency = 10.0f;
  public float loveRange = 3.0f;
  public float loveRunSpeed = 3.0f;

  private float active_time_ = 0;

  public float spinTorqueScale = 1.0f;
  public AnimationCurve spinTorqueCurve;

  public ParticleSystem hearts;
  public AudioSource beatSource;
  public float spinsPerLoop = 1.0f;

  void Start() {
    SetFaceAlpha(0.0f);
    hearts.Stop();
  }

  public override void BootUp() {
    base.BootUp();
    SetFaceAlpha(1.0f);
    active_time_ = 0.0f;
  }

  public override void ShutDown() {
    base.ShutDown();
    SetFaceAlpha(0.0f);
    hearts.Stop();
  }

  void NormalEyes() {
    for (int i = 0; i < leftEyes.Length; ++i)
      leftEyes[i].material.mainTexture = normalLeftEye;

    for (int i = 0; i < rightEyes.Length; ++i)
      rightEyes[i].material.mainTexture = normalRightEye;
  }

  void CycleLoveyEyes() {
    int love_index = (int)(active_time_ * eyeChangeFrequency);
    for (int i = 0; i < leftEyes.Length; ++i)
      leftEyes[i].material.mainTexture = lovingLeftEyes[love_index % leftEyes.Length];

    for (int i = 0; i < rightEyes.Length; ++i)
      rightEyes[i].material.mainTexture = lovingRightEyes[love_index % rightEyes.Length];
  }

  void Update() {
    if (GetBody() == null)
      return;

    int head_mask = LayerMask.GetMask("BoxHead");    
    Collider[] heads = Physics.OverlapSphere(transform.position, loveRange, head_mask);
    Transform target = null;
    float closest_distance = loveRange;

    foreach (Collider head in heads) {
      RobotHead robot_head = head.GetComponent<RobotHead>();
      if (robot_head != null && robot_head.GetBody() != null && robot_head != this) {
        float distance = (robot_head.transform.position - transform.position).magnitude;
        if (closest_distance > distance) {
          closest_distance = distance;
          target = robot_head.transform;
        }
      }
    }

    if (target == null) {
      NormalEyes();
      hearts.Stop();
    }
    else {
      if (!hearts.isPlaying)
        hearts.Play();
      CycleLoveyEyes();
      Vector3 direction = (target.position - transform.position).normalized;
      Vector3 torque = loveRunSpeed * Vector3.Cross(direction, Vector3.up);
      GetBody().feet.GetComponent<Rigidbody>().AddTorque(torque);
    }

    if (GetBody().feet.IsUpright()) {
      float progress = (1.0f * beatSource.timeSamples) / beatSource.clip.samples;
      float spin_phase = spinsPerLoop * progress;
      spin_phase -= (int)spin_phase;
      
      float spin_torque = spinTorqueScale * spinTorqueCurve.Evaluate(spin_phase);
      GetComponent<Rigidbody>().AddTorque(spin_torque * GetBody().transform.up);
    }

    active_time_ += Time.deltaTime;
  }
}
