using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {

  public float maxAngle = 20.0f;
  public float frequency = 1.0f;
  
  private float phase_ = 0.0f;

  void Update () {
    phase_ += 2 * Mathf.PI * frequency * Time.deltaTime;
    float angle = maxAngle * Mathf.Cos(phase_);
    transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
  }
}
