/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class SpawnDoor : MonoBehaviour {

  enum DoorState {
    kClosed,
    kOpening,
    kOpen,
    kClosing
  };

  public GameObject spawn;
  public GameObject door;
  public Vector3 startSpawnPoint = Vector3.forward;
  public Vector3 endSpawnPoint = Vector3.zero;
  public float spawnTime = 3.0f;
  public float doorOpenTime = 0.5f;
  public float doorCloseTime = 0.5f;

  public AudioClip openingSound;
  public AudioClip closingSound;

  public AnimationCurve openCurve;
  public AnimationCurve closeCurve;
  public AnimationCurve spawnCurve;

  private GameObject current_spawn_;
  private float spawn_time_ = 0.0f;
  private int num_spawns_ = 0;
  private DoorState door_state_ = DoorState.kClosed;

  void Start() {
    SetDoorOpenness(0);
    spawn_time_ = spawnTime;
  }

  public int GetNumSpawns() {
    return num_spawns_;
  }

  public bool IsSpawning() {
    return spawn_time_ <= spawnTime || current_spawn_ != null;
  }

  void SetDoorOpenness(float openness) {
    Vector3 scale = door.transform.localScale;
    scale.x = openness;
    door.transform.localScale = scale;
  }

  void Spawn() {
    Vector3 spawn_point = transform.TransformPoint(startSpawnPoint);
    current_spawn_ = Instantiate(spawn, spawn_point, Quaternion.identity) as GameObject;
    current_spawn_.SetActive(true);

    Rigidbody[] rigidbodies = current_spawn_.GetComponentsInChildren<Rigidbody>();
    foreach (Rigidbody body in rigidbodies) {
      body.detectCollisions = false;
      body.useGravity = false;
    }
  }

  public void StartSpawn() {
    spawn_time_ = 0.0f;
  }

  void ActivateSpawn() {
    num_spawns_++;
    Rigidbody[] rigidbodies = current_spawn_.GetComponentsInChildren<Rigidbody>();
    foreach (Rigidbody body in rigidbodies) {
      body.detectCollisions = true;
      body.useGravity = true;
    }
    current_spawn_ = null;
  }

  void Update() {
    if (spawn_time_ <= doorOpenTime) {
      if (door_state_ == DoorState.kClosed)
        GetComponent<AudioSource>().PlayOneShot(openingSound);
      door_state_ = DoorState.kOpening;
      SetDoorOpenness(openCurve.Evaluate(spawn_time_ / doorOpenTime));
    }
    else if (spawn_time_ < spawnTime - doorCloseTime) {
      if (current_spawn_ == null)
        Spawn();
      door_state_ = DoorState.kOpen;
    }
    else if (spawn_time_ < spawnTime) {
      if (door_state_ == DoorState.kOpen)
        GetComponent<AudioSource>().PlayOneShot(closingSound);
      door_state_ = DoorState.kClosing;

      float t = (spawn_time_ + doorCloseTime - spawnTime) / doorCloseTime;
      SetDoorOpenness(closeCurve.Evaluate(t));
    }
    else {
      door_state_ = DoorState.kClosed;
      if (current_spawn_ != null) {
        ActivateSpawn();
      }
      SetDoorOpenness(0);
    }
    
    spawn_time_ += Time.deltaTime;
    if (current_spawn_ != null) {
      float spawn_progress = (spawn_time_ - doorOpenTime) / (spawnTime - doorOpenTime);
      float spawn_movement = spawnCurve.Evaluate(spawn_progress);
      Vector3 local_spawn = startSpawnPoint + spawn_movement * (endSpawnPoint - startSpawnPoint);
      current_spawn_.transform.position = transform.TransformPoint(local_spawn);
    }
  }
}
