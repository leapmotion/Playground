using UnityEngine;
using System.Collections;

public class StartFireworks : MonoBehaviour {

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += PlayFireworks;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= PlayFireworks;
  }

  void PlayFireworks() {
    ParticleSystem fireworks = GetComponent<ParticleSystem>();
    fireworks.Play();
  }
}
