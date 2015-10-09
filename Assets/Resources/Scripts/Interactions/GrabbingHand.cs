/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// Leap Motion hand script that detects pinches and grabs the
// closest rigidbody with a spring force if it's within a given range.
public class GrabbingHand : MonoBehaviour {

  public enum PinchState {
    kPinched,
    kReleased,
    kReleasing
  }

  protected const float RELEASE_MAXIMUM_SPRING = 0.05f;
  protected const float RELEASE_DAMPING = 0.1f;

  public LayerMask grabbableLayers;
  public float grabObjectDistanceRatio = 0.7f;
  public float releaseDistanceRatio = 1.2f;
  public float grabObjectDistance = 2.0f;
  public float releaseBreakDistance = 0.3f;

  public float rotationFiltering = 0.4f;
  public float positionFiltering = 0.4f;
  public float minConfidence = 0.3f;
  public float maxVelocity = 0.3f;

  public Vector3 maxMovement = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
  public Vector3 minMovement = new Vector3(-Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity);

  protected PinchState pinch_state_;
  protected Collider active_object_;

  protected float last_max_angular_velocity_;
  protected Quaternion rotation_from_palm_;

  protected Vector3 current_pinch_;
  protected Quaternion palm_rotation_;

  void Start() {
    pinch_state_ = PinchState.kReleased;
    active_object_ = null;
  }

  void OnDestroy() {
    OnRelease();
  }

  protected void IgnoreCollisions(GameObject obj, bool ignore = true) {
    Collider[] first_colliders = GetComponentsInChildren<Collider>();
    Collider[] second_colliders = obj.GetComponentsInChildren<Collider>();

    for (int i = 0; i < first_colliders.Length; ++i) {
      for (int j = 0; j < second_colliders.Length; ++j)
        Physics.IgnoreCollision(first_colliders[i], second_colliders[j], ignore);
    }
  }

  protected Collider GetClosestGrabbableObject(Vector3 pinch_position) {
    Collider closest = null;
    Vector3 distance = new Vector3(grabObjectDistance, 0.0f, 0.0f);
    Collider[] close_things = Physics.OverlapSphere(pinch_position, grabObjectDistance, grabbableLayers);

    for (int j = 0; j < close_things.Length; ++j) {
      Vector3 new_distance = pinch_position - close_things[j].transform.position;
      if (close_things[j].GetComponent<Rigidbody>() != null && new_distance.magnitude < distance.magnitude &&
          !close_things[j].transform.IsChildOf(transform) &&
          close_things[j].tag != "NotGrabbable") {
        GrabbableObject grabbable = close_things[j].GetComponent<GrabbableObject>();
        if (grabbable == null || !grabbable.IsGrabbed()) {
          closest = close_things[j];
          distance = new_distance;
        }
      }
    }

    return closest;
  }

  protected void Hover(Vector3 pinch_position) {
    Collider hover = GetClosestGrabbableObject(pinch_position);

    if (hover != active_object_ && active_object_ != null) {
      GrabbableObject old_grabbable = active_object_.GetComponent<GrabbableObject>();

      if (old_grabbable != null)
        old_grabbable.OnStopHover();
    }

    if (hover != null) {
      GrabbableObject new_grabbable = hover.GetComponent<GrabbableObject>();

      if (new_grabbable != null)
        new_grabbable.OnStartHover();
    }

    active_object_ = hover;
  }

  protected void StartPinch(Vector3 pinch_position) {
    HandModel hand_model = GetComponent<HandModel>();
    if (active_object_ != null) {
      IgnoreCollisions(active_object_.gameObject, true);

      palm_rotation_ = hand_model.GetPalmRotation();
      rotation_from_palm_ = Quaternion.Inverse(palm_rotation_) * active_object_.transform.rotation;
      current_pinch_ = active_object_.transform.position;

      GrabbableObject grabbable = active_object_.GetComponent<GrabbableObject>();
      if (grabbable == null || grabbable.rotateQuickly) {
        last_max_angular_velocity_ = active_object_.GetComponent<Rigidbody>().maxAngularVelocity;
        active_object_.GetComponent<Rigidbody>().maxAngularVelocity = Mathf.Infinity;
      }

      if (grabbable != null) {
        grabbable.OnGrab();

        if (grabbable.preferredOrientation) {
          Vector3 palm_vector = grabbable.palmOrientation;
          if (hand_model.GetLeapHand().IsLeft)
            palm_vector = Vector3.Scale(palm_vector, new Vector3(-1, 1, 1));

          Quaternion relative_rotation = Quaternion.Inverse(palm_rotation_) *
                                         active_object_.transform.rotation;
          Vector3 axis_in_palm = relative_rotation * grabbable.objectOrientation;
          Quaternion axis_correction = Quaternion.FromToRotation(axis_in_palm, palm_vector);
          if (Vector3.Dot(axis_in_palm, palm_vector) < 0)
            axis_correction = Quaternion.FromToRotation(axis_in_palm, -palm_vector);
            
          rotation_from_palm_ = axis_correction * relative_rotation;
        }
      }
    }
  }

  void OnRelease() {
    if (active_object_ != null) {
      GrabbableObject grabbable = active_object_.GetComponent<GrabbableObject>();
      if (grabbable != null)
        grabbable.OnRelease();

      if (grabbable == null || grabbable.rotateQuickly)
        active_object_.GetComponent<Rigidbody>().maxAngularVelocity = last_max_angular_velocity_;

      IgnoreCollisions(active_object_.gameObject, false);

    }
    active_object_ = null;
  }

  public PinchState GetPinchState(Vector3 pinch_position) {
    HandModel hand_model = GetComponent<HandModel>();
    Hand leap_hand = hand_model.GetLeapHand();

    Vector leap_thumb_tip = leap_hand.Fingers[0].TipPosition;
    float closest_distance = Mathf.Infinity;

    // Check thumb tip distance to joints on all other fingers.
    // If it's close enough, you're pinching.
    for (int i = 1; i < HandModel.NUM_FINGERS; ++i) {
      Finger finger = leap_hand.Fingers[i];

      for (int j = 0; j < FingerModel.NUM_BONES; ++j) {
        Vector leap_joint_position = finger.Bone((Bone.BoneType)j).NextJoint;

        float thumb_tip_distance = leap_joint_position.DistanceTo(leap_thumb_tip);
        closest_distance = Mathf.Min(closest_distance, thumb_tip_distance);
      }
    }

    // Scale trigger distance by thumb proximal bone length.
    float proximal_length = leap_hand.Fingers[0].Bone(Bone.BoneType.TYPE_PROXIMAL).Length;
    float trigger_distance = proximal_length * grabObjectDistanceRatio;
    float release_distance = proximal_length * releaseDistanceRatio;

    if (closest_distance <= trigger_distance)
      return PinchState.kPinched;
    if (closest_distance <= release_distance && pinch_state_ != PinchState.kReleased &&
        !ObjectReleaseBreak(pinch_position))
      return PinchState.kReleasing;
    return PinchState.kReleased;
  }

  Vector3 GetPinchPosition() {
    HandModel hand_model = GetComponent<HandModel>();
    return 0.5f * (hand_model.fingers[0].GetTipPosition() + 
                   hand_model.fingers[1].GetTipPosition());
  }

  void UpdatePalmRotation() {
    HandModel hand_model = GetComponent<HandModel>();
    palm_rotation_ = Quaternion.Slerp(palm_rotation_, hand_model.GetPalmRotation(),
                                      1.0f - rotationFiltering);
  }


  void ContinueHardPinch(Vector3 pinch_position) {
    Vector3 delta_pinch = pinch_position - current_pinch_;
    current_pinch_ = current_pinch_ + (1.0f - positionFiltering) * delta_pinch;
    Quaternion target_rotation = palm_rotation_ * rotation_from_palm_;

    Vector3 clamped_pinch = current_pinch_;
    clamped_pinch.x = Mathf.Clamp(clamped_pinch.x, minMovement.x, maxMovement.x);
    clamped_pinch.y = Mathf.Clamp(clamped_pinch.y, minMovement.y, maxMovement.y);
    clamped_pinch.z = Mathf.Clamp(clamped_pinch.z, minMovement.z, maxMovement.z);
    Vector3 velocity = (clamped_pinch - active_object_.transform.position) / Time.deltaTime;
    active_object_.GetComponent<Rigidbody>().velocity = velocity;

    Quaternion delta_rotation = target_rotation *
                                Quaternion.Inverse(active_object_.transform.rotation);

    float angle = 0.0f;
    Vector3 axis = Vector3.zero;
    delta_rotation.ToAngleAxis(out angle, out axis);

    if (angle >= 180) {
      angle = 360 - angle;
      axis = -axis;
    }
    if (angle != 0)
      active_object_.GetComponent<Rigidbody>().angularVelocity = angle * axis;
  }

  bool ObjectReleaseBreak(Vector3 pinch_position) {
    if (active_object_ == null)
      return true;

    Vector3 delta_position = pinch_position - active_object_.transform.position;
    return delta_position.magnitude > releaseBreakDistance;
  }

  void ContinueSoftPinch(Vector3 pinch_position) {
    Vector3 delta_pinch = pinch_position - current_pinch_;
    current_pinch_ = current_pinch_ + (1.0f - positionFiltering) * delta_pinch;
    Quaternion target_rotation = palm_rotation_ * rotation_from_palm_;

    float gravity_amount = RELEASE_MAXIMUM_SPRING;
    Vector3 delta_position = current_pinch_ - active_object_.transform.position;

    float force = 20 * gravity_amount / (0.04f + delta_position.magnitude * delta_position.magnitude);
    active_object_.GetComponent<Rigidbody>().AddForce(delta_position.normalized * force);
    active_object_.GetComponent<Rigidbody>().velocity *= 0.9f;

    Quaternion delta_rotation = target_rotation *
                                Quaternion.Inverse(active_object_.transform.rotation);

    float angle = 0.0f;
    Vector3 axis = Vector3.zero;
    delta_rotation.ToAngleAxis(out angle, out axis);

    if (angle >= 180) {
      angle = 360 - angle;
      axis = -axis;
    }
    if (angle != 0)
      active_object_.GetComponent<Rigidbody>().angularVelocity = gravity_amount * angle * axis;
  }

  void FixedUpdate() {
    UpdatePalmRotation();
    HandModel hand_model = GetComponent<HandModel>();
    Hand leap_hand = hand_model.GetLeapHand();

    if (leap_hand == null)
      return;

    Vector3 pinch_position = GetPinchPosition();
    PinchState new_pinch_state = GetPinchState(pinch_position);
    if (pinch_state_ == PinchState.kPinched) {
      if (new_pinch_state == PinchState.kReleased) {
        OnRelease();
        Hover(pinch_position);
      }
      else if (active_object_ != null)
        ContinueHardPinch(pinch_position);
    }
    else if (pinch_state_ == PinchState.kReleasing) {
      if (new_pinch_state == PinchState.kReleased) {
        OnRelease();
        Hover(pinch_position);
      }
      else if (active_object_ != null)
        ContinueSoftPinch(pinch_position);
    }
    else {
      if (new_pinch_state == PinchState.kPinched) {
        StartPinch(pinch_position);
      }
      else
        Hover(pinch_position);
    }
    pinch_state_ = new_pinch_state;
  }
}
