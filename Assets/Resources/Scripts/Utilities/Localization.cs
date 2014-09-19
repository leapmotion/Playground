/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Localization : MonoBehaviour {

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

#if UNITY_STANDALONE_WIN

  [DllImport("KERNEL32.DLL")]
  private static extern System.UInt16 GetUserDefaultUILanguage();

  public string GetLocalizedText() {
    System.UInt16 identifier = GetUserDefaultUILanguage();

    switch (identifier) {
      case 0x409:
        return english;
      case 0x804:
        return chineseSimplified;
      case 0xc04:
      case 0x1404:
      case 0x404:
        return chineseTraditional;
      case 0x40C:
        return french;
      case 0x407:
        return german;
      case 0x410:
        return italian;
      case 0x411:
        return japanese;
      case 0x412:
        return korean;
      case 0x416:
      case 0x816:
        return portuguese;
      case 0xC0A:
        return spanish;
      default:
        return english;
    }
  }

#else

  public string GetLocalizedText() {
    switch (Application.systemLanguage) {
      case SystemLanguage.English:
        return english;
      case SystemLanguage.Chinese:
        if (IsTraditionalChinese())
          return chineseTraditional;
        return chineseSimplified;
      case SystemLanguage.French:
        return french;
      case SystemLanguage.German:
        return german;
      case SystemLanguage.Italian:
        return italian;
      case SystemLanguage.Japanese:
        return japanese;
      case SystemLanguage.Korean:
        return korean;
      case SystemLanguage.Portuguese:
        return portuguese;
      case SystemLanguage.Spanish:
        return spanish;
      default:
        return english;
    }
  }

#endif

#if UNITY_STANDALONE_OSX

  [DllImport("CheckChineseTraditional")]
  private static extern bool IsTraditionalChinese();

#else

  private bool IsTraditionalChinese() {
    return false;
  }

#endif

  public void Start() {
    GetComponent<GUIText>().text = GetLocalizedText();
  }
}

