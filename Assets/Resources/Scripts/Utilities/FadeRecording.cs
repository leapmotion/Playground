/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class FadeRecording : MonoBehaviour {

  public float startTime = 3.0f;
  public float fadeInLength = 0.01f;
  public float fadeOutLength = 0.01f;
  public float forceFadeDecay = 0.8f;
  public float maxTransparency = 0.5f;
  public bool onlyPlayIfNoHands = false;
  public HandController live_controller_;

  public Renderer[] additionalObjects;

  private bool played_ = false;
  private bool exiting_ = false;
  private float force_fade_ = 1.0f;
  
  void Start() {
    HandController controller = GetComponent<HandController>();
    controller.enabled = false;
    controller.PauseRecording();
  }

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += StartExiting;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= StartExiting;
  }

  void StartExiting() {
    exiting_ = true;
  }

  void SetAlphaOfRenderer(Renderer rend, float alpha) {
    if (rend.material.HasProperty("_Color")) {
      Color new_color = rend.material.color;
      new_color.a = alpha;
      rend.material.color = new_color;
    }
    if (rend.material.HasProperty("_TintColor")) {
      Color new_color = rend.material.GetColor("_TintColor");
      new_color.a = alpha;
      rend.material.SetColor("_TintColor", new_color);
    }
  }

  void Update() {
    if (Time.timeSinceLevelLoad < startTime)
      return;

    HandController controller = GetComponent<HandController>();
    if (!played_ && controller.IsConnected()) {
      controller.enabled = true;
      controller.StopRecording();
      controller.PlayRecording();
      played_ = true;
    }

    float progress = controller.GetRecordingProgress();

    if (live_controller_ != null &&
        live_controller_.GetAllGraphicsHands().Length > 0 &&
        onlyPlayIfNoHands) {
      force_fade_ *= forceFadeDecay;
    }
    else if (exiting_) 
      force_fade_ *= forceFadeDecay;
    else
      force_fade_ = 1.0f - forceFadeDecay * (1.0f - force_fade_);

    if (force_fade_ < 0.01f)
      progress = 1.0f;

    if (progress >= 1.0f) {
      controller.DestroyAllHands();
      for (int i = 0; i < additionalObjects.Length; ++i)
        Destroy(additionalObjects[i].gameObject);
      gameObject.SetActive(false);
      return;
    }

    float alpha = force_fade_ * maxTransparency;
    if (progress < fadeInLength)
      alpha *= progress / fadeInLength;
    else if (1.0f - progress < fadeOutLength)
      alpha *= (1.0f - progress) / fadeOutLength;

    HandModel[] hands = controller.GetAllGraphicsHands();
    foreach (HandModel hand in hands) {
      Renderer[] renderers = hand.GetComponentsInChildren<Renderer>();
      for (int i = 0; i < renderers.Length; ++i)
        SetAlphaOfRenderer(renderers[i], alpha);
      for (int i = 0; i < additionalObjects.Length; ++i)
        SetAlphaOfRenderer(additionalObjects[i], alpha);
    }
  }
}
