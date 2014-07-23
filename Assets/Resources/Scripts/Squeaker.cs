/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class Squeaker : MonoBehaviour {

  public float loudness = 1.0f;
  public AudioClip squeakClip;
  public float squeakFrequency = 1.0f;
  private float phase_ = 0.0f;

  private const float MAX_PHASE = 0.2f;

  void Update() {
    float speed = rigidbody.angularVelocity.magnitude;
    phase_ += Mathf.Clamp(squeakFrequency * speed / 360.0f, 0, MAX_PHASE);
    if (phase_ > 1.0f) {
      phase_ -= 1.0f;
      audio.PlayOneShot(squeakClip, loudness - 1.0f / speed);
    }
  }
}
