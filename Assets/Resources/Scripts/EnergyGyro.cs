/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class EnergyGyro : MonoBehaviour {

  public Rotate[] gimbals;
  public float[] startSpeeds;
  public float[] endSpeeds;
  public Renderer[] rings;
  public float damping = 0.01f;
  public float chargeTransition = 0.2f;
  public float shrinkSize = 0.2f;
  public float shrinkTime = 3.0f;
  public float endEmissionGain = 0.7f;
  public float endTiling = 0.0f;

  private float current_size_;
  private float current_gain_;
  private float start_size_;
  private float start_emission_gain_;
  private float charge_level_ = 0.0f;
  private bool charged = false;

	void Update () {
    if (charged) {
      if (current_size_ > shrinkSize) {
        float update_size = Time.deltaTime * (start_size_ - shrinkSize) / shrinkTime;
        current_size_ -= update_size;
        transform.localScale = new Vector3(current_size_, current_size_, current_size_);

        float update_gain = Time.deltaTime * (start_emission_gain_ - endEmissionGain) / shrinkTime;
        current_gain_ -= update_gain;
        rings[0].material.SetFloat("_EmissionGain", current_gain_);
      }
    }
    for (int i = 0; i < gimbals.Length; ++i) {
      float degreesPerSecond = startSpeeds[i] + charge_level_ * (endSpeeds[i] - startSpeeds[i]);
      gimbals[i].degreesPerSecond = degreesPerSecond;
    }

    if (!charged) {
      charge_level_ *= (1 - damping);

      if (charge_level_ >= chargeTransition) {
        charged = true;
        start_size_ = transform.localScale.x;
        start_emission_gain_ = rings[0].material.GetFloat("_EmissionGain");
        Debug.Log(start_size_);
        current_size_ = start_size_;
        current_gain_ = start_emission_gain_;
      }
    }
	}

  public void Zap(float amount) {
    charge_level_ += amount;
    charge_level_ = Mathf.Clamp(charge_level_, 0.0f, 1.0f);
  }

  public Vector3[] GetZappingPoints() {
    Vector3[] points = new Vector3[gimbals.Length * 2];
    for (int i = 0; i < gimbals.Length; ++i) {
      points[2 * i] = gimbals[i].transform.TransformPoint(gimbals[i].rotateVector / 2.0f);
      points[2 * i + 1] = gimbals[i].transform.TransformPoint(-gimbals[i].rotateVector / 2.0f);
    }
    return points;
  }
}
