/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class RobotHead : MonoBehaviour {

  protected RobotBody robot_body_;

  public Transform face;
  public AudioClip bootUpNoise;
  public AudioClip shutDownNoise;
  public float bootUpVolume = 1.0f;
  public float shutDownVolume = 1.0f;

  public delegate void NotifyBootUp();
  public static event NotifyBootUp OnBootUp;

  public delegate void NotifyShutDown();
  public static event NotifyShutDown OnShutDown;

  protected void SetFaceAlpha(float alpha) {
    if (face == null)
      return;

    Renderer[] renderers = face.GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers) {
      Color color = renderer.material.GetColor("_TintColor");
      color.a = alpha;
      renderer.material.SetColor("_TintColor", color);
    }
  }

  public virtual void BootUp() {
    GetComponent<AudioSource>().PlayOneShot(bootUpNoise, bootUpVolume);
    OnBootUp();
  }

  public virtual void ShutDown() {
    GetComponent<AudioSource>().PlayOneShot(shutDownNoise, shutDownVolume);
    OnShutDown();
  }

  public void SetBody(RobotBody body) {
    robot_body_ = body;
    if (body == null) {
      GetComponent<ChangeColorOnGrab>().Disconnect();
      // audio.Stop();
    }
    else {
      GetComponent<ChangeColorOnGrab>().Connect();
      // audio.Play();
    }
  }

  public RobotBody GetBody() {
    return robot_body_;
  }

  public void SetOrientation(Vector3 up, Vector3 forward) {
    if (face != null)
      face.rotation = Quaternion.LookRotation(forward, up);
  }
}
