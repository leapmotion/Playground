using UnityEngine;
using System.Collections;

public class OnBreakPlaySound : MonoBehaviour {

  public AudioClip[] soundBank;

  void OnJointBreak() {
    audio.PlayOneShot(soundBank[Random.Range(0, soundBank.Length)]);
  }
}
