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
  public GUITexture blackFade;
  public AnimationCurve fadeCurve;
  public float maxAlpha = 0.5f;
  public HandController controller;

  private bool showing_ = false;
  private float show_start_time_;
  private bool pressed_ = false;
  private float time_since_pressed_ = 0.0f;
  private float time_connected_ = 0.0f;

  public delegate void NextLevelAction();
  public static event NextLevelAction OnContinue;

  void Start() {
    SetTextAlpha(0.0f);
  }

	void OnGUI() {
    if (Event.current.type == EventType.KeyDown && time_connected_ >= enableWaitTime &&
        !pressed_) {
      pressed_ = true;
      if (OnContinue != null)
        OnContinue();
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
    blackFade.color = Color.Lerp(Color.clear, Color.black, alpha);
  }

  void Update() {
    if (controller.IsConnected())
      time_connected_ += Time.deltaTime;
    else
      time_connected_ = 0.0f;

    SetBlackAlpha(fadeCurve.Evaluate(1.0f - Time.timeSinceLevelLoad / fadeInTime));
    if (showing_) {
      float time = Time.timeSinceLevelLoad - show_start_time_;
      SetTextAlpha(fadeCurve.Evaluate(time / textFadeTime));
    }

    if (!pressed_)
      return;

    if (time_since_pressed_ >= fadeOutTime)
      Application.LoadLevel(Application.loadedLevel + 1);

    time_since_pressed_ += Time.deltaTime;
    SetBlackAlpha(fadeCurve.Evaluate(time_since_pressed_ / fadeOutTime));
    SetTextAlpha(1.0f - fadeCurve.Evaluate(time_since_pressed_ / fadeOutTime));
  }
}

