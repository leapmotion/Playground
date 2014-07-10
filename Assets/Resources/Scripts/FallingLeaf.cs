using UnityEngine;
using System.Collections;

public class FallingLeaf : MonoBehaviour {

  public float dragForce = 0.1f;
  public float dragTorque = 0.0003f;

  void FixedUpdate () {
    Vector3 velocity = rigidbody.velocity;
    Vector3 normal = transform.up;

    float dot = Vector3.Dot(velocity, normal);
    rigidbody.AddForce(-normal * dragForce * dot);

    Vector3 cross = Vector3.Cross(velocity, normal);
    rigidbody.AddTorque(-dragTorque * cross);
  }
}
