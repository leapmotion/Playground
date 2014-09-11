using UnityEngine;
using System.Collections;

public class BlinkRenderersOnBeat : MonoBehaviour {

  public RobotHead[] requiredDancers;
  public bool playing = false;
  public AudioSource beatSource;
  public float beatsPerCycle = 1;
  public Renderer[] lights;

  private int last_light_ = 0;

  void TurnAllOff() {
    for (int i = 0; i < lights.Length; ++i)
      lights[i].enabled = false;
  }

  bool IsDanceParty() {
    foreach (RobotHead head in requiredDancers) {
      if (head.GetBody() == null)
        return false;
    }
    return true;
  }

  void DoTheLights() {
    float progress = (1.0f * beatSource.timeSamples) / beatSource.clip.samples;
    int light_update = (int)(beatsPerCycle * lights.Length * progress) % lights.Length;
    if (last_light_ != light_update) {
      int random_light = Random.Range(0, lights.Length);
      for (int i = 0; i < lights.Length; ++i)
        lights[i].enabled ^= i == random_light;

      last_light_ = light_update;
    }
  }

  void Update () {
    if (IsDanceParty())
      DoTheLights();
    else
      TurnAllOff();
  }
}
