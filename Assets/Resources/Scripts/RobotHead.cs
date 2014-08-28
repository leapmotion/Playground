using UnityEngine;
using System.Collections;

public class RobotHead : MonoBehaviour {

  protected RobotBody robot_body_;

  public Transform face;
  public AudioClip bootUpNoise;
  public AudioClip shutDownNoise;

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
    audio.PlayOneShot(bootUpNoise, 10.0f);
  }

  public virtual void ShutDown() {
    audio.PlayOneShot(shutDownNoise, 10.0f);
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
