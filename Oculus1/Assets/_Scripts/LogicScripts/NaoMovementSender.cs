using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaoMovementSender : MonoBehaviour {
	private PythonClient clientObject;
	private float elapsedTime = 0.0f;
	private bool sendingMovements = false;

	// Use this for initialization
	void Start () {
		clientObject = GameObject.Find("/LogicScripts").GetComponent<PythonClient>();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (clientObject.serverConnection.isConnected && elapsedTime >= clientObject.SendFrequency) {
			Vector2 movements = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
			
			// If you are moving and you stop moving the joystick, send a NONE command to stop movement
			if (sendingMovements) {
				if (Math.Abs(movements.x) < .0001f && Math.Abs(movements.y) < .0001f) {
					sendingMovements = false;
					clientObject.serverConnection.fnPacketTest("WALK|NONE");
				} else {
					clientObject.serverConnection.fnPacketTest("WALK|" + movements.ToString("G2"));
				}
			}
			// Only send packets if you want to move
			else {
				if (Math.Abs(movements.x) > .0001f || Math.Abs(movements.y) > .0001f) {
					sendingMovements = true;
					clientObject.serverConnection.fnPacketTest("WALK|" + movements.ToString("G2"));
				}
			}
			elapsedTime = 0.0f;
		}
	}
}
