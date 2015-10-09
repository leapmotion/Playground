/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ForceFieldBounce : MonoBehaviour {

  public float fieldLoudness = 5.0f;
  public float otherLoudness = 5.0f;
  public AudioClip bounceClip;
  public AudioClip floorClip;

  void OnCollisionEnter(Collision collision) {
    if (collision.collider.GetComponent<ForceField>() != null) {
      float strength = collision.relativeVelocity.magnitude;
      GetComponent<AudioSource>().PlayOneShot(bounceClip, fieldLoudness - 1.0f / strength);
    }
    else {
      float strength = collision.relativeVelocity.magnitude;
      GetComponent<AudioSource>().PlayOneShot(floorClip, otherLoudness - 1.0f / strength);
    }
  }
}
