/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class ChangeColorOnGrab : Grabbable {

  public Color colorGrabbed;
  public Color colorReleased;

  private bool connected_ = false;

  void Start() {
    OnRelease();
  }

  public override void OnGrab() {
    base.OnGrab();
    renderer.material.color = colorGrabbed;
  }

  public override void OnRelease() {
    base.OnRelease();

    if (!connected_)
      renderer.material.color = colorReleased;
  }

  public void Connect() {
    connected_ = true;
    renderer.material.color = colorGrabbed;
  }

  public void Disconnect() {
    connected_ = false;
    if (!grabbed_)
      renderer.material.color = colorReleased;
  }
}
