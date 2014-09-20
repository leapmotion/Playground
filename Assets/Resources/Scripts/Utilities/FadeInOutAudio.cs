/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class FadeInOutAudio : MonoBehaviour {

  public float fadeInWaitTime = 0.0f;
  public float fadeInTime = 1.0f;
  public float fadeOutWaitTime = 0.0f;
  public float fadeOutTime = 1.0f;
  public float maxVolume = 1.0f;
  public AnimationCurve fadeCurve;
  public bool onlyPlayIfFirstStage = false;

  public int activeStages = 1;

  private float progress_ = 0.0f;
  private bool fading_out_ = false;
  private int start_stage_ = 0;
  private float time_since_fade_out_ = 0.0f;

  void Start() {
    if (onlyPlayIfFirstStage && Application.loadedLevel > 0)
      enabled = false;
    SetVolume(0.0f);
    start_stage_ = Application.loadedLevel;
  }

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += OnContinue;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= OnContinue;
  }

  void SetVolume(float volume) {
    GetComponent<AudioSource>().volume = volume;
  }

  public void FadeOut() {
    fading_out_ = true;
  }

  public void FadeIn() {
    fading_out_ = false;
  }

  public void OnContinue() {
    if (Application.loadedLevel >= activeStages + start_stage_ - 1)
      FadeOut();
  }

  void Update() {
    if (fading_out_) {
      time_since_fade_out_ += Time.deltaTime;
      if (time_since_fade_out_ >= fadeOutWaitTime)
        progress_ -= Time.deltaTime / fadeOutTime;
    }
    else if (Time.timeSinceLevelLoad >= fadeInWaitTime)
      progress_ += Time.deltaTime / fadeInTime;
    
    progress_ = Mathf.Clamp(progress_, 0.0f, 1.0f);
    SetVolume(maxVolume * fadeCurve.Evaluate(progress_));
  }
}
