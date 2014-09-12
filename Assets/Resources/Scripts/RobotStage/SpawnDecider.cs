using UnityEngine;
using System.Collections;

public class SpawnDecider : MonoBehaviour {

  public float firstSpawnTime = 8.0f;
  public RobotHead[] heads;
  public int maxSpawns = 4;
  public float waitToSpawnNext = 1.0f;

  private float spawn_charge_ = 0.0f;
  private int num_attached_ = 0;

  void OnEnable() {
    RobotHead.OnBootUp += IncrementAttached;
    RobotHead.OnShutDown += DecrementAttached;
  }

  void OnDisable() {
    RobotHead.OnBootUp -= IncrementAttached;
    RobotHead.OnShutDown -= DecrementAttached;
  }

  void DecrementAttached() {
    num_attached_--;
    if (num_attached_ < 0)
      num_attached_ = 0;
  }

  void IncrementAttached() {
    num_attached_++;
  }

  bool ShouldSpawn() {
    SpawnDoor door = GetComponent<SpawnDoor>();
    return num_attached_ >= door.GetNumSpawns() &&
           Time.timeSinceLevelLoad >= firstSpawnTime && 
           !door.IsSpawning() &&
           door.GetNumSpawns() < maxSpawns;
  }

  void Update() {
    if (ShouldSpawn()) {
      spawn_charge_ += Time.deltaTime;

      if (spawn_charge_ >= waitToSpawnNext)
        GetComponent<SpawnDoor>().StartSpawn();
    }
    else
      spawn_charge_ = 0.0f;
  }
}
