using UnityEngine;
using System.Collections;

public class Splasher : MonoBehaviour {

  public float waterLevel = 0.0f;
  public float waterSplashThickness = 0.3f;

  public delegate void NotifySplash(float speed);
  public static event NotifySplash OnSplash;

  void Update() {
    if (transform.position.y <= waterLevel &&
        transform.position.y >= waterLevel - waterSplashThickness) {
      OnSplash(rigidbody.velocity.magnitude);
    }
  }
}
