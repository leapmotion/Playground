/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour {

  public int maxLevels = 4;

  private void LoadLevel(int level) {
    KeepToNextScene[] to_delete = FindObjectsOfType<KeepToNextScene>() as KeepToNextScene[];
    foreach(KeepToNextScene deleted in to_delete)
      Destroy(deleted.gameObject);

    Application.LoadLevel(level);
  }

  public void Update() {
    for (int i = 0; i < maxLevels; ++i) {
      if (Input.GetKeyDown((i + 1).ToString())) {
        LoadLevel(i);
        return;
      }
    }
  }
}
