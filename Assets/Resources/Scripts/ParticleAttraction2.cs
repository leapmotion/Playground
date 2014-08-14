using UnityEngine;
using System.Collections;
using Leap;

public class ParticleAttraction2 : MonoBehaviour {

  public float minAttractionForce = 1.0f;
  public float maxAttractionForce = 1.0f;
  public float repulsionForce = 1.0f;
  public float minAttractionDistance = 0.2f;
  public float minRepulsionDistance = 1.0f;
  public float damping = 0.0f;
  public Vector3 attractionPosition;
  public Vector3 attractionDirection;

  public float rotationalForce = 1.0f;
  
  private const float height_ = 8.0f;

  void Start () {
  }
  
  void FixedUpdate () {
    if (particleSystem == null)
      return;

    int num_particles = particleSystem.particleCount;
    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[num_particles];
    particleSystem.GetParticles(particles);

    if (num_particles > 0) {
      Vector3 delta = attractionPosition - particles[0].position;
      float attract_distance = delta.magnitude + minAttractionDistance;
      float force = maxAttractionForce * (attract_distance);
      particles[0].velocity += force * delta.normalized * Time.deltaTime;
      particles[0].velocity *= (1.0f - damping);
    }

    for (int i = 1; i < num_particles; ++i) {
      float t = i / (num_particles - 1.0f);
      float attraction_force = maxAttractionForce + t * (minAttractionForce - maxAttractionForce);

      Vector3 next_delta = particles[i - 1].position - particles[i].position;
      float next_attract_distance = next_delta.magnitude + minAttractionDistance;
      float next_force = attraction_force * (next_attract_distance);
      particles[i].velocity += next_force * next_delta.normalized * Time.deltaTime;

      particles[i].velocity *= (1.0f - damping);
    }

    particleSystem.SetParticles(particles, num_particles);
  }
}
