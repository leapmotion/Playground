using UnityEngine;
using System.Collections;
using Leap;

public class ParticleEngine : MonoBehaviour {

  public int numberOfParticles = 1000;
  public float minSpeed = 1.0f;
  public float dampen = 0.5f;
  public float attraction = 0.1f;
  public float size = 0.5f;
  public Color slowColor = Color.white;
  public Color fastColor = Color.black;
  
  private Controller leap_controller_;
  private ParticleSystem particle_system_;
  
  private const float height_ = 8.0f;

	// Use this for initialization
	void Start () {
    leap_controller_ = new Controller();
    particle_system_ = (ParticleSystem)(GameObject.Find("Particles").GetComponent(typeof(ParticleSystem)));
	}
	
	// Update is called once per frame
	void Update () {
    // // Set particle system variables
    // particle_system_.maxParticles = numberOfParticles;
    // particle_system_.emissionRate = numberOfParticles / lifetime;
    // particle_system_.startSpeed = minSpeed;
    // particle_system_.startLifetime = lifetime;
    // particle_system_.startSize = size;
    
    // ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particle_system_.particleCount];
    // particle_system_.GetParticles(particles);
    // Vector3 starting_pos = particle_system_.transform.position - new Vector3(0.0f,0.0f,0.5f);
    
    // List<Vector3> list_palm_vel = new List<Vector3>();
    // List<Vector3> list_palm_pos = new List<Vector3>();
    // Frame frame = leap_controller_.Frame();
    // for (int i = 0; i < frame.Hands.Count; ++i) {
      // Vector3 temp_pos = frame.Hands[i].PalmPosition.ToUnityScaled()*10;
      // Vector3 palm_pos = new Vector3(temp_pos.x, -temp_pos.z, temp_pos.y);
      // list_palm_pos.push_back(palm_pos);
      
      // Vector3 temp_vel = frame.Hands[i].PalmVelocity.ToUnityScaled();
      // Vector3 palm_vel = new Vector3(temp_vel.x, -temp_vel.z, temp_vel.y);
      // list_palm_vel.push_back(palm_vel);
    // }
    
    // for (int i = 0; i < particles.Length; ++i) {
      // particles.startLifetime = particle_system_.startLifetime;
      // particles.size = particle_system_.startSize;
      // Vector3 starting_vel = (particles[j].position - starting_pos).normalized * minSpeed;
      // float distance = Vector3.Distance(particles[j].position, palm_pos);
      // float magnitude = palm_vel.magnitude;
      // float ratio = Mathf.Min(0.5f, magnitude / Mathf.Pow(distance,6));
      // particles[j].velocity = ratio * palm_vel*2 + (1-ratio) * particles[j].velocity;
      // Vector3 starting_vel = (particles[i].position - starting_pos).normalized * minSpeed;
      // particles[i].velocity = 0.95f*particles[i].velocity + 0.05f*starting_vel;
    // }
    // particle_system_.SetParticles(particles, particles.Length);
	}
}
