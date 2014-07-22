using UnityEngine;
using System.Collections;

public class RobotBody : MonoBehaviour {

  private const int FORCE_MAGNITUDE = 8;

	void FixedUpdate () {
    if (Input.GetKey(KeyCode.UpArrow))
      rigidbody.AddForce(FORCE_MAGNITUDE * Vector3.forward);
    if (Input.GetKey(KeyCode.DownArrow))
      rigidbody.AddForce(FORCE_MAGNITUDE * Vector3.back);
    if (Input.GetKey(KeyCode.LeftArrow))
      rigidbody.AddForce(FORCE_MAGNITUDE * Vector3.left);
    if (Input.GetKey(KeyCode.RightArrow))
      rigidbody.AddForce(FORCE_MAGNITUDE * Vector3.right);
    if (Input.GetKey(KeyCode.Space))
      rigidbody.AddForce(-30 * FORCE_MAGNITUDE * transform.up);
	
	}
}
