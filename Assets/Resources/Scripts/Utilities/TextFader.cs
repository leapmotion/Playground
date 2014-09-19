/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class TextFader : MonoBehaviour {

  public float textFadeInTime = 5.0f;
  public float textFadeOutTime = 5.0f;
  public float waitTime = 10.0f;
  public AnimationCurve fadeCurve;
  public float maxAlpha = 0.5f;
  public bool showOnlyWhenNoHands = true;
  public bool showOnlyAfterUse = true;
  public bool dontShowAnymore = false;
  public HandController controller;

  private float text_alpha_ = 0.0f;
  private float time_connected_ = 0.0f;
  private bool shown_hands_ = false;
  private bool exiting_ = false;

  void Start() {
    SetTextAlpha(0.0f);
  }

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += Exit;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= Exit;
  }

  void Exit() {
    exiting_ = true;
  }

  void SetTextAlpha(float alpha) {
    GUIText text = GetComponent<GUIText>();
    Color color = text.color;
    color.a = maxAlpha * alpha;
    text.color = color;
  }
  
  bool ShouldShowText() {
    return time_connected_ >= waitTime && !exiting_ && !dontShowAnymore &&
           (!showOnlyWhenNoHands || controller.GetAllGraphicsHands().Length == 0) &&
           (!showOnlyAfterUse || shown_hands_);
  }

  void Update() {
    shown_hands_ = shown_hands_ || controller.GetAllGraphicsHands().Length > 0;

    if (controller != null && controller.IsConnected())
      time_connected_ += Time.deltaTime;
    else
      time_connected_ = 0.0f;

    if (ShouldShowText())
      text_alpha_ = Mathf.Clamp(text_alpha_ + Time.deltaTime / textFadeInTime, 0.0f, 1.0f);
    else
      text_alpha_ = Mathf.Clamp(text_alpha_ - Time.deltaTime / textFadeOutTime, 0.0f, 1.0f);

    SetTextAlpha(fadeCurve.Evaluate(text_alpha_));
  }
}
