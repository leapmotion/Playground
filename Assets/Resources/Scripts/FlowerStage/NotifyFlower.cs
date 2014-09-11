using UnityEngine;
using System.Collections;

public class NotifyFlower : MonoBehaviour {

  public delegate void NotifyBreak(bool isPedal);
  public static event NotifyBreak OnFlowerBreak;

  public delegate void NotifyRustle(float strength);
  public static event NotifyRustle OnFlowerRustle;

  public bool isPedal = false;

  void OnJointBreak() {
    OnFlowerBreak(isPedal);
  }

  void OnCollisionEnter(Collision collision) {
    Rigidbody joint = GetComponent<GrabbableObject>().breakableJoint;
    if (joint != null && joint.hingeJoint != null)
      OnFlowerRustle(collision.relativeVelocity.magnitude);
  }
}
