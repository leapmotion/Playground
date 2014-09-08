using UnityEngine;
using System.Collections;

public class SpawnDecider : MonoBehaviour {

  public float firstSpawnTime = 8.0f;
  public RobotHead[] heads;
  public int maxSpawns = 4;
  public float waitToSpawnNext = 1.0f;

  private float spawn_charge_ = 0.0f;

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
      spawn_charge_ += Time.deltaTime;

      if (spawn_charge_ >= waitToSpawnNext)
        door.StartSpawn();
    }
    else
      spawn_charge_ = 0.0f;
  }
}
