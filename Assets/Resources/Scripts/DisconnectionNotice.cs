using UnityEngine;
using System.Collections;

public class DisconnectionNotice : MonoBehaviour {

  public HandController controller;
  public float fadeInTime = 1.0f;
  public float fadeOutTime = 1.0f;
  public AnimationCurve fade;

  private float fadedIn = 0.0f;

  void Start() {
    if (controller.IsConnected())
      fadedIn = 0.0f;
    else
      fadedIn = 1.0f;
    SetAlpha(fadedIn);
  }

  void SetAlpha(float alpha) {
    Color color = renderer.material.color;
    color.a = alpha;
    renderer.material.color = color;
  }

  void Update() {
    if (controller.IsConnected())
      fadedIn -= Time.deltaTime / fadeOutTime;
    else
      fadedIn += Time.deltaTime / fadeInTime;
    fadedIn = Mathf.Clamp(fadedIn, 0.0f, 1.0f);

    SetAlpha(fade.Evaluate(fadedIn));
  }
}
