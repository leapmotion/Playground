using UnityEngine;
using System.Collections;

public class TimeOfDayFade : MonoBehaviour {

  public bool visibleAtNight = true;
  public Transform sun;
  public float magnification = 1.0f;

  void Update () {
    if (sun == null)
      return;

    float day_amount = Vector3.Dot(-Vector3.up, sun.transform.TransformDirection(Vector3.forward)); 

    Color color = renderer.material.color; 
    if (visibleAtNight && day_amount < 0)
      color.a = magnification * -day_amount;
    else if (!visibleAtNight && day_amount > 0)
      color.a = magnification * day_amount;
    else
      color.a = 0;
    renderer.material.color = color;
  }
}
