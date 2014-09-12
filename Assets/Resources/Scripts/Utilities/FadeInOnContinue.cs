using UnityEngine;
using System.Collections;

public class FadeInOnContinue : MonoBehaviour {

  public float fadeInTime = 1.0f;
  public AnimationCurve fadeCurve;

  private bool fading_ = false;
  private float fade_amount_ = 0.0f;

  void Start() {
    SetAlpha(0.0f);
  }

  void OnEnable() {
    PressAnyKeyToContinue.OnContinue += FadeIn;
  }

  void OnDisable() {
    PressAnyKeyToContinue.OnContinue -= FadeIn;
  }

  void FadeIn() {
    fading_ = true;
  }

  void SetAlpha(float alpha) {
    Color color = renderer.material.GetColor("_TintColor");
    color.a = alpha;
    renderer.material.SetColor("_TintColor", color);
  }

  void Update() {
    if (fading_) {
      fade_amount_ += Time.deltaTime / fadeInTime;
      SetAlpha(fadeCurve.Evaluate(fade_amount_));

      if (fade_amount_ >= 1.0f)
        fading_ = false;
    }
  }
}
