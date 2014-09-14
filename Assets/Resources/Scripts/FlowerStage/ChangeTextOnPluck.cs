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
