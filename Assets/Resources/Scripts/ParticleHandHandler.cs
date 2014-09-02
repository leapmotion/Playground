/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class ParticleHandHandler : MonoBehaviour {

  private bool enable_emission_ = false;

  void LateUpdate() {
    bool play_particles = GetComponent<HandModel>().GetLeapHand() != null;
    ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

    if (play_particles) {
      foreach (ParticleSystem system in particles) {
        system.enableEmission = enable_emission_;
        if (!system.isPlaying) {
          system.Play();
        }
      }
      enable_emission_ = true;
    }
    else {
      foreach (ParticleSystem system in particles) {
        system.emissionRate = 0.0f;
      }
    }
  }
}
