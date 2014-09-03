using UnityEngine;
using System.Collections;

public class FadeInOutAudio : MonoBehaviour {

  public float fadeInTime = 1.0f;
  public float fadeOutTime = 1.0f;
  public float maxVolume = 1.0f;
  public AnimationCurve fadeCurve;

  private bool fading_out_ = false;
  private float start_fade_volume_;
  private float start_fade_time_;

  void Start() {
    SetVolume(0.0f);
  }

  void SetVolume(float volume) {
    GetComponent<AudioSource>().volume = volume;
  }

  public void FadeOut() {
    fading_out_ = true;
    start_fade_volume_ = GetComponent<AudioSource>().volume;
    start_fade_time_ = Time.timeSinceLevelLoad;
  }

  void Update() {
    float time = Time.timeSinceLevelLoad;
    if (fading_out_) {
      float t = (Time.timeSinceLevelLoad - start_fade_time_) / fadeOutTime;
      SetVolume(start_fade_volume_ * (1.0f - fadeCurve.Evaluate(t)));
    }
    else if (time < fadeInTime)
      SetVolume(maxVolume * fadeCurve.Evaluate(time / fadeInTime));

  }
}
