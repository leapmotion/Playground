using UnityEngine;
using System.Collections;

public class FadeInOutAudio : MonoBehaviour {

  public float fadeInTime = 1.0f;
  public float fadeOutTime = 1.0f;
  public float maxVolume = 1.0f;
  public AnimationCurve fadeCurve;
  public bool onlyPlayIfFirstStage = false;

  private bool fading_out_ = false;
  private float start_fade_volume_;
  private float start_fade_time_;
  private float time_alive_ = 0.0f;

  void Start() {
    if (onlyPlayIfFirstStage && Application.loadedLevel > 0)
      enabled = false;
    SetVolume(0.0f);
  }

  void SetVolume(float volume) {
    GetComponent<AudioSource>().volume = volume;
  }

  public void FadeOut() {
    fading_out_ = true;
    start_fade_volume_ = GetComponent<AudioSource>().volume;
    start_fade_time_ = time_alive_;
  }

  void Update() {
    time_alive_ += Time.deltaTime;
    if (fading_out_) {
      float t = (time_alive_ - start_fade_time_) / fadeOutTime;
      SetVolume(start_fade_volume_ * (1.0f - fadeCurve.Evaluate(t)));
    }
    else if (time_alive_ < fadeInTime)
      SetVolume(maxVolume * fadeCurve.Evaluate(time_alive_ / fadeInTime));

  }
}
