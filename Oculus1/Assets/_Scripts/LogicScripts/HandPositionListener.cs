using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SharpConnect;

// Sends positions of touch controllers to python script
public class HandPositionListener : MonoBehaviour {
    private PythonClient clientObject;
    public GameObject rightHand;
    public GameObject leftHand;
    public Text textTitle, textUI, textStats;

    private enum States { Setup, Playtime };
    private enum SetupStates { AskForArmsDown, AskForArmsUp, Done };
    private States currState = States.Setup;
    private SetupStates currSetupState = SetupStates.AskForArmsDown;
    private float elapsedTime = 0.0f;
    
    // Initial hand position, Shoulder pos, Arm length (Arm length should be the same for both, but will be recorded)
    private Vector3[] leftHandDimens = new Vector3[3];
    private Vector3[] rightHandDimens = new Vector3[3];

    // Use this for initialization
    void Start () {
        clientObject = GameObject.Find("/LogicScripts").GetComponent<PythonClient>();

        // Put OVRCameraRig and LocalAvatar into scene
        if (rightHand == null)
            rightHand = GameObject.Find("LocalAvatar/hand_right");
        if (leftHand == null)
            leftHand = GameObject.Find("LocalAvatar/hand_left");
    }
	
	// Update is called once per frame
	void Update () {
        // Report hand positions to Python script once per second
        if (currState == States.Playtime)
        {
            textTitle.text = "Robot Control Phase";
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= clientObject.SendFrequency)
            {
                string LArmMessage = ((leftHand.transform.position - leftHandDimens[0]) / leftHandDimens[2].y).ToString("G4");
                string RArmMessage = ((rightHand.transform.position - rightHandDimens[0]) / rightHandDimens[2].y).ToString("G4");

                // Send arm positions to server
                if (clientObject.serverConnection.isConnected) {
                    clientObject.serverConnection.fnPacketTest("MOVE|LArm|" + LArmMessage); 
                    clientObject.serverConnection.fnPacketTest("MOVE|RArm|" + RArmMessage); 
                }

                // Reset timer
                elapsedTime = 0.0f;
            }
        }
        else
        {
            switch (currSetupState) {
                case SetupStates.AskForArmsDown:
                    textUI.text = "Place your left and right hands straight down at your sides. Then press either your left or right hand trigger.";
                    if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger | OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch | OVRInput.Controller.LTouch)) {
                        // Initial left and right hand positions
                        leftHandDimens[0] = leftHand.transform.position;
                        rightHandDimens[0] = rightHand.transform.position;
                        textStats.text = "Left hand: " + leftHandDimens[0] + "\nRight Hand: " + rightHandDimens[0];
                        currSetupState = SetupStates.AskForArmsUp;
                    }
                    break;
                case SetupStates.AskForArmsUp:
                    textUI.text = "Raise your left and right hands straight up. Then press either your left or right hand trigger.";
                    if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger | OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch | OVRInput.Controller.LTouch)) {
                        Vector3 leftArmLength = (leftHand.transform.position - leftHandDimens[0]) / 2;
                        // Shoulder pos = halfway between lowered hand and raised hand
                        leftHandDimens[1] = leftHandDimens[0] + leftArmLength;
                        // Arm length - magnitude of vector from lowered hand to shoulder
                        leftHandDimens[2] = new Vector3(0, Vector3.Magnitude(leftArmLength), 0);
                        
                        Vector3 rightArmLength = (rightHand.transform.position - rightHandDimens[0]) / 2;
                        // Shoulder pos = halfway between lowered hand and raised hand
                        rightHandDimens[1] = rightHandDimens[0] + rightArmLength;
                        // Arm length - magnitude of vector from lowered hand to shoulder
                        rightHandDimens[2] = new Vector3(0, Vector3.Magnitude(rightArmLength), 0);

                        textStats.text = "Recorded Arm Lengths: " + leftHandDimens[2].y + " (L), " + rightHandDimens[2].y + " (R)\n"
                            + "Initial Hand Positions: " + leftHandDimens[0] + " (L), " + rightHandDimens[0] + " (R)\n"
                            + "Shoulders: " + leftHandDimens[1] + " (L), " + rightHandDimens[1] + " (R)\n";;
                        currSetupState = SetupStates.Done;
                    }
                    break;
                default: // Done
                    textUI.text = "Calibrated! Arm coordinates are sent to NAO every 5 seconds. Please stand still while " +
                        "controlling NAO.";
                    currState = States.Playtime;
                    break;
            }
        }
    }
}
