/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class ParticleFinisher : MonoBehaviour {

  public float destroyWaitTime = 5.0f;
  public string particlesName;

  ParticleSystem[] systems_;

  void Start() {
    systems_ = GetComponentsInChildren<ParticleSystem>();
  }

  void OnDestroy() {
    foreach (ParticleSystem system in systems_) {
      if (system.name == particlesName) {
        system.Stop();
        system.transform.parent = null;
        Destroy(system.gameObject, destroyWaitTime);
      }
    }
  }
}
