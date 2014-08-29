/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class FadeRecording : MonoBehaviour {

  public float startTime = 0.0f;
  public float fadeInLength = 0.01f;
  public float fadeOutLength = 0.01f;
  public float maxTransparency = 0.5f;
  public Material material;
  
  void Start() {
  }

  void Update() {
    HandController controller = GetComponent<HandController>();
    float progress = controller.GetRecordingProgress();

    float alpha = maxTransparency;
    if (progress < fadeInLength)
      alpha *= progress / fadeInLength;
    else if (1.0f - progress < fadeOutLength)
      alpha *= (1.0f - progress) / fadeOutLength;

    HandModel[] hands = controller.GetAllGraphicHands();
    foreach (HandModel hand in hands) {
      Renderer[] renderers = hand.GetComponentsInChildren<Renderer>();
      for (int i = 0; i < renderers.Length; ++i) {
        Color new_color = renderers[i].material.color;
        new_color.a = alpha;
        renderers[i].material.color = new_color;
      }
    }
  }
}
