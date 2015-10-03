/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class StemDance : MonoBehaviour {

  public Rigidbody[] segments;
  public float maxBend = 5.0f;
  public float minBend = -5.0f;
  public float danceFrequency = 1.0f;
  public float danceDelay = 1.0f;
  public AnimationCurve danceCurve;
  public AnimationCurve segmentPhaseOffset;
  public AnimationCurve segmentDanceAmplitude;

  private float[] natural_bends_;
  private float phase_ = 0.0f;
  private bool dancing_ = true;
  private float dance_amount_ = 0.0f;

  void Update () {
    for (int i = 0; i < segments.Length; ++i) {
      if (segments[i] == null)
        return;
    }

    if (Input.GetKeyDown("d"))
      dancing_ = !dancing_;

    if (dancing_)
      dance_amount_ += Time.deltaTime / danceDelay;
    else {
      dance_amount_ -= Time.deltaTime / danceDelay;
      return;
    }

    dance_amount_ = Mathf.Clamp(dance_amount_, 0.0f, 1.0f);
    
    phase_ += danceFrequency * Time.deltaTime;
    for (int i = 0; i < segments.Length; ++i) {
      float stem_position = (i - 1.0f) / segments.Length;
      float segment_phase = phase_ + segmentPhaseOffset.Evaluate(stem_position);
      segment_phase -= (int)segment_phase;
      float dance_curve = danceCurve.Evaluate(segment_phase);
      float dance_amplitude = segmentDanceAmplitude.Evaluate(stem_position);
      float desired_bend = (minBend + (maxBend - minBend) * dance_curve) * dance_amplitude;

      if (segments[i].GetComponent<HingeJoint>() != null) {
        JointSpring spring = segments[i].GetComponent<HingeJoint>().spring;
        spring.targetPosition = desired_bend;
        segments[i].GetComponent<HingeJoint>().spring = spring;
      }
    }
  }
}
