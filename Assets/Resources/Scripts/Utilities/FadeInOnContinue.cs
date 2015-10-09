/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class FadeInOnContinue : MonoBehaviour {

  public float fadeInTime = 1.0f;
  public AnimationCurve fadeCurve;

  private bool fading_ = false;
  private float fade_amount_ = 0.0f;

  void Start() {
    SetAlpha(0.0f);
  }

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += FadeIn;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= FadeIn;
  }

  void FadeIn() {
    fading_ = true;
  }

  void SetAlpha(float alpha) {
    Color color = GetComponent<Renderer>().material.GetColor("_TintColor");
    color.a = alpha;
    GetComponent<Renderer>().material.SetColor("_TintColor", color);
  }

  void Update() {
    if (fading_) {
      fade_amount_ += Time.deltaTime / fadeInTime;
      SetAlpha(fadeCurve.Evaluate(fade_amount_));

      if (fade_amount_ >= 1.0f)
        fading_ = false;
    }
  }
}
