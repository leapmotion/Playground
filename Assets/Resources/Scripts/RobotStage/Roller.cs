/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class Roller : MonoBehaviour {

  public RobotBody upright;

  public float angleForceScale = 40.0f;
  public float angularVelocityForceScale = 40.0f;
  public float velocityForceScale = 3.0f;
  public float maxAngularVelocity = 20.0f;
  public HingeJoint[] armJoints;
  public float restingDistance = -5.226256f;
  public float recoveringDistance = -2.0f;

  void Start () {
    GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;
    Physics.IgnoreCollision(GetComponent<Collider>(), upright.GetComponent<Collider>(), true);
  }

  public bool IsUpright() {
    return upright.IsUpright();
  }
  
  void FixedUpdate () {
    if (upright.IsUpright()) {
      float rotation_angle = 0;
      Vector3 rotation_axis;
      Quaternion force_quaternion = Quaternion.FromToRotation(Vector3.up,
          upright.transform.up);
      force_quaternion.ToAngleAxis(out rotation_angle, out rotation_axis);

      Vector3 velocity_diff = GetComponent<Rigidbody>().velocity - upright.GetComponent<Rigidbody>().velocity;
      Vector3 velocity_diff_rotation = Vector3.Cross(velocity_diff, Vector3.up);
      Vector3 velocity_rotation = Vector3.Cross(GetComponent<Rigidbody>().velocity, Vector3.up);

      // This is pretty much hacked together. Change these but beware.
      Vector3 force =
          angleForceScale * Mathf.Tan(rotation_angle / 180.0f * Mathf.PI) * rotation_axis +
          angularVelocityForceScale * velocity_diff_rotation -
          velocityForceScale * velocity_rotation;
      GetComponent<Rigidbody>().AddTorque(force * Time.deltaTime * 50);
    }
  }
}
