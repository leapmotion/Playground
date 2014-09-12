using UnityEngine;
using System.Collections;

public class HideTextOnPluck : MonoBehaviour {
  
  void OnEnable() {
    NotifyFlower.OnFlowerBreak += HideText;
  }

  void OnDisable() {
    NotifyFlower.OnFlowerBreak -= HideText;
  }

  void HideText(bool isPedal) {
    if (isPedal) {
      GetComponent<TextFader>().dontShowAnymore = true;
      enabled = false;
    }
  }
}
