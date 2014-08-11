using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class LeapRecorder : MonoBehaviour {

  private Controller leap_controller_;

  private List<byte[]> list_of_frames_;
  private int frameIdx = 0;
  private bool is_recording_ = false;
  
	// Use this for initialization
	void Start () {
    leap_controller_ = new Controller();
    list_of_frames_ = new List<byte[]>();
	}
	
	// Update is called once per frame
	void Update () {
    if (is_recording_) {
      Frame frame = new Frame();
      frame = leap_controller_.Frame();
      list_of_frames_.Add(frame.Serialize);
    }
	}
  
  public void Record() {
    is_recording_ = true;
    list_of_frames_.Clear();
  }
  
  public void EndRecord() {
    is_recording_ = false;
  }
  
  public Frame GetFrame(int index) {
    Frame frame = new Frame();
    frame.Deserialize(list_of_frames_[index]);
    return frame;
  }
  
  public int GetNumFrames() {
    return list_of_frames_.Count;
  }
  
  public void Save() {
  }
  
  public void Load() {
  }
}
