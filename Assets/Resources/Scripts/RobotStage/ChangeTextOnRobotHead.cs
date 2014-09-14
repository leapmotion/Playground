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
