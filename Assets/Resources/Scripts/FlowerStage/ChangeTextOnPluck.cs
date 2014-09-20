/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ChangeTextOnPluck : MonoBehaviour {

  public bool shouldEnableOnPluck = false;
  
  void OnEnable() {
    NotifyFlower.OnFlowerBreak += ChangeText;
  }

  void OnDisable() {
    NotifyFlower.OnFlowerBreak -= ChangeText;
  }

  void ChangeText(bool isPedal) {
    if (isPedal) {
      GetComponent<TextFader>().dontShowAnymore = !shouldEnableOnPluck;
      enabled = false;
    }
  }
}
