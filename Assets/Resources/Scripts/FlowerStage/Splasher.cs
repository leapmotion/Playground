/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class Splasher : MonoBehaviour {

  public float waterLevel = 0.0f;
  public float waterSplashThickness = 0.3f;

  public delegate void NotifySplash(float speed);
  public static event NotifySplash OnSplash;

  void Update() {
    if (transform.position.y <= waterLevel &&
        transform.position.y >= waterLevel - waterSplashThickness) {
      OnSplash(GetComponent<Rigidbody>().velocity.magnitude);
    }
  }
}
