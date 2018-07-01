using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaoHeadLimiter : MonoBehaviour {
	public GameObject YawScreen; // Changes from green to red if turns too far
	public GameObject PitchScreen;
	public Color Green = Color.green;
	public Color Red = Color.red;
	private float MAX_YAW = 119.5f;
	private float MAX_FPITCH = 29.5f;	// forward
	private float MAX_BPITCH = -38.5f;	// backward
	private bool validYaw = true;
	private bool validPitch = true;

	// Use this for initialization
	void Start () {
		if (YawScreen != null)
			YawScreen.GetComponent<MeshRenderer>().material.color = Green;
		if (PitchScreen != null)
			PitchScreen.GetComponent<MeshRenderer>().material.color = Green;
	}
	
	// Update is called once per frame
	void Update () {
		// Yaw Update code
		float yaw = transform.eulerAngles.y;

		if (validYaw) {
			if (yaw > MAX_YAW && yaw < 360f - MAX_YAW) {
				if (YawScreen == null) {
					Debug.Log("Turned too far: " + yaw);
				} else {
					YawScreen.GetComponent<MeshRenderer>().material.color = Red;
				}

				validYaw = false;
			}
		} else {
			if (yaw <= MAX_YAW || yaw >= 360f - MAX_YAW) {
				if (YawScreen == null) {
					Debug.Log("Back in correct place: " + yaw);
				} else {
					YawScreen.GetComponent<MeshRenderer>().material.color = Green;
				}

				validYaw = true;
			}
		}

		// Pitch update code. Essentially the same as yaw.
		float pitch = transform.eulerAngles.x;

		if (validPitch) {
			if (pitch > MAX_FPITCH && pitch < 360f + MAX_BPITCH) {
				if (PitchScreen == null) {
					Debug.Log("Turned too far: " + pitch);
				} else {
					PitchScreen.GetComponent<MeshRenderer>().material.color = Red;
				}

				validPitch = false;
			}
		} else {
			if (pitch <= MAX_FPITCH || pitch >= 360f + MAX_BPITCH) {
				if (PitchScreen == null) {
					Debug.Log("Back in correct place: " + pitch);
				} else {
					PitchScreen.GetComponent<MeshRenderer>().material.color = Green;
				}

				validPitch = true;
			}
		}
	}
}
