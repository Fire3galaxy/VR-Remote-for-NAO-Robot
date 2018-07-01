using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveSwitcher : MonoBehaviour {
	public GameObject FirstPersonViews, ThirdPersonViews;
	public enum ViewState {FirstPerson, ThirdPerson};
	public ViewState initialState = ViewState.FirstPerson;
	private GameObject fpScreen = null, tpScreen = null;
	private RGBPhotoListener cameraScript;
	
	void Start() {
		// Be sure we have camera script
		cameraScript = GameObject.Find("LogicScripts").GetComponent<RGBPhotoListener>();

		// Be sure we have both camera screens
		if (FirstPersonViews != null) {
			foreach (Transform t in FirstPersonViews.transform) {
				if (t.gameObject.name == "Paper-sized Screen") {
					fpScreen = t.gameObject;
					break;
				}
				Debug.Log(t.gameObject.name);
			}
			Debug.Assert(fpScreen != null);
		} else {
			Debug.Assert(false);
		}

		if (ThirdPersonViews != null) {
			foreach (Transform t in ThirdPersonViews.transform) {
				if (t.name == "CameraDisplay") {
					tpScreen = t.gameObject;
					break;
				}
			}
			Debug.Assert(tpScreen != null);
		} else {
			Debug.Assert(false);
		}

		// Set camera to correct display
		if (initialState == ViewState.FirstPerson) {
			FirstPersonViews.SetActive(true);
			ThirdPersonViews.SetActive(false);
			cameraScript.display = fpScreen;
		} else {
			FirstPersonViews.SetActive(false);
			ThirdPersonViews.SetActive(true);
			cameraScript.display = tpScreen;
		}
	}

	// Update is called once per frame
	void Update () {
		// Alternate between 1st and 3rd person
		if (OVRInput.GetDown(OVRInput.RawButton.B, OVRInput.Controller.RTouch) || Input.GetKeyDown("s")) {
			FirstPersonViews.SetActive(!FirstPersonViews.activeSelf);
			ThirdPersonViews.SetActive(!ThirdPersonViews.activeSelf);

			if (FirstPersonViews.activeSelf)
				cameraScript.display = fpScreen;
			else
				cameraScript.display = tpScreen;
		}
	}
}
