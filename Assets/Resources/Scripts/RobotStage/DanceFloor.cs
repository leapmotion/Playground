/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class DanceFloor : MonoBehaviour {

  public int width = 32;
  public int height = 32;

  public Color[] colors;
  public Color defaultColor;
  public float changesPerCycle = 1.0f;
  public AudioSource beatSource;
  public float colorChance = 0.2f;

  private Texture2D texture_;
  private Color[] pixels_;
  private bool playing_ = true;
  private float last_phase_ = 0.0f;

  void Start() {
    pixels_ = new Color[width * height];

    for (int i = 0; i < width * height; ++i) {
      pixels_[i] = defaultColor;
    }

    texture_ = new Texture2D(width, height);
    texture_.filterMode = FilterMode.Point;
    texture_.wrapMode = TextureWrapMode.Clamp;
    texture_.SetPixels(pixels_);
    texture_.Apply();
    GetComponent<Renderer>().material.mainTexture = texture_;
  }

  void Update() {
    if (!playing_)
      return;

    float progress = (1.0f * beatSource.timeSamples) / beatSource.clip.samples;
    float phase = changesPerCycle * progress;
    phase -= (int)phase;

    if (phase < last_phase_) {
      for (int x = 0; x < width; ++x) {
        for (int y = 0; y < height; ++y) {
          if (Random.Range(0.0f, 1.0f) < colorChance)
            pixels_[y * width + x] = colors[Random.Range(0, colors.Length)];
          else
            pixels_[y * width + x] = defaultColor;
        }
      }
      texture_.SetPixels(pixels_);
      texture_.Apply();
    }
    last_phase_ = phase;
  }
}
