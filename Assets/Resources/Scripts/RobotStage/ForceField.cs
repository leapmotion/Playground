/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {

  private const int COLOR_CHANNELS = 3;

  public float damping = 0.1f;
  public float tension = 0.01f;
  public float collisionImpulse = 1.0f;
  public int width = 32;
  public int height = 32;
  public float radialGlow = 0.2f;

  private float[,,] velocities_;
  private float[,,] positions_;

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

    for (int channel = 0; channel < COLOR_CHANNELS; ++channel) {
      float color_strength = strength * collision.collider.GetComponent<Renderer>().material.color[channel];
      if (x >= 1 && x <= width && y >= 1 && y <= height) {
        positions_[channel, x, y] += color_strength;
        if (x > 1)
          positions_[channel, x - 1, y] += 0.5f * color_strength;
        if (x < width)
          positions_[channel, x + 1, y] += 0.5f * color_strength;
        if (y > 1)
          positions_[channel, x, y - 1] += 0.5f * color_strength;
        if (y < height)
          positions_[channel, x, y + 1] += 0.5f * color_strength;
      }
    }
  }

  void Start() {
    velocities_ = new float[COLOR_CHANNELS, width + 2, height + 2];
    positions_ = new float[COLOR_CHANNELS, width + 2, height + 2];

    pixels_ = new Color[width * height];

    for (int i = 0; i < width * height; ++i)
      pixels_[i] = new Color(1, 1, 1);

    texture_ = new Texture2D(width, height);
    texture_.filterMode = FilterMode.Point;
    texture_.SetPixels(pixels_);
    texture_.Apply();
    GetComponent<Renderer>().material.mainTexture = texture_;
  }

  void FixedUpdate() {
    // Apply forces.
    for (int c = 0; c < COLOR_CHANNELS; ++c) {
      for (int x = 1; x <= width; ++x) {
        for (int y = 1; y <= height; ++y) {
          float average = 0.25f * (positions_[c, x + 1, y] + positions_[c, x - 1, y] +
                                   positions_[c, x, y + 1] + positions_[c, x, y - 1]);
          float delta = average - positions_[c, x, y];
          velocities_[c, x, y] += tension * delta;
          velocities_[c, x, y] *= (1.0f - damping);
        }
      }
    }

    // Apply velocities.
    for (int c = 0; c < COLOR_CHANNELS; ++c) {
      for (int x = 1; x <= width; ++x) {
        for (int y = 1; y <= height; ++y)
          positions_[c, x, y] += velocities_[c, x, y];
      }
    }
  }

  void Update() {
    for (int x = 0; x < width; ++x) {
      for (int y = 0; y < height; ++y) {
        float x_radius = width / 2.0f;
        float y_radius = height / 2.0f;
        float x_dist = (x - x_radius) / x_radius;
        float y_dist = (y - y_radius) / y_radius;
        float dist_from_center = Mathf.Sqrt(x_dist * x_dist + y_dist * y_dist);
        float glow_amount = radialGlow * (1 - dist_from_center);
        float alpha = 0.0f;
        for (int c = 0; c < COLOR_CHANNELS; ++c) {
          pixels_[y * width + x][c] = positions_[c, x + 1, y + 1] + glow_amount;
          alpha = Mathf.Max(alpha, pixels_[y * width + x][c]);
        }
        pixels_[y * width + x].a = alpha;
      }
    }
    texture_.SetPixels(pixels_);
    texture_.Apply();
  }
}
