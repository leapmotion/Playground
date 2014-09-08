/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class WaitToShow : MonoBehaviour {

  public float showWaitTime = 0.0f;
  
  private float time_waited_ = 0.0f;

  void Start() {
    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (Renderer render in renderers) {
      render.enabled = false;
    }
  }

  void Update() {
    time_waited_ += Time.deltaTime;
    bool enabled = time_waited_ >= showWaitTime;

    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (Renderer render in renderers) {
      render.enabled = enabled;
    }
  }
}
