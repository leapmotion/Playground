using UnityEngine;
using System.Collections;

public class SpawnDecider : MonoBehaviour {

  public RobotHead[] heads;
  public int maxSpawns = 4;

  void Start() {
  }

  void Update() {
    int num_attached = 0;
    foreach (RobotHead head in heads) {
      if (head.GetBody() != null)
        num_attached++;
    }

    SpawnDoor door = GetComponent<SpawnDoor>();
    if (num_attached >= door.GetNumSpawns() &&
        !door.IsSpawning() &&
        door.GetNumSpawns() < maxSpawns) {
      door.StartSpawn();
    }
  }
}
