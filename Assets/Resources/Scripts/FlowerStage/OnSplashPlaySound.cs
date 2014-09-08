using UnityEngine;
using System.Collections;

public class OnSplashPlaySound : MonoBehaviour {

  public AudioClip[] soundBank;
  public float waterLevel = 0.0f;
  public float waterSplashThickness = 0.3f;
  public float minSpeedForSplash = 0.1f;
  public float unitVolumePerUnitSecond = 0.1f;
  public float waitForNextSplash = 0.5f;

  private float splash_charge_ = 0.0f;

  void Update() {
    if (transform.position.y <= waterLevel &&
        transform.position.y >= waterLevel - waterSplashThickness) {
      float speed = rigidbody.velocity.magnitude;
      float volume = (speed - minSpeedForSplash) * unitVolumePerUnitSecond;
      if (volume > 0.0f && splash_charge_ > waitForNextSplash) {
        audio.PlayOneShot(soundBank[Random.Range(0, soundBank.Length)], volume);
        splash_charge_ = 0.0f;
      }
    }
    splash_charge_ += Time.deltaTime;
  }
}
