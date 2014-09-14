/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

  public Vector3 rotateVector;
  public float degreesPerSecond;
  public bool localSpace = false;

  void Start() {
    if (localSpace)
      rotateVector = transform.TransformDirection(rotateVector);
  }

  void Update () {
    float angle = Time.deltaTime * degreesPerSecond;
    Vector3 vector = rotateVector;
    transform.rotation = transform.rotation * Quaternion.AngleAxis(angle, vector);
  }
}
