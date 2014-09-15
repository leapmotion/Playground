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
