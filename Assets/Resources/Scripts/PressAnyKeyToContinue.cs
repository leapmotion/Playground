/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class PressAnyKeyToContinue : MonoBehaviour {

  public float waitTime = 1.0f;
  public float fadeOutTime = 2.0f;
  public Renderer blackFade;
  public AnimationCurve fadeCurve;

  private bool pressed = false;
  private float time_since_pressed_ = 0.0f;

	void OnGUI() {
    if (Event.current.type == EventType.KeyDown && Time.timeSinceLevelLoad >= waitTime)
      pressed = true;
  }

  void Update() {
    if (!pressed)
      return;

    if (time_since_pressed_ >= fadeOutTime)
      Application.LoadLevel(Application.loadedLevel + 1);

    time_since_pressed_ += Time.deltaTime;
    Color color = blackFade.material.color;
    color.a = fadeCurve.Evaluate(time_since_pressed_ / fadeOutTime);
    blackFade.material.color = color;
  }
}

