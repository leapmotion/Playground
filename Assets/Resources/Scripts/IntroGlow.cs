using UnityEngine;
using System.Collections;

public class IntroGlow : MonoBehaviour {

  public float maxIntensity = 1.0f;
  public float minIntensity = 1.0f;
  public int period = 0;

  private int time = 0;

  void Update() {
    if (period > 0) {    
      Color color = this.renderer.material.color;

      float amplitude = (maxIntensity - minIntensity)/2;
      float offset = (maxIntensity+minIntensity)/2;

      color.a = amplitude * Mathf.Sin((float)time / (float)period * Mathf.PI * 2) + offset;

      this.renderer.material.color = color;
      time++;
      if (time > period) {
        time = 0;
      }
    }
  }
}
