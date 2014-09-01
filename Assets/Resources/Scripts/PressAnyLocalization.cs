/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class PressAnyLocalization : MonoBehaviour {

  public string english;
  public string chineseSimplified;
  public string chineseTraditional;
  public string french;
  public string german;
  public string italian;
  public string japanese;
  public string korean;
  public string portuguese;
  public string spanish;

	public void Start() {
    GUIText text = GetComponent<GUIText>();

    switch (Application.systemLanguage) {
      case SystemLanguage.English:
        text.text = english;
        break;
      case SystemLanguage.Chinese:
        text.text = chineseSimplified;
        break;
      case SystemLanguage.French:
        text.text = french;
        break;
      case SystemLanguage.German:
        text.text = german;
        break;
      case SystemLanguage.Italian:
        text.text = italian;
        break;
      case SystemLanguage.Japanese:
        text.text = japanese;
        break;
      case SystemLanguage.Korean:
        text.text = korean;
        break;
      case SystemLanguage.Portuguese:
        text.text = portuguese;
        break;
      case SystemLanguage.Spanish:
        text.text = spanish;
        break;
      default:
        text.text = english;
        break;
    }
  }
}

