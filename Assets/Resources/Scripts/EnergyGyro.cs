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
  public AnimationCurve shrinkCurve;

  private float shrink_percent_ = 0.0f;
  private float start_size_;
  private float start_emission_gain_;
  private float start_tiling_;
  private float charge_level_ = 0.0f;
  private bool charged = false;

	void Update () {
    for (int i = 0; i < gimbals.Length; ++i) {
      float degreesPerSecond = startSpeeds[i] + charge_level_ * (endSpeeds[i] - startSpeeds[i]);
      gimbals[i].degreesPerSecond = degreesPerSecond;
    }

    Renderer[] renderers = GetComponentsInChildren<Renderer>();

    if (charged) {
      if (shrink_percent_ < 1.0f) {
        shrink_percent_ += Time.deltaTime / shrinkTime;
        float shrink_amount = shrinkCurve.Evaluate(shrink_percent_);

        float size = start_size_ + shrink_amount * (shrinkSize - start_size_);
        transform.localScale = size * Vector3.one;

        float gain = start_emission_gain_ +
                     shrink_amount * (endEmissionGain - start_emission_gain_);

        foreach (Renderer render in renderers)
          render.material.SetFloat("_EmissionGain", gain);

        float tiling = start_tiling_ + shrink_amount * (endTiling - start_tiling_);
        float offset =  0.5f - tiling / 2.0f;
        for (int i = 0; i < rings.Length; ++i) {
          rings[i].material.SetTextureScale("_MainTex", new Vector2(0.0f, tiling));
          rings[i].material.SetTextureOffset("_MainTex", new Vector2(0.5f, offset));
        }
      }
    }
    else {
      charge_level_ *= (1 - damping);

      if (charge_level_ >= chargeTransition) {
        charged = true;
        start_size_ = transform.localScale.x;
        start_emission_gain_ = rings[0].material.GetFloat("_EmissionGain");
        start_tiling_ = rings[0].material.GetTextureScale("_MainTex")[1];
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
