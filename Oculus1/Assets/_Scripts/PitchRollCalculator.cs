using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PitchRollCalculator : MonoBehaviour {
	public GameObject rightHand;
    public GameObject leftHand;
	public enum TestState {Setup, Testing};
	public TestState state = TestState.Setup;
	public Vector3 shoulderPos;
	public Vector3 currRotation;

	// Use this for initialization
	void Start () {
		if (rightHand == null)
            rightHand = GameObject.Find("LocalAvatar/hand_right");
        if (leftHand == null)
            leftHand = GameObject.Find("LocalAvatar/hand_left");
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
			case TestState.Setup:
				if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch)) {
					shoulderPos = leftHand.transform.position;
					state = TestState.Testing;
				}
				break;
			case TestState.Testing:
				Quaternion armRotation = Quaternion.FromToRotation(-Vector3.right, Vector3.Normalize(leftHand.transform.position - shoulderPos));
				Debug.Log(armRotation.eulerAngles);
				currRotation = armRotation.eulerAngles;
				break;
		}
	}
}
