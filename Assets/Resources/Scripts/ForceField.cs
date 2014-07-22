/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {

  public float damping = 0.1f;
  public float tension = 0.01f;
  public float collisionImpulse = 1.0f;
  public int width = 32;
  public int height = 32;

  private float[,] velocities_;
  private float[,] positions_;

  private Texture2D texture_;
  private Color[] pixels_;

  void OnCollisionEnter(Collision collision) {
    float strength = collisionImpulse * collision.relativeVelocity.magnitude;

    Vector3 contact_point = Vector3.zero;
    foreach (ContactPoint contact in collision.contacts) {
      contact_point += contact.point / collision.contacts.Length;
    }

    Vector3 local_position = transform.InverseTransformPoint(contact_point);
    int x = (int)(width * (local_position.x + 0.5f) + 1.0f);
    int y = (int)(height * (local_position.y + 0.5f) + 1.0f);

    if (x >= 1 && x <= width && y >= 1 && y <= height)
      positions_[x, y] = strength;
  }

  void Start() {
    velocities_ = new float[width + 2, height + 2];
    positions_ = new float[width + 2, height + 2];

    pixels_ = new Color[width * height];

    for (int i = 0; i < width * height; ++i)
      pixels_[i] = new Color(0, 0, 0, 10.0f);

    texture_ = new Texture2D(width, height);
    texture_.filterMode = FilterMode.Point;
    texture_.SetPixels(pixels_);
    texture_.Apply();
    renderer.material.mainTexture = texture_;
  }

  void FixedUpdate() {
    // Apply forces.
    for (int x = 1; x <= width; ++x) {
      for (int y = 1; y <= height; ++y) {
        float average = 0.25f * (positions_[x + 1, y] + positions_[x - 1, y] +
                                 positions_[x, y + 1] + positions_[x, y - 1]);
        float delta = average - positions_[x, y];
        velocities_[x, y] += tension * delta;
        velocities_[x, y] *= (1.0f - damping);
      }
    }

    // Apply velocities.
    for (int x = 1; x <= width; ++x) {
      for (int y = 1; y <= height; ++y)
        positions_[x, y] += velocities_[x, y];
    }
  }

  void Update() {
    for (int x = 0; x < width; ++x) {
      for (int y = 0; y < height; ++y) {
        pixels_[y * width + x].a = positions_[x + 1, y + 1];
      }
    }
    texture_.SetPixels(pixels_);
    texture_.Apply();

    if (Input.GetKeyDown("a"))
      positions_[20, 20] = 5.0f;
  }
}
