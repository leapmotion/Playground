/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ForceFieldBounce : MonoBehaviour {

  public float loudness = 5.0f;
  public AudioClip bounceClip;

  void OnCollisionEnter(Collision collision) {
    if (collision.collider.GetComponent<ForceField>() != null) {
      float strength = loudness * collision.relativeVelocity.magnitude;
      audio.PlayOneShot(bounceClip, 1.0f - 1.0f / strength);
    }
  }
}
