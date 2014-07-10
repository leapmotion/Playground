/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class FadeInOut : MonoBehaviour {

  public float fadeInTime = 0.0f;
  
  private float alpha_ = 0.0f;
  private float last_time_;

  void Start() {
    last_time_ = Time.time;
    Update();
  }

  void Update() {
    float delta_time = Time.time - last_time_;
    last_time_ = Time.time;

    Hand leap_hand = GetComponent<HandModel>().GetLeapHand();
    if (leap_hand == null) {
      alpha_ -= delta_time / fadeInTime;
      if (alpha_ <= 0) {
        Destroy(gameObject);
        return;
      }
    }
    else {
      alpha_ += delta_time / fadeInTime;
      alpha_ = Mathf.Clamp(alpha_, 0, 1);
    }

    Renderer[] renderers = GetComponentsInChildren<Renderer>();

    for (int i = 0; i < renderers.Length; ++i) {
      Color main_color = renderers[i].material.color;
      main_color.a = alpha_ * alpha_;
      renderers[i].material.color = main_color;
    }
  }
}
