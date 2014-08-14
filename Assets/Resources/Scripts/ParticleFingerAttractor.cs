using UnityEngine;
using System.Collections;
using Leap;

public class ParticleFingerAttractor : MonoBehaviour {

  public ParticleAttraction2 particles;
  
  void FixedUpdate () {
    Vector3 tip_position = GetComponent<FingerModel>().GetTipPosition();
    Vector3 tip_direction = GetComponent<FingerModel>().GetBoneDirection(3);
    particles.attractionPosition = tip_position;
    particles.attractionDirection = tip_direction;
  }
}
