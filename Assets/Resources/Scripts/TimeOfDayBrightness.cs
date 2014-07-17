using UnityEngine;
using System.Collections;

public class TimeOfDayBrightness : MonoBehaviour {

  public float magnification = 1.0f;
  public float offset = 0.0f;
  private float max_intensity_ = 0.0f;

  void Start() {
    Light sun = GetComponent<Light>();
    if (sun != null)
      max_intensity_ = sun.intensity;
  }

  void Update() {
    Light sun = GetComponent<Light>();
    if (sun != null) {
      float dot = Vector3.Dot(-Vector3.up, sun.transform.TransformDirection(Vector3.forward)); 
      float day_amount = Mathf.Clamp(offset + magnification * dot, 0.0f, 1.0f);
      sun.intensity = day_amount * max_intensity_;
    }
  }
}
