using UnityEngine;
using System.Collections;

public class RobotAngryHead : RobotHead {

  private const float Y_PERLIN_OFFSET = 10.0f;
  private const float Z_PERLIN_OFFSET = 20.0f;

  public LineRenderer[] hairs;
  public float hairLength = 1.0f;
  public int hairSegments = 20;
  public float hairChangeFrequency = 1.0f;
  public float hairJaggedness = 5.0f;
  public float curlAmplitude = 0.5f;
  public float hairStart = 0.5f;

  private bool active_ = false;
  private float hair_noise_phase_ = 0.0f;

  void Start() {
    SetFaceAlpha(0.0f);
  }

  public override void BootUp() {
    SetFaceAlpha(1.0f);
    active_ = true;
  }

  public override void ShutDown() {
    SetFaceAlpha(0.0f);
    active_ = false;
  }

  void DisableHair() {
    foreach (LineRenderer hair in hairs) {
      if (hair)
        hair.SetVertexCount(0);
    }
  }

  void Update() {
    if (active_) {
      float phase_inc = Mathf.PI / (hairs.Length - 1);
      float phase = 0.0f;

      foreach (LineRenderer hair in hairs) {
        float x_tip = hairLength * Mathf.Cos(phase);
        float y_tip = hairLength * Mathf.Sin(phase);
        Vector3 tip = new Vector3(x_tip, y_tip, 0);
        phase += phase_inc;

        hair.SetVertexCount(hairSegments);
        for (int i = 0; i < hairSegments; ++i) {
          float hair_phase = i / (hairSegments - 1.0f);
          Vector3 normal_position = (hairStart + (1 - hairStart) * hair_phase) * tip;

          float offset_x = Mathf.PerlinNoise(hair_noise_phase_  + phase * 1000,
                                             hair_phase * hairJaggedness) - 0.5f;
          float offset_y = Mathf.PerlinNoise(Y_PERLIN_OFFSET + hair_noise_phase_ + phase * 1000,
                                             hair_phase * hairJaggedness) - 0.5f;
          float offset_z = Mathf.PerlinNoise(Z_PERLIN_OFFSET + hair_noise_phase_ + phase * 1000,
                                             hair_phase * hairJaggedness) - 0.5f;
          hair.SetPosition(i, normal_position +
                              curlAmplitude * new Vector3(offset_x, offset_y, offset_z));
        }
      }
      hair_noise_phase_ += Time.deltaTime * hairChangeFrequency;
    }
    else
      DisableHair();
  }
}
