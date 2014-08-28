/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class TextureRotater : MonoBehaviour {

  public float rotateFrequency = 1.0f;

  private float offset = 0.0f;

  void Update() {
    offset += Time.deltaTime * rotateFrequency;
    renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
  }
}
