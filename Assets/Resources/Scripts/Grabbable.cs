/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {

  public bool preferredOrientation = false;
  public Vector3 objectOrientation;
  public Vector3 palmOrientation;

  public Rigidbody[] ignoreOnGrab;

  public Joint breakableJoint;
  public float breakForce;
  public float breakTorque;

  protected bool grabbed_ = false;

  public bool IsGrabbed() {
    return grabbed_;
  }

  void OnJointBreak(float break_force) {
    for (int i = 0; i < ignoreOnGrab.Length; ++i)
      ignoreOnGrab[i].gameObject.layer = LayerMask.NameToLayer("Broken");
  }

  public virtual void OnGrab(){
    grabbed_ = true;
    for (int i = 0; i < ignoreOnGrab.Length; ++i)
      ignoreOnGrab[i].detectCollisions = false;

    if (breakableJoint != null) {
      breakableJoint.breakForce = breakForce;
      breakableJoint.breakTorque = breakTorque;
    }
  }

  public virtual void OnRelease(){
    grabbed_ = false;
    for (int i = 0; i < ignoreOnGrab.Length; ++i)
      ignoreOnGrab[i].detectCollisions = true;

    if (breakableJoint != null) {
      breakableJoint.breakForce = Mathf.Infinity;
      breakableJoint.breakTorque = Mathf.Infinity;
    }
  }
}
