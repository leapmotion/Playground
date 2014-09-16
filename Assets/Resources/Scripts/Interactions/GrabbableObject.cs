/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class GrabbableObject : MonoBehaviour {

  public bool preferredOrientation = false;
  public Vector3 objectOrientation;
  public Vector3 palmOrientation;

  public Rigidbody[] ignoreOnGrab;

  public Rigidbody breakableJoint;
  public float breakForce;
  public float breakTorque;
  public bool rotateQuickly = true;

  protected bool grabbed_ = false;

  public bool IsGrabbed() {
    return grabbed_;
  }

  void OnJointBreak(float break_force) {
    for (int i = 0; i < ignoreOnGrab.Length; ++i) {
      // Clears ignore collision with flower.
      ignoreOnGrab[i].collider.enabled = false;
      ignoreOnGrab[i].collider.enabled = true;
    }
  }

  public virtual void OnGrab(){
    grabbed_ = true;
    for (int i = 0; i < ignoreOnGrab.Length; ++i)
      ignoreOnGrab[i].detectCollisions = false;

    if (breakableJoint != null) {
      Joint breakJoint = breakableJoint.GetComponent<Joint>();
      if (breakJoint != null) {
        breakJoint.breakForce = breakForce;
        breakJoint.breakTorque = breakTorque;
      }
    }
  }

  public virtual void OnRelease(){
    grabbed_ = false;
    for (int i = 0; i < ignoreOnGrab.Length; ++i)
      ignoreOnGrab[i].detectCollisions = true;

    if (breakableJoint != null) {
      Joint breakJoint = breakableJoint.GetComponent<Joint>();
      if (breakJoint != null) {
        breakJoint.breakForce = Mathf.Infinity;
        breakJoint.breakTorque = Mathf.Infinity;
      }
    }
  }

  void Update() {
    if (grabbed_ && breakableJoint != null) {
      Joint breakJoint = breakableJoint.GetComponent<Joint>();
      if (breakJoint != null) {
        breakJoint.breakForce = breakForce;
        breakJoint.breakTorque = breakTorque;
      }
    }
  }
}
