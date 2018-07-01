using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertSphereMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Mesh sphereMesh = GetComponent<MeshFilter>().mesh;
		Vector3[] meshNormals = sphereMesh.normals;
		int[] meshTriangles = sphereMesh.triangles;

		// Inverting all normals
		for (int i = 0; i < meshNormals.Length; i++) {
			meshNormals[i] = -meshNormals[i];
		}
		sphereMesh.normals = meshNormals;

		// "Flip" triangles
		for (int i = 0; i < meshTriangles.Length; i += 3) {
			int t = meshTriangles[i];
			meshTriangles[i] = meshTriangles[i + 2];
			meshTriangles[i + 2] = t;
		}
		sphereMesh.triangles = meshTriangles;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
