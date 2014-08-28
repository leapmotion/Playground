/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class FaceClosestHand : MonoBehaviour {

  public HandController handController;
    
  Vector3 target_look_direction_ = Vector3.up;
  // Vector3 current_look_direction_ = Vector3.up;

  void Update() {
    HandModel[] hands = handController.GetAllGraphicHands();

    float closest_distance = Mathf.Infinity;

    foreach (HandModel hand in hands) {
      Vector3 delta = hand.GetPalmPosition() - transform.position;
      float distance = delta.magnitude;
      if (distance < closest_distance) {
        closest_distance = distance;
        target_look_direction_ = delta;
      }
    }

    transform.up = target_look_direction_;
  }
}
