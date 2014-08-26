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

  private const float RELEASE_MAXIMUM_SPRING = 0.05f;
  private const float RELEASE_DAMPING = 0.1f;

  public float grabDistanceRatio = 0.7f;
  public float releaseDistanceRatio = 1.2f;
  public float grabDistance = 2.0f;
  public float rotationFiltering = 0.4f;
  public float positionFiltering = 0.4f;
  public float minConfidence = 0.3f;
  public float maxVelocity = 0.3f;

  public Vector3 maxMovement = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
  public Vector3 minMovement = new Vector3(-Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity);

  private float last_max_angular_velocity_;
  private bool pinching_;
  private bool releasing_;
  private float release_distance_;
  private Collider grabbed_;
  private Quaternion rotation_from_palm_;

  private Vector3 current_pinch_;
  private Quaternion palm_rotation_;

  private void IgnoreCollisions(GameObject obj, bool ignore = true) {
    Collider[] first_colliders = GetComponentsInChildren<Collider>();
    Collider[] second_colliders = obj.GetComponentsInChildren<Collider>();

    for (int i = 0; i < first_colliders.Length; ++i) {
      for (int j = 0; j < second_colliders.Length; ++j)
        Physics.IgnoreCollision(first_colliders[i], second_colliders[j], ignore);
    }
  }

  void Start() {
    pinching_ = false;
    releasing_ = false;
    release_distance_ = 0.0f;
    grabbed_ = null;
  }

  void OnDestroy() {
    OnRelease();
  }

  private void OnPinch(Vector3 pinch_position) {
    pinching_ = true;
    releasing_ = false;

    // Check if we pinched a movable object and grab the closest one that's not part of the hand.
    Collider[] close_things = Physics.OverlapSphere(pinch_position, grabDistance);
    Vector3 distance = new Vector3(grabDistance, 0.0f, 0.0f);

    HandModel hand_model = GetComponent<HandModel>();

    for (int j = 0; j < close_things.Length; ++j) {
      Vector3 new_distance = pinch_position - close_things[j].transform.position;
      if (close_things[j].rigidbody != null && new_distance.magnitude < distance.magnitude &&
          !close_things[j].transform.IsChildOf(transform) &&
          close_things[j].tag != "NotGrabbable") {
        grabbed_ = close_things[j];
        distance = new_distance;
      }
    }

    if (grabbed_ != null) {
      IgnoreCollisions(grabbed_.gameObject, true);

      palm_rotation_ = hand_model.GetPalmRotation();
      rotation_from_palm_ = Quaternion.Inverse(palm_rotation_) * grabbed_.transform.rotation;
      current_pinch_ = grabbed_.transform.position;

      GrabbableObject grabbable = grabbed_.GetComponent<GrabbableObject>();
      if (grabbable == null || grabbable.rotateQuickly) {
        last_max_angular_velocity_ = grabbed_.rigidbody.maxAngularVelocity;
        grabbed_.rigidbody.maxAngularVelocity = Mathf.Infinity;
      }
      if (grabbable != null) {
        grabbable.OnGrab();

        if (grabbable.preferredOrientation) {
          Vector3 palm_vector = grabbable.palmOrientation;
          if (hand_model.GetLeapHand().IsLeft)
            palm_vector = Vector3.Scale(palm_vector, new Vector3(-1, 1, 1));

          Quaternion relative_rotation = Quaternion.Inverse(palm_rotation_) *
                                         grabbed_.transform.rotation;
          Vector3 axis_in_palm = relative_rotation * grabbable.objectOrientation;
          Quaternion axis_correction = Quaternion.FromToRotation(axis_in_palm, palm_vector);
          if (Vector3.Dot(axis_in_palm, palm_vector) < 0)
            axis_correction = Quaternion.FromToRotation(axis_in_palm, -palm_vector);
            
          rotation_from_palm_ = axis_correction * relative_rotation;
        }
      }
    }
  }

  void OnReleasing(float release_distance) {
    pinching_ = false;
    releasing_ = true;
    release_distance_ = release_distance;
  }

  void OnRelease() {
    pinching_ = false;
    releasing_ = false;
    if (grabbed_ != null) {
      GrabbableObject grabbable = grabbed_.GetComponent<GrabbableObject>();
      if (grabbable != null)
        grabbable.OnRelease();

      if (grabbable == null || grabbable.rotateQuickly)
        grabbed_.rigidbody.maxAngularVelocity = last_max_angular_velocity_;

      IgnoreCollisions(grabbed_.gameObject, false);

    }
    grabbed_ = null;
  }

  void FixedUpdate() {
    bool trigger_pinch = false;
    bool trigger_release = true;
    HandModel hand_model = GetComponent<HandModel>();
    Hand leap_hand = hand_model.GetLeapHand();

    if (leap_hand == null)
      return;

    // Scale trigger distance by thumb proximal bone length.
    Vector leap_thumb_tip = leap_hand.Fingers[0].TipPosition;
    float proximal_length = leap_hand.Fingers[0].Bone(Bone.BoneType.TYPE_PROXIMAL).Length;
    float trigger_distance = proximal_length * grabDistanceRatio;
    float release_distance = proximal_length * releaseDistanceRatio;
    float closest_distance = Mathf.Infinity;

    // Check thumb tip distance to joints on all other fingers.
    // If it's close enough, start pinching.
    for (int i = 1; i < HandModel.NUM_FINGERS && !trigger_pinch; ++i) {
      Finger finger = leap_hand.Fingers[i];

      for (int j = 0; j < FingerModel.NUM_BONES && !trigger_pinch; ++j) {
        Vector leap_joint_position = finger.Bone((Bone.BoneType)j).NextJoint;
        if (leap_joint_position.DistanceTo(leap_thumb_tip) < trigger_distance) {
          trigger_pinch = true;
          closest_distance = leap_joint_position.DistanceTo(leap_thumb_tip);
        }
        if (leap_joint_position.DistanceTo(leap_thumb_tip) < release_distance) {
          trigger_release = false;
          closest_distance = leap_joint_position.DistanceTo(leap_thumb_tip);
        }
      }
    }

    Vector3 pinch_position = 0.5f * (hand_model.fingers[0].GetTipPosition() + 
                                     hand_model.fingers[1].GetTipPosition());

    // Only change state if it's different.
    if (leap_hand.Confidence >= minConfidence) {

      float velocity = leap_hand.PalmVelocity.ToUnityScaled().magnitude;
      if (trigger_pinch && !pinching_ && velocity <= maxVelocity)
        OnPinch(pinch_position);
      else if (trigger_release && (releasing_ || pinching_))
        OnRelease();
      else if (!trigger_pinch && pinching_)
        OnReleasing((closest_distance - trigger_distance) / (release_distance - trigger_distance));
    }

    // Accelerate what we are grabbing toward the pinch.
    if (grabbed_ != null) {
      palm_rotation_ = Quaternion.Slerp(palm_rotation_, hand_model.GetPalmRotation(),
                                        1.0f - rotationFiltering);

      Vector3 delta_pinch = pinch_position - current_pinch_;
      current_pinch_ = current_pinch_ + (1.0f - positionFiltering) * delta_pinch;
      Quaternion target_rotation = palm_rotation_ * rotation_from_palm_;

      if (releasing_) {
        float gravity_amount = RELEASE_MAXIMUM_SPRING * (1.0f - release_distance_);
        Vector3 delta_position = current_pinch_ - grabbed_.transform.position;
        if (delta_position.magnitude > 0.3f) {
          OnRelease();
          return;
        }
        float force = 20 * gravity_amount / (0.04f + delta_position.magnitude * delta_position.magnitude);
        grabbed_.rigidbody.AddForce(delta_position.normalized * force);
        grabbed_.rigidbody.velocity *= 0.9f;

        Quaternion delta_rotation = target_rotation *
                                    Quaternion.Inverse(grabbed_.transform.rotation);

        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        delta_rotation.ToAngleAxis(out angle, out axis);

        if (angle >= 180) {
          angle = 360 - angle;
          axis = -axis;
        }
        if (angle != 0)
          grabbed_.rigidbody.angularVelocity = gravity_amount * angle * axis;
      }
      else {
        Vector3 clamped_pinch = current_pinch_;
        clamped_pinch.x = Mathf.Clamp(clamped_pinch.x, minMovement.x, maxMovement.x);
        clamped_pinch.y = Mathf.Clamp(clamped_pinch.y, minMovement.y, maxMovement.y);
        clamped_pinch.z = Mathf.Clamp(clamped_pinch.z, minMovement.z, maxMovement.z);
        Vector3 velocity = (clamped_pinch - grabbed_.transform.position) / Time.deltaTime;
        grabbed_.rigidbody.velocity = velocity;

        Quaternion delta_rotation = target_rotation *
                                    Quaternion.Inverse(grabbed_.transform.rotation);

        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        delta_rotation.ToAngleAxis(out angle, out axis);

        if (angle >= 180) {
          angle = 360 - angle;
          axis = -axis;
        }
        if (angle != 0)
          grabbed_.rigidbody.angularVelocity = angle * axis;
      }
    }
  }
}
