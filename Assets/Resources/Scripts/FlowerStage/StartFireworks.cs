/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class StartFireworks : MonoBehaviour {

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += PlayFireworks;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= PlayFireworks;
  }

  void PlayFireworks() {
    ParticleSystem fireworks = GetComponent<ParticleSystem>();
    fireworks.Play();
  }
}
