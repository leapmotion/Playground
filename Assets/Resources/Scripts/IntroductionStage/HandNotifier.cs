/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class HandNotifier : MonoBehaviour {

  public delegate void NotifyHandCreated();
  public static event NotifyHandCreated OnHandCreated;

  public delegate void NotifyHandDestroyed();
  public static event NotifyHandDestroyed OnHandDestroyed;

  void Start() {
    if (OnHandCreated != null)
      OnHandCreated();
  }

  void OnDestroy() {
    if (OnHandDestroyed != null)
      OnHandDestroyed();
  }
}
