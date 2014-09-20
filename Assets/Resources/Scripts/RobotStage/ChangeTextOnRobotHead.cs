/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ChangeTextOnRobotHead : MonoBehaviour {

  public bool shouldEnableOnHead = false;
  
  void OnEnable() {
    RobotHead.OnBootUp += ChangeText;
  }

  void OnDisable() {
    RobotHead.OnBootUp -= ChangeText;
  }

  void ChangeText() {
    GetComponent<TextFader>().dontShowAnymore = !shouldEnableOnHead;
    enabled = false;
  }
}
