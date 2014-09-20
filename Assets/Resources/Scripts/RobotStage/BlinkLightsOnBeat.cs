/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class BlinkLightsOnBeat : MonoBehaviour {

  public AudioSource beatSource;
  public float beatsPerCycle = 1;
  public Blinker[] lights;

  private int last_beat_ = 0;
  private int last_light_ = 0;
  private bool complete_ = false;

  void OnEnable() {
    NotifyComplete.OnCompleted += Completed;
    NotifyComplete.OnIncompleted += Incompleted;
  }

  void OnDisable() {
    NotifyComplete.OnCompleted -= Completed;
    NotifyComplete.OnIncompleted -= Incompleted;
  }

  void Incompleted() {
    complete_ = false;
  }

  void Completed() {
    complete_ = true;
  }

  void DoTheLights() {
    float progress = (1.0f * beatSource.timeSamples) / beatSource.clip.samples;
    int beat_update = (int)(beatsPerCycle * lights.Length * progress) % lights.Length;

    if (last_beat_ != beat_update) {
      int index = Random.Range(0, lights.Length - 1);
      if (index >= last_light_)
        index++;
      lights[index].Blink();
      last_beat_ = beat_update;
      last_light_ = index;
    }
  }

  void Update() {
    if (complete_)
      DoTheLights();
  }
}
