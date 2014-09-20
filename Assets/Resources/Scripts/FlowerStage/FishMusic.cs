/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class FishMusic : MonoBehaviour {

  public int minFishForMusic = 2;
  public int maxFishForMusic = 7;
  public float volumeDamping = 0.95f;

  private int current_number_of_fish_ = 0;
  private float current_volume_ = 0.0f;

  public delegate void NotifyMoreFish();
  public static event NotifyMoreFish OnMoreFish;

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += Deactivate;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= Deactivate;
  }

  float PercentFish() {
    FishForce[] fishies = FindObjectsOfType<FishForce>();
    if (fishies.Length == 0)
      return 0.0f;

    int num_fish = 0;
    foreach (FishForce fish in fishies) {
      if (fish.IsFish())
        num_fish++;
    }
    if (num_fish > current_number_of_fish_)
      OnMoreFish();

    current_number_of_fish_ = num_fish;
    return Mathf.Clamp((1.0f * (num_fish - minFishForMusic)) / maxFishForMusic, 0.0f, 1.0f);
  }

  void Update() {
    current_volume_ = current_volume_ + (1 - volumeDamping) * (PercentFish() - current_volume_);
    GetComponent<FadeInOutAudio>().maxVolume = current_volume_;
  }

  void Deactivate(){
    enabled = false;
  }
}
