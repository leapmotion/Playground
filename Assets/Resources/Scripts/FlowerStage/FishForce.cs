/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class FishForce : MonoBehaviour {
  
  public int expectedJoints = 2;
  public float morphTime = 1.0f;
  public AnimationCurve morphTransition;

  public Rigidbody fishFront;
  public float swimForce = 1.0f;
  public Vector3 swimCenter = Vector3.zero;
  public float swimCircleRadius = 1.0f;
  public float swimCircleFrequency = 0.3f;
  public float swimOuterCircleRadius = 1.0f;
  public float swimOuterCircleFrequency = 0.3f;
  public float swimStartAngle = 0.0f;

  private float current_morph_ = 0.0f;
  private bool is_fish_ = false;

  // Grabble grabble grabble..
  bool IsGrabbed() {
    GrabbableObject[] grabbables = GetComponentsInChildren<GrabbableObject>();

    foreach (GrabbableObject grabbable in grabbables) {
      if (grabbable.IsGrabbed())
        return true;
    }
    return false;
  }

  bool IsAttached() {
    return expectedJoints == GetComponentsInChildren<Joint>().Length;
  }

  public bool IsFish() {
    return is_fish_;
  }

  bool IsTouchingWater() {
    AerodynamicLeaf[] leaves = GetComponentsInChildren<AerodynamicLeaf>();
    
    foreach (AerodynamicLeaf leaf in leaves) {
      if (leaf.TouchingWater())
        return true;
    }
    return false;
  }

  bool ShouldBeFish() {
    return !IsGrabbed() && IsTouchingWater() && !IsAttached();
  }

  void Update() {
    if (ShouldBeFish())
      current_morph_ += Time.deltaTime / morphTime;
    else
      current_morph_ -= Time.deltaTime / morphTime;

    current_morph_ = Mathf.Clamp(current_morph_, 0.0f, 1.0f);
    GetComponent<PetalMesh>().morph = morphTransition.Evaluate(current_morph_);
    is_fish_ = current_morph_ >= 0.5f;
  }

  void FixedUpdate() {
    float fish_amount = 2.0f * (current_morph_ - 0.5f);
    if (fish_amount > 0.0f && ShouldBeFish()) {
      float swimAngle = swimStartAngle + 180 * swimCircleFrequency * Time.timeSinceLevelLoad;
      float swimOuterAngle = swimStartAngle +
                             180 * swimOuterCircleFrequency * Time.timeSinceLevelLoad;
      Vector3 offset = Quaternion.AngleAxis(swimAngle, Vector3.up) * Vector3.right;
      Vector3 outerOffset = Quaternion.AngleAxis(swimOuterAngle, Vector3.up) * Vector3.right;
      Vector3 swimTarget = swimCenter + swimOuterCircleRadius * outerOffset +
                           swimCircleRadius * offset;
      Vector3 delta = swimTarget - fishFront.transform.position;
      fishFront.AddForce(swimForce * fish_amount * delta.normalized * Time.deltaTime);
    }
  }
}
