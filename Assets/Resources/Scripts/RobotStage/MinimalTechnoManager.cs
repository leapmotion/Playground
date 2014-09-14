using UnityEngine;
using System.Collections;

public class MinimalTechnoManager : MonoBehaviour {

  public FadeInOutAudio[] firstWave;
  public FadeInOutAudio[] secondWave;
  public FadeInOutAudio[] thirdWave;
  public FadeInOutAudio[] fourthWave;

  private FadeInOutAudio[][] waves = new FadeInOutAudio[4][];
  private int wave = 0;

  void OnEnable() {
    RobotHead.OnBootUp += IncrementWave;
    RobotHead.OnShutDown += DecrementWave;
    PressAnyKeyToContinue.OnContinue += Done;
  }

  void OnDisable() {
    RobotHead.OnBootUp -= IncrementWave;
    RobotHead.OnShutDown -= DecrementWave;
    PressAnyKeyToContinue.OnContinue -= Done;
  }

  void Done() {
    wave = -waves.Length;
  }

  void IncrementWave() {
    wave++;
  }

  void DecrementWave() {
    wave--;
  }

  void Start() {
    waves[0] = firstWave;
    waves[1] = secondWave;
    waves[2] = thirdWave;
    waves[3] = fourthWave;

    for (int i = 0; i < waves.Length; ++i) {
      foreach (FadeInOutAudio audio_source in waves[i])
        audio_source.FadeOut();
    }
  }

  void Update () {
    int i = 0;
    for (; i < wave; ++i) {
      foreach (FadeInOutAudio audio_source in waves[i])
        audio_source.FadeIn();
    }

    for (; i < waves.Length; ++i) {
      foreach (FadeInOutAudio audio_source in waves[i])
        audio_source.FadeOut();
    }
  }
}
