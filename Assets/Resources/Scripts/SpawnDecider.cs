using UnityEngine;
using System.Collections;

public class SpawnDecider : MonoBehaviour {

  public float firstSpawnTime = 8.0f;
  public RobotHead[] heads;
  public int maxSpawns = 4;

  void Update() {
    int num_attached = 0;
    foreach (RobotHead head in heads) {
      if (head.GetBody() != null)
        num_attached++;
    }

    SpawnDoor door = GetComponent<SpawnDoor>();
    if (num_attached >= door.GetNumSpawns() &&
        Time.timeSinceLevelLoad >= firstSpawnTime && 
        !door.IsSpawning() &&
        door.GetNumSpawns() < maxSpawns) {
      door.StartSpawn();
    }
  }
}
