/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ChangeTextOnHandNumber : MonoBehaviour {

  public int numberOfHands = 1;
  public bool enableOnNumberOfHands = true;

  void OnEnable() {
    HandCountNotifier.OnHandChange += HandChange;
  }

  void OnDisable() {
    HandCountNotifier.OnHandChange -= HandChange;
  }

  void HandChange(int total, int current) {
    if (total >= numberOfHands) {
      GetComponent<TextFader>().dontShowAnymore = !enableOnNumberOfHands;
    }
  }
}
