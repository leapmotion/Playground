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
