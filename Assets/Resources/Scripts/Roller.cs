using UnityEngine;
using System.Collections;

public class Roller : MonoBehaviour {

  public Transform upright;

  public float angleForceScale = 40.0f;
  public float angularVelocityForceScale = 40.0f;
  public float velocityForceScale = 3.0f;
  public float maxAngularVelocity = 20.0f;
  public HingeJoint[] armJoints;
  public float restingDistance = -5.226256f;
  public float recoveringDistance = -2.0f;
  public float fallenWaitTime = 3.0f;

  private bool is_upright_ = true;
  private float fallen_time_ = 0.0f;

	void Start () {
    rigidbody.maxAngularVelocity = maxAngularVelocity;
    Physics.IgnoreCollision(collider, upright.collider, true);
	}

  public bool IsUpright() {
    return is_upright_;
  }
	
	void Update () {
    float rotation_angle = 0;
    Vector3 rotation_axis;
    Quaternion force_quaternion = Quaternion.FromToRotation(Vector3.up,
                                                            upright.transform.up);
    force_quaternion.ToAngleAxis(out rotation_angle, out rotation_axis);

    // You get knocked down, don't get up again.
    is_upright_ = rotation_angle < 60;

    if (is_upright_) {
      // Vector3 upright_angular_vel = upright.rigidbody.angularVelocity;
      Vector3 velocity_diff = rigidbody.velocity - upright.rigidbody.velocity;
      Vector3 velocity_diff_rotation = Vector3.Cross(velocity_diff, Vector3.up);
      Vector3 velocity_rotation = Vector3.Cross(rigidbody.velocity, Vector3.up);

      // This is pretty much hacked together. Change these but beware.
      Vector3 force =
          angleForceScale * Mathf.Tan(rotation_angle / 180.0f * Mathf.PI) * rotation_axis +
          angularVelocityForceScale * velocity_diff_rotation -
          velocityForceScale * velocity_rotation;
      rigidbody.AddTorque(force);

      /*
      foreach (HingeJoint arm_joint in armJoints) {
        arm_joint.useSpring = false;
      }
      */
      fallen_time_ = 0.0f;
    }
    else {
      /*
      if (fallen_time_ > fallenWaitTime) {
        foreach (HingeJoint arm_joint in armJoints) {
          arm_joint.useSpring = true;
        }
      }
      */
      fallen_time_ += Time.deltaTime;
    }
	}
}
