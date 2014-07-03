using UnityEngine;
using System.Collections;

public class FallingLeaf : MonoBehaviour {

  public float dragForce = 0.1f;

  void FixedUpdate () {
    Vector3 velocity = rigidbody.velocity;
    Vector3 normal = transform.up;

    rigidbody.AddForce(-normal * Vector3.Dot(velocity, normal) / 10.0f);
  }
}
