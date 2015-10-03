/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class FadeInOutAndFinish : MonoBehaviour {

  public float fadeInTime = 1.0f;
  public float onTime = 1.0f;
  public float fadeOutTime = 1.0f;
  public AnimationCurve fadeCurve;

  private float total_time_;

  void Start() {
    SetAlpha(0.0f);
    total_time_ = fadeInTime + onTime + fadeOutTime;
  }

  void SetAlpha(float alpha) {
    Color color = GetComponent<Renderer>().material.color;
    color.a = alpha;
    GetComponent<Renderer>().material.color = color;
  }

  void Update() {
    float time = Time.timeSinceLevelLoad;
    if (time < fadeInTime)
      SetAlpha(fadeCurve.Evaluate(time / fadeInTime));
    else if (time < fadeInTime + onTime)
      SetAlpha(1);
    else if (time < total_time_)
      SetAlpha(fadeCurve.Evaluate((total_time_ - time) / fadeOutTime));
    else if (Application.loadedLevel + 1 < Application.levelCount)
      Application.LoadLevel(Application.loadedLevel + 1);
    else
      Application.Quit();
  }
}
