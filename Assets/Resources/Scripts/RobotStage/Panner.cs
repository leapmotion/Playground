/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class Panner : MonoBehaviour {

  public Vector3 panAmount = Vector3.zero;
  public float panFrequency = 1.0f;

  Vector3 default_euler_angles_;

  void Start() {
    default_euler_angles_ = transform.eulerAngles;
  }

  void Update() {
    float t = Mathf.Sin(Time.timeSinceLevelLoad * panFrequency * 2 * Mathf.PI);
    transform.eulerAngles = default_euler_angles_ + t * panAmount;
  }
}
