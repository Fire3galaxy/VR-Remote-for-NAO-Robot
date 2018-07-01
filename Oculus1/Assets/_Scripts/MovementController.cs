using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    public float speed;

	// Update is called once per frame
	void Update () {
        float moveVertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        float moveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        transform.Translate(moveHorizontal, 0, moveVertical);
	}
}
