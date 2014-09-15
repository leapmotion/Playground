using UnityEngine;
using System.Collections;

public class HandCountNotifier : MonoBehaviour {

  int total_seen_hands_ = 0;
  int current_hands_ = 0;

  public delegate void NotifyHandChange(int total_hands, int current_hands);
  public static event NotifyHandChange OnHandChange;

  void OnEnable() {
    HandNotifier.OnHandCreated += OnHandCreated;
    HandNotifier.OnHandDestroyed += OnHandDestroyed;
  }

  void OnDisable() {
    HandNotifier.OnHandCreated -= OnHandCreated;
    HandNotifier.OnHandDestroyed -= OnHandDestroyed;
  }

  void OnHandCreated() {
    total_seen_hands_++;
    current_hands_++;
    OnHandChange(total_seen_hands_, current_hands_);
  }

  void OnHandDestroyed() {
    current_hands_--;
    OnHandChange(total_seen_hands_, current_hands_);
  }
}
