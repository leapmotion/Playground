/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class ShowOnCollision : MonoBehaviour {

  public float showTime = 1.0f;
  public AnimationCurve showAnimation;
  public float amplitudeSpeedScale = 1.0f;
  public float playbackSpeedScale = 1.0f;
  
  private float progress_ = 0.0f;
  private float speed_ = 0.0f;

  void OnCollisionEnter(Collision collision) {
    speed_ = collision.relativeVelocity.magnitude;
    progress_ = 0.0f;
  }

  void Update() {
    float alpha = 0.0f;
    if (progress_ <= 1.0f) {
      alpha = speed_ * amplitudeSpeedScale * showAnimation.Evaluate(progress_);
      progress_ += (playbackSpeedScale / speed_) * Time.deltaTime / showTime;
    }

    Color color = renderer.material.color;
    color.a = alpha;
    renderer.material.color = color;
  }
}
