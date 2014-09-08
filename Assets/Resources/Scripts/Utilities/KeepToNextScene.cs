using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeepToNextScene : MonoBehaviour {

  void Start () {
    DontDestroyOnLoad(gameObject);
  }
}
