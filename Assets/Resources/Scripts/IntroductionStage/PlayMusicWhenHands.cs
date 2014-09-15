using UnityEngine;
using System.Collections;

public class PlayMusicWhenHands : MonoBehaviour {

  public float filtering = 0.9f;
  public float maxVolume = 1.0f;

  private float volume_ = 0.0f;
  private int current_hands_ = 0;

  void Start() {
    audio.volume = 0.0f;
  }

  void OnEnable() {
    HandCountNotifier.OnHandChange += HandChange;
  }

  void OnDisable() {
    HandCountNotifier.OnHandChange -= HandChange;
  }

  void HandChange(int total, int current) {
    current_hands_ = current;
  }

  void Update() {
    if (current_hands_ > 0)
      volume_ += (maxVolume - volume_) * (1.0f - filtering);
    else
      volume_ *= filtering;

    GetComponent<FadeInOutAudio>().maxVolume = volume_;
  }
}
