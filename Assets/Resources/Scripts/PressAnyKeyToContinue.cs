/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class PressAnyKeyToContinue : MonoBehaviour {

  public float enableWaitTime = 1.0f;
  public float textFadeTime = 1.0f;
  public float fadeInTime = 2.0f;
  public float fadeOutTime = 2.0f;
  public Renderer blackFade;
  public AnimationCurve fadeCurve;
  public float maxAlpha = 0.5f;
  public FadeInOutAudio[] audioFaders;
  public FadeInOutMaterial[] materialFaders;

  private bool showing_ = false;
  private float show_start_time_;
  private bool pressed = false;
  private float time_since_pressed_ = 0.0f;

  void Start() {
    SetTextAlpha(0.0f);
  }

	void OnGUI() {
    if (Event.current.type == EventType.KeyDown && Time.timeSinceLevelLoad >= enableWaitTime) {
      pressed = true;
      foreach (FadeInOutMaterial material_fader in materialFaders) {
        material_fader.FadeOut();
      }
      foreach (FadeInOutAudio audio_fader in audioFaders) {
        audio_fader.FadeOut();
      }
    }
  }

  public void Show() {
    if (showing_)
      return;
    showing_ = true;
    show_start_time_ = Time.timeSinceLevelLoad;
  }

  void SetTextAlpha(float alpha) {
    GUIText text = GetComponent<GUIText>();
    Color color = text.color;
    color.a = maxAlpha * alpha;
    text.color = color;
  }

  void SetBlackAlpha(float alpha) {
    Color color = blackFade.material.color;
    color.a = alpha;
    blackFade.material.color = color;
  }

  void Update() {
    SetBlackAlpha(1.0f - fadeCurve.Evaluate(Time.timeSinceLevelLoad / fadeInTime));
    if (showing_) {
      float time = Time.timeSinceLevelLoad - show_start_time_;
      SetTextAlpha(fadeCurve.Evaluate(time / textFadeTime));
    }

    if (!pressed)
      return;

    if (time_since_pressed_ >= fadeOutTime)
      Application.LoadLevel(Application.loadedLevel + 1);

    time_since_pressed_ += Time.deltaTime;
    SetBlackAlpha(fadeCurve.Evaluate(time_since_pressed_ / fadeOutTime));
    SetTextAlpha(1.0f - fadeCurve.Evaluate(time_since_pressed_ / fadeOutTime));
  }
}

