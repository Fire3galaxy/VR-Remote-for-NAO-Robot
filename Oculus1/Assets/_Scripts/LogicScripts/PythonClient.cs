using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using SharpConnect;
using System.Security.Permissions;

public class PythonClient: MonoBehaviour {
    public Connector serverConnection = new Connector();
    public float SendFrequency = 1.0f;

    void Start() {
        Debug.Log(serverConnection.fnConnectResult("localhost", 10000));
    }
    
    void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.Y, OVRInput.Controller.LTouch) || Input.GetKeyDown("c")) {
            if (serverConnection.isConnected) {
                try { serverConnection.fnDisconnect(); } catch { }
            } else {
                Debug.Log(serverConnection.fnConnectResult("localhost", 10000));
            }
        }
    }

    void OnApplicationQuit() {
        if (serverConnection.isConnected) {
            try { serverConnection.fnDisconnect(); } catch { }
        }
    }
}