/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class PressAnyKeyToContinue : MonoBehaviour {

  public float enableWaitTime = 1.0f;
  public float fadeInTime = 2.0f;
  public float fadeOutTime = 2.0f;
  public GUITexture blackFade;
  public AnimationCurve fadeCurve;
  public HandController controller;

  private bool pressed_ = false;
  private float time_since_pressed_ = 0.0f;
  private float time_connected_ = 0.0f;

  public delegate void NextLevelAction();
  public static event NextLevelAction OnContinue;

	void OnGUI() {
    if (Event.current.type == EventType.KeyDown && time_connected_ >= enableWaitTime &&
        !pressed_) {
      pressed_ = true;
      if (OnContinue != null)
        OnContinue();
    }
  }

  void SetBlackAlpha(float alpha) {
    blackFade.color = Color.Lerp(Color.clear, Color.black, alpha);
  }

  void PressedUpdate() {
    if (time_since_pressed_ >= fadeOutTime) {
      if (Application.loadedLevel + 1 < Application.levelCount)
        Application.LoadLevel(Application.loadedLevel + 1);
      else
        Application.Quit();
    }

    time_since_pressed_ += Time.deltaTime;
    SetBlackAlpha(fadeCurve.Evaluate(time_since_pressed_ / fadeOutTime));
  }

  void NormalUpdate() {
    if (controller.IsConnected())
      time_connected_ += Time.deltaTime;
    else
      time_connected_ = 0.0f;

    SetBlackAlpha(fadeCurve.Evaluate(1.0f - Time.timeSinceLevelLoad / fadeInTime));
  }

  void Update() {
    if (pressed_)
      PressedUpdate();
    else
      NormalUpdate();
  }
}
