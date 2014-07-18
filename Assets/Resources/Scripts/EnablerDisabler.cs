/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class EnablerDisabler : MonoBehaviour {
  
  public GameObject[] objects;

	public void Update() {
    for (int i = 0; i < objects.Length && i < 10; ++i) {
      if (Input.GetKeyDown(i.ToString())) {
        objects[i].SetActive(!objects[i].activeInHierarchy);
      }
    }
  }
}

