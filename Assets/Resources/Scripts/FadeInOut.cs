/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class FadeInOut : MonoBehaviour {

  public float fadeInTime = 0.01f;
  public float fadeInWait = 0.0f;
  public float fadeOutTime = 0.01f;
  
  private float alpha_ = 0.0f;
  private float last_time_;
  private float time_waited_ = 0.0f;

  void Start() {
    last_time_ = Time.time;
    FixedUpdate();
  }

  void FixedUpdate() {
    float delta_time = Time.time - last_time_;
    last_time_ = Time.time;

    Hand leap_hand = GetComponent<HandModel>().GetLeapHand();
    if (leap_hand == null) {
      alpha_ -= delta_time / fadeOutTime;
      if (alpha_ <= 0) {
        Destroy(gameObject);
        return;
      }
    }
    else if (time_waited_ < fadeInWait) {
      time_waited_ += Time.deltaTime;
    }
    else {
      alpha_ += delta_time / fadeInTime;
      alpha_ = Mathf.Clamp(alpha_, 0, 1);
    }

    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    for (int i = 0; i < renderers.Length; ++i) {
      if (renderers[i].material.HasProperty("_Color")) {
        Color main_color = renderers[i].material.color;
        main_color.a = alpha_ * alpha_;
        renderers[i].material.color = main_color;
      }
      else if (renderers[i].material.HasProperty("_TintColor")) {
        Color main_color = renderers[i].material.GetColor("_TintColor");
        main_color.a = alpha_ * alpha_;
        renderers[i].material.SetColor("_TintColor", main_color);
      }
    }
  }
}
