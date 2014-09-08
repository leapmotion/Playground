/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class ParticleSpawnHandHandler : MonoBehaviour {

  public float fadeOutTime = 0.1f;
  public float fadeOut = 0.99f;

  public float fadeOutStartTime = 5.0f;

  private bool enable_emission_ = false;
  private float time_alive_ = 0.0f;

  void LateUpdate() {
    time_alive_ += Time.deltaTime;
    bool play_particles = GetComponent<HandModel>().GetLeapHand() != null &&
                          time_alive_ <= fadeOutStartTime;
    ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

    if (play_particles) {
      foreach (ParticleSystem system in particles) {
        system.enableEmission = enable_emission_;
        if (!system.isPlaying) {
          system.Play();
        }
      }
      enable_emission_ = true;
    }
    else {
      foreach (ParticleSystem system in particles) {
        system.emissionRate = 0.0f;
        int num_particles = system.particleCount;
        ParticleSystem.Particle[] parts = new ParticleSystem.Particle[num_particles];
        system.GetParticles(parts);
        for (int i = 0; i < num_particles; ++i) {
          parts[i].lifetime *= fadeOut;
          Color color = parts[i].color;
          color.a = color.a * fadeOut;
          parts[i].color = color;
        }

        system.SetParticles(parts, num_particles);
      }
    }
  }
}
