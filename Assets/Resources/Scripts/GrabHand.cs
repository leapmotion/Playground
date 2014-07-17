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
public class GrabHand : MonoBehaviour {

  private const float TRIGGER_DISTANCE_RATIO = 0.8f;

  public float grabDistance = 2.5f;
  public float filtering = 0.5f;
  public float minConfidence = 0.3f;
  // public float minActiveTime = 0.5f;
  public float maxVelocity = 0.3f;

  private bool pinching_;
  private Collider grabbed_;
  private Quaternion rotation_from_palm_;
  private Vector3 start_position_;

  private Vector3 palm_position_;
  private Quaternion palm_rotation_;

  void Start() {
    pinching_ = false;
    grabbed_ = null;
  }

  void OnDestroy() {
    OnRelease();
  }

  private void OnPinch(Vector3 pinch_position) {
    pinching_ = true;

    // Check if we pinched a movable object and grab the closest one that's not part of the hand.
    Collider[] close_things = Physics.OverlapSphere(pinch_position, grabDistance);
    Vector3 distance = new Vector3(grabDistance, 0.0f, 0.0f);

    HandModel hand_model = GetComponent<HandModel>();

    for (int j = 0; j < close_things.Length; ++j) {
      Vector3 new_distance = pinch_position - close_things[j].transform.position;
      if (close_things[j].rigidbody != null && new_distance.magnitude < distance.magnitude &&
          !close_things[j].transform.IsChildOf(transform)) {
        grabbed_ = close_things[j];
        distance = new_distance;
      }
    }

    if (grabbed_ != null) {
      grabbed_.rigidbody.maxAngularVelocity = Mathf.Infinity;
      grabbed_.rigidbody.detectCollisions = false;
      palm_rotation_ = hand_model.GetPalmRotation();
      palm_position_ = hand_model.GetPalmPosition();
      rotation_from_palm_ = Quaternion.Inverse(palm_rotation_) * grabbed_.transform.rotation;
      start_position_ = Quaternion.Inverse(palm_rotation_) *
                        (grabbed_.transform.position - palm_position_);

      Grabbable grabbable = grabbed_.GetComponent<Grabbable>();
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

  void OnRelease() {
    pinching_ = false;
    if (grabbed_ != null) {
      grabbed_.rigidbody.maxAngularVelocity = 7.0f;
      grabbed_.rigidbody.detectCollisions = true;

      Grabbable grabbable = grabbed_.GetComponent<Grabbable>();
      if (grabbable != null)
        grabbable.OnRelease();
    }
    grabbed_ = null;
  }

  void Update() {
    bool trigger_pinch = false;
    HandModel hand_model = GetComponent<HandModel>();
    Hand leap_hand = hand_model.GetLeapHand();

    if (leap_hand == null)
      return;

    // Scale trigger distance by thumb proximal bone length.
    Vector leap_thumb_tip = leap_hand.Fingers[0].TipPosition;
    float proximal_length = leap_hand.Fingers[0].Bone(Bone.BoneType.TYPE_PROXIMAL).Length;
    float trigger_distance = proximal_length * TRIGGER_DISTANCE_RATIO;

    // Check thumb tip distance to joints on all other fingers.
    // If it's close enough, start pinching.
    for (int i = 1; i < HandModel.NUM_FINGERS && !trigger_pinch; ++i) {
      Finger finger = leap_hand.Fingers[i];

      for (int j = 0; j < FingerModel.NUM_BONES && !trigger_pinch; ++j) {
        Vector leap_joint_position = finger.Bone((Bone.BoneType)j).NextJoint;
        if (leap_joint_position.DistanceTo(leap_thumb_tip) < trigger_distance)
          trigger_pinch = true;
      }
    }

    Vector3 pinch_position = 0.5f * (hand_model.fingers[0].GetTipPosition() + 
                                     hand_model.fingers[1].GetTipPosition());

    // Only change state if it's different.
    if (leap_hand.Confidence >= minConfidence) {

      float velocity = leap_hand.PalmVelocity.ToUnityScaled().magnitude;
      if (trigger_pinch && !pinching_ && velocity <= maxVelocity)
        OnPinch(pinch_position);
      else if (!trigger_pinch && pinching_)
        OnRelease();
    }

    // Accelerate what we are grabbing toward the pinch.
    if (grabbed_ != null) {
      Grabbable grabbable = grabbed_.GetComponent<Grabbable>();
      palm_rotation_ = Quaternion.Slerp(palm_rotation_, hand_model.GetPalmRotation(),
                                        1.0f - filtering);
      Vector3 delta_palm_position = hand_model.GetPalmPosition() - palm_position_;
      palm_position_ += (1 - filtering) * delta_palm_position;

      Vector3 target_position = pinch_position;
      Quaternion target_rotation = palm_rotation_ * rotation_from_palm_;

      if (grabbable != null && grabbable.keepDistanceWhenGrabbed)
        target_position = palm_position_ + palm_rotation_ * start_position_;

      Vector3 velocity = (target_position - grabbed_.transform.position) / Time.fixedDeltaTime;
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
