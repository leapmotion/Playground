using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class LeapRecorder : MonoBehaviour {

  private List<string> list_of_frames_;

  private bool is_recording_ = false;
  private Controller leap_controller_;

	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKey(KeyCode.R) && !is_recording_) {
      is_recording_ = true;
      Debug.Log("Record");
      //list_of_frames_.Add(leap_controller_.Frame().serialize());
    } else if (Input.GetKey(KeyCode.E) && is_recording_){
      is_recording_ = false;
      Debug.Log("End");
    }
	}
}
