using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStation : MonoBehaviour {
	// Use this for initialization

	void Awake () {
		DontDestroyOnLoad (this);
		if (FindObjectsOfType (GetType ()).Length > 1) {
			DestroyImmediate (gameObject);
		}
	}

	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("is triggered");
			other.gameObject.GetComponent<ResetManager> ().ResetScene ();
		}
	}
}
