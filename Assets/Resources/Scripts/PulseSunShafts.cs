/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class PulseSunShafts : MonoBehaviour {

  public float minRadius = 0.6f;
  public float maxRadius = 0.9f;
  public float pulseFrequency = 0.5f;

  void Update () {
    SunShafts shafts = GetComponent<SunShafts>();
    float phase = Time.timeSinceLevelLoad * pulseFrequency * Mathf.PI * 2;
    float t = (Mathf.Sin(phase) + 1.0f) / 2.0f;
    shafts.maxRadius = minRadius + t * (maxRadius - minRadius);
  }
}
