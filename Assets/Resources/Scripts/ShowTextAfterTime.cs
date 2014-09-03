using UnityEngine;
using System.Collections;

public class ShowTextAfterTime : MonoBehaviour {

  public float time = 10.0f;

  void Update () {
    if (Time.timeSinceLevelLoad >= time) {
      GetComponent<PressAnyKeyToContinue>().Show();
    }
  }
}
