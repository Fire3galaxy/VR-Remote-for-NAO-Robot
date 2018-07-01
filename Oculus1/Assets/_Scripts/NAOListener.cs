using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAOListener : MonoBehaviour {
	public GameObject LeftHand;

	private PythonClient clientObject;
	private string lastMessage = "";
	private float elapsedTime = 0.0f;
	private float waitTime = .5f;

	// Use this for initialization
	void Start () {
		clientObject = GameObject.Find("/LogicScripts").GetComponent<PythonClient>();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= waitTime) {
			elapsedTime = 0.0f;
			// ProcessMessage(clientObject.serverConnection.strMessage);
		}
	}

	void ProcessMessage(string message) {
			// Parse message
			string[] parsedStr = message.Split('|');

			switch (parsedStr[0]) {
				case "LARM":
					// Update left arm position
					string[] parsedPosition = parsedStr[1].Split(',');

					if (parsedPosition.Length == 6) {
						// (NAO) Y Z X -> (UNITY) X Y Z
						Vector3 position = new Vector3(float.Parse(parsedPosition[1]), float.Parse(parsedPosition[2]), float.Parse(parsedPosition[0]));
						Debug.Log(new Vector3(float.Parse(parsedPosition[4]), float.Parse(parsedPosition[5]), float.Parse(parsedPosition[3])));
						Quaternion rotation = Quaternion.Euler(new Vector3(float.Parse(parsedPosition[4]), float.Parse(parsedPosition[5]), float.Parse(parsedPosition[3])));
						if (LeftHand != null) {
							LeftHand.transform.localPosition = position;
							//LeftHand.transform.rotation = rotation;
						}
					}
					break;
				default:
					break;
			}
	}
}
