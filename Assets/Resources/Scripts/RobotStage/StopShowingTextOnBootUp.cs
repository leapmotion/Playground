using UnityEngine;
using System.Collections;

public class StopShowingTextOnBootUp : MonoBehaviour {

  void OnEnable() {
    RobotHead.OnBootUp += StopShowingText;
  }

  void OnDisable() {
    RobotHead.OnBootUp -= StopShowingText;
  }

  void StopShowingText() {
    GetComponent<TextFader>().dontShowAnymore = true;
  }
}
