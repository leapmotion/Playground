/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ForceGC : MonoBehaviour {

  public int frames = 10;

  void Update() {
    if (Time.frameCount % frames == 0) {
      System.GC.Collect();
    }
  }
}
