/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ReattachHingeWhenClose : MonoBehaviour {

  public Rigidbody connection;
  public float reconnectDistance = 1.0f;
  public Vector3 anchor = Vector3.zero;
  public Vector3 axis = Vector3.up;

  public bool useSpring = false;
  public float springForce = 1.0f;
  public float springDampen = 1.0f;
  public float springTarget = 0.0f;

  public bool useLimits = false;

  private Vector3 connected_anchor_;
  private Quaternion start_rotation_;

  void Start() {
    connected_anchor_ = GetComponent<HingeJoint>().connectedAnchor;
    start_rotation_ = transform.rotation;
  }

  void Update() {
    if (GetComponent<HingeJoint>() == null && connection != null) {
      float distance = (connection.transform.position - transform.position).magnitude;
      if (distance <= reconnectDistance) {
        transform.rotation = connection.transform.rotation * start_rotation_;
        HingeJoint new_joint = gameObject.AddComponent<HingeJoint>();
        new_joint.connectedBody = connection;
        new_joint.anchor = anchor;
        new_joint.axis = axis;
        new_joint.useSpring = useSpring;
        new_joint.useLimits = useLimits;
        new_joint.autoConfigureConnectedAnchor = false;
        new_joint.connectedAnchor = connected_anchor_;

        JointSpring new_joint_spring = new_joint.spring;
        new_joint_spring.spring = springForce;
        new_joint_spring.damper = springDampen;
        new_joint_spring.targetPosition = springTarget;
        new_joint.spring = new_joint_spring;
      }
    }
  }
}
