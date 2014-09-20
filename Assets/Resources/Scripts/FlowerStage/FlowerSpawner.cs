/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class FlowerSpawner : MonoBehaviour {

  public FlowerGrower[] flowerModels;
  public int maxFlowers = 2;

  private int next_flower_model_index_ = 0;
  private int current_flower_index_ = 0;
  private FlowerGrower[] current_flowers_;
  private bool ending_ = false;

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += KillAll;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= KillAll;
  }

  void Start() {
    FlowerGrower flower = Instantiate(flowerModels[next_flower_model_index_]) as FlowerGrower;

    current_flowers_ = new FlowerGrower[maxFlowers];
    current_flowers_[current_flower_index_] = flower;

    flower.gameObject.SetActive(true);
    next_flower_model_index_ = (next_flower_model_index_ + 1) % flowerModels.Length;
  }

  void Update() {
    if (!ending_ && current_flowers_[current_flower_index_].IsBroken()) {
      int next_flower_index = (current_flower_index_ + 1) % maxFlowers;
      FlowerGrower current_flower = current_flowers_[current_flower_index_];
      FlowerGrower next_flower = current_flowers_[next_flower_index];

      current_flower.RemoveStump();
      if (next_flower != null && !next_flower.IsGrabbed())
        next_flower.Die();

      if (current_flower.IsStumpClear() && (next_flower == null || next_flower.IsDead())) {
        if (next_flower != null)
          Destroy(next_flower.gameObject);
        FlowerGrower flower = Instantiate(flowerModels[next_flower_model_index_]) as FlowerGrower;
        current_flower_index_ = next_flower_index;
        current_flowers_[current_flower_index_] = flower;
        flower.gameObject.SetActive(true);
        next_flower_model_index_ = (next_flower_model_index_ + 1) % flowerModels.Length;
      }
    }
  }

  void KillAll() {
    for (int i = 0; i < current_flowers_.Length; ++i) {
      if (current_flowers_[i] != null)
        current_flowers_[i].Die();
    }
    ending_ = true;
  }
}
