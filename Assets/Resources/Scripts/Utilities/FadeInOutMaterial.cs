/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class FadeInOutMaterial : MonoBehaviour {

  public float fadeInTime = 4.0f;
  public float fadeOutTime = 4.0f;
  public float maxAlpha = 1.0f;
  public AnimationCurve fadeCurve;

  private bool fading_out_ = false;
  private float start_fade_alpha_;
  private float start_fade_time_;

  void Start() {
    SetAlpha(0.0f);
  }

  void SetAlpha(float alpha) {
    Color color = GetComponent<Renderer>().material.GetColor("_TintColor");
    color.a = alpha;
    GetComponent<Renderer>().material.SetColor("_TintColor", color);
  }

  public void FadeOut() {
    fading_out_ = true;
    start_fade_alpha_ = GetComponent<Renderer>().material.GetColor("_TintColor").a;
    start_fade_time_ = Time.timeSinceLevelLoad;
  }

  void Update() {
    float time = Time.timeSinceLevelLoad;
    if (fading_out_) {
      float t = (Time.timeSinceLevelLoad - start_fade_time_) / fadeOutTime;
      SetAlpha(start_fade_alpha_ * fadeCurve.Evaluate(1.0f - t));
    }
    else if (time < fadeInTime)
      SetAlpha(maxAlpha * fadeCurve.Evaluate(time / fadeInTime));
  }
}
