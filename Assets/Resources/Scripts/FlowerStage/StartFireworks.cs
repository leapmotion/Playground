using UnityEngine;
using System.Collections;

public class StartFireworks : MonoBehaviour {

  void Update () {
    if (Input.GetKeyDown("s")) {
      ParticleSystem fireworks = GetComponent<ParticleSystem>();
      if (fireworks != null)
        fireworks.Play();
    }

  }
}
