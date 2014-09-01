/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class PressAnyKeyToContinue : MonoBehaviour {

  public float waitTime = 1.0f;

	public void OnGUI() {
    if (Event.current.type == EventType.KeyDown && Time.timeSinceLevelLoad >= waitTime)
      Application.LoadLevel(Application.loadedLevel + 1);
  }
}

