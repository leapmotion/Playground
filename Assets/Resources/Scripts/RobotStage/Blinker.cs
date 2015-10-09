/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class Blinker : MonoBehaviour {

  public float fadeInTime = 0.2f;
  public float fadeOutTime = 0.2f;
  public AnimationCurve fadeCurve;
  public float maxAlpha = 0.2f;

  float progress_ = 0.0f;

  void Start() {
    progress_ = fadeInTime + fadeOutTime;
  }

  public void Blink() {
    progress_ = 0.0f;
  }

  void SetAlpha(float alpha) {
    Color color = GetComponent<Renderer>().material.GetColor("_TintColor");
    color.a = maxAlpha * alpha;
    GetComponent<Renderer>().material.SetColor("_TintColor", color);
  }

  void Update () {
    progress_ += Time.deltaTime;
    if (progress_ < fadeInTime)
      SetAlpha(fadeCurve.Evaluate(progress_ / fadeInTime));
    else
      SetAlpha(fadeCurve.Evaluate(1.0f - (progress_ - fadeInTime) / fadeOutTime));
  }
}
