/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

  public float increment = 0.01f;
  public float amplitude = 0.01f;

  float x_position_;
  float y_position_;
  float z_position_;

  void OnCollisionEnter(Collision collision) {
    Destroy(gameObject);
  }

  void Start() {
    x_position_ = Random.Range(0, 100000);
    y_position_ = Random.Range(0, 100000);
    z_position_ = Random.Range(0, 100000);
  }

  void Update() {
    Vector3 position = transform.position;
    position.x += amplitude * (Mathf.PerlinNoise(x_position_, 0.0f) - 0.5f);
    position.y += amplitude * (Mathf.PerlinNoise(y_position_, 5.0f) - 0.5f);
    position.z += amplitude * (Mathf.PerlinNoise(z_position_, 10.0f) - 0.5f);
    transform.position = position;

    x_position_ += increment;
    y_position_ += increment;
    z_position_ += increment;
  }
}
