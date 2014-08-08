using UnityEngine;
using System.Collections;

public class RobotHead : MonoBehaviour {

  protected RobotBody robot_body_;

  public Transform face;

  protected void SetFaceAlpha(float alpha) {
    if (face == null)
      return;

    Renderer[] renderers = face.GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers) {
      Color color = renderer.material.GetColor ("_TintColor");;
      color.a = alpha;
      renderer.material.SetColor ("_TintColor", color);
    }
  }

  public virtual void BootUp() {
  }

  public virtual void ShutDown() {
  }

  public void SetBody(RobotBody body) {
    robot_body_ = body;
    /*
    if (body == null)
      GetComponent<AudioSource>().Stop();
    else
      GetComponent<AudioSource>().Play();
      */
  }

  public RobotBody GetBody() {
    return robot_body_;
  }

  public void SetOrientation(Vector3 up, Vector3 forward) {
    if (face != null)
      face.rotation = Quaternion.LookRotation(forward, up);
  }
}
