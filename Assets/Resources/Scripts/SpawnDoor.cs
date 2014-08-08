using UnityEngine;
using System.Collections;

public class SpawnDoor : MonoBehaviour {

  public GameObject spawn;
  public GameObject door;
  public Vector3 startSpawnPoint = Vector3.forward;
  public Vector3 endSpawnPoint = Vector3.zero;
  public float spawnTime = 3.0f;
  public float doorOpenTime = 0.5f;
  public float doorCloseTime = 0.5f;

  public AnimationCurve openCurve;
  public AnimationCurve closeCurve;
  public AnimationCurve spawnCurve;

  private GameObject current_spawn_;
  private float spawn_time_ = 0.0f;

  void Start() {
    SetDoorOpenness(0);
    spawn_time_ = spawnTime;
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

  void StartSpawn() {
    spawn_time_ = 0.0f;
  }

  void ActivateSpawn() {
    Rigidbody[] rigidbodies = current_spawn_.GetComponentsInChildren<Rigidbody>();
    foreach (Rigidbody body in rigidbodies) {
      body.detectCollisions = true;
      body.useGravity = true;
    }
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Space))
      StartSpawn();

    if (spawn_time_ <= doorOpenTime) {
      SetDoorOpenness(openCurve.Evaluate(spawn_time_ / doorOpenTime));
    }
    else if (spawn_time_ < spawnTime - doorCloseTime) {
      if (current_spawn_ == null)
        Spawn();
    }
    else if (spawn_time_ < spawnTime) {
      float t = (spawn_time_ + doorCloseTime - spawnTime) / doorCloseTime;
      SetDoorOpenness(closeCurve.Evaluate(t));
    }
    else {
      if (current_spawn_ != null) {
        ActivateSpawn();
        current_spawn_ = null;
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
