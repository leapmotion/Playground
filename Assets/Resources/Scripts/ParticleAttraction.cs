using UnityEngine;
using System.Collections;
using Leap;

public class ParticleAttraction : MonoBehaviour {

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

    for (int i = 0; i < num_particles; ++i) {
      float t = i / (num_particles - 1.0f);
      float attraction_force = minAttractionForce + t * (maxAttractionForce - minAttractionForce);
      Vector3 delta = attractionPosition - particles[i].position;
      float attract_distance = delta.magnitude + minAttractionDistance;
      float repulse_distance = delta.magnitude + minRepulsionDistance;
      float force = attraction_force / (attract_distance);
      force -= repulsionForce / (repulse_distance);
      particles[i].velocity += force * delta.normalized * Time.deltaTime;

      Vector3 rotationForceDirection = Vector3.Cross(attractionDirection, delta.normalized);
      particles[i].velocity += rotationalForce * rotationForceDirection * Time.deltaTime;

      particles[i].velocity *= (1.0f - damping);
    }

    particleSystem.SetParticles(particles, num_particles);
  }
}
