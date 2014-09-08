/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour {

  public void Update() {
    if (Input.GetKeyDown("1"))
      Application.LoadLevel(0);
    else if (Input.GetKeyDown("2"))
      Application.LoadLevel(1);
    else if (Input.GetKeyDown("3"))
      Application.LoadLevel(2);
    else if (Input.GetKeyDown("4"))
      Application.LoadLevel(3);
  }
}

