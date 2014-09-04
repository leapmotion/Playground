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
  public float maxTransparency = 0.5f;

  private bool played = false;
  
  void Start() {
    HandController controller = GetComponent<HandController>();
    controller.enabled = false;
    controller.PauseRecording();
  }

  void Update() {
    if (Time.timeSinceLevelLoad < startTime)
      return;

    HandController controller = GetComponent<HandController>();
    if (!played) {
      controller.enabled = true;
      controller.StopRecording();
      controller.PlayRecording();
      played = true;
    }

    float progress = controller.GetRecordingProgress();

    if (progress >= 1.0f) {
      controller.DestroyAllHands();
      gameObject.SetActive(false);
    }

    float alpha = maxTransparency;
    if (progress < fadeInLength)
      alpha *= progress / fadeInLength;
    else if (1.0f - progress < fadeOutLength)
      alpha *= (1.0f - progress) / fadeOutLength;

    HandModel[] hands = controller.GetAllGraphicHands();
    foreach (HandModel hand in hands) {
      Renderer[] renderers = hand.GetComponentsInChildren<Renderer>();
      for (int i = 0; i < renderers.Length; ++i) {
        if (renderers[i].material.HasProperty("_Color")) {
          Color new_color = renderers[i].material.color;
          new_color.a = alpha;
          renderers[i].material.color = new_color;
        }
        if (renderers[i].material.HasProperty("_TintColor")) {
          Color new_color = renderers[i].material.GetColor("_TintColor");
          new_color.a = alpha;
          renderers[i].material.SetColor("_TintColor", new_color);
        }
      }
    }
  }
}
