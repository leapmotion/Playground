/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ScaleEmissionFromVelocity : MonoBehaviour {

  public float minEmissionRate = 0.0f;
  public float startEmittingSpeed = 0.1f;
  public float ratePerUnitPerSecond = 10.0f;

  private Vector3 last_position = Vector3.zero;

  void Start () {
    GetComponent<ParticleSystem>().emissionRate = 0.0f;
  }

  void Update () {
    if (last_position == Vector3.zero)
      last_position = transform.position;

    float speed = (transform.position - last_position).magnitude / Time.deltaTime;
    GetComponent<ParticleSystem>().emissionRate = (speed - startEmittingSpeed) * ratePerUnitPerSecond;
    last_position = transform.position;
  }
}
