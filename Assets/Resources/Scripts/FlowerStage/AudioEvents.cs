/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class AudioEvents : MonoBehaviour {

  public AudioClip[] splashSounds;
  public AudioClip[] breakPickSounds;
  public AudioClip[] musicPickSounds;
  public AudioClip[] rustleSounds;
  public AudioClip[] fishCreationSounds;
  public float minSplashSpeed = 0.2f;
  public float splashVolumePerUnitySpeed = 0.1f;
  public float musicPickLevel = 0.4f;
  public float minSplashWaitTime = 0.5f;
  public float minRustleWaitTime = 0.5f;
  public float minFishCreationWaitTime = 5.0f;
  public float fishCreationLevel = 0.4f;
  public float rustleScale = 0.1f;

  private int last_splash_index_ = 0;
  private int last_music_index_ = 0;
  private int last_break_index_ = 0;
  private float last_splash_time_ = 0;
  private float last_rustle_time_ = 0;
  private float last_fish_creation_time_ = 0;
  
  void OnEnable() {
    NotifyFlower.OnFlowerBreak += PlayBreakSound;
    NotifyFlower.OnFlowerRustle += PlayRustleSound;
    FishMusic.OnMoreFish += PlayFishCreationSound;
    Splasher.OnSplash += PlaySplashSound;
  }

  void OnDisable() {
    NotifyFlower.OnFlowerBreak -= PlayBreakSound;
    NotifyFlower.OnFlowerRustle -= PlayRustleSound;
    FishMusic.OnMoreFish -= PlayFishCreationSound;
    Splasher.OnSplash -= PlaySplashSound;
  }

  AudioClip GetSplashSound() {
    int index = Random.Range(0, splashSounds.Length - 1);
    if (index >= last_splash_index_)
      index++;

    last_splash_index_ = index;
    return splashSounds[index];
  }

  AudioClip GetBreakSound() {
    int index = Random.Range(0, breakPickSounds.Length - 1);
    if (index >= last_break_index_)
      index++;

    last_break_index_ = index;
    return breakPickSounds[index];
  }

  AudioClip GetMusicSound() {
    int index = Random.Range(0, musicPickSounds.Length - 1);
    if (index >= last_music_index_)
      index++;

    last_music_index_ = index;
    return musicPickSounds[index];
  }

  AudioClip GetRustleSound() {
    int index = Random.Range(0, rustleSounds.Length);
    return rustleSounds[index];
  }

  AudioClip GetFishCreationSound() {
    int index = Random.Range(0, fishCreationSounds.Length);
    return fishCreationSounds[index];
  }

  void PlaySplashSound(float speed) {
    float new_time = Time.timeSinceLevelLoad;
    float waited = new_time - last_splash_time_;
    if (waited >= minSplashWaitTime) {
      float volume = splashVolumePerUnitySpeed * (speed - minSplashSpeed);
      GetComponent<AudioSource>().PlayOneShot(GetSplashSound(), volume);
      last_splash_time_ = new_time;
    }
  }

  void PlayBreakSound(bool isPedal) {
    GetComponent<AudioSource>().PlayOneShot(GetBreakSound());
    if (isPedal)
      GetComponent<AudioSource>().PlayOneShot(GetMusicSound(), musicPickLevel);
  }

  void PlayRustleSound(float strength) {
    float new_time = Time.timeSinceLevelLoad;
    float waited = new_time - last_rustle_time_;
    if (waited >= minRustleWaitTime) {
      GetComponent<AudioSource>().PlayOneShot(GetRustleSound(), rustleScale * strength);
      last_rustle_time_ = new_time;
    }
  }

  void PlayFishCreationSound() {
    float new_time = Time.timeSinceLevelLoad;
    float waited = new_time - last_fish_creation_time_;
    if (waited >= minFishCreationWaitTime) {
      GetComponent<AudioSource>().PlayOneShot(GetFishCreationSound(), fishCreationLevel);
      last_fish_creation_time_ = new_time;
    }
  }
}
