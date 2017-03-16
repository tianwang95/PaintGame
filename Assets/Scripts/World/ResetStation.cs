using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStation : MonoBehaviour {
	// Use this for initialization
	public static List<Vector3> ResetStationPositions = new List<Vector3>();

	void Awake () {
		DontDestroyOnLoad (this);
		if (ResetStationPositions.Contains (gameObject.transform.position)) {
			DestroyImmediate (gameObject);
		} else {
			ResetStationPositions.Add (gameObject.transform.position);
		}
	}

	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<ResetManager> ().ResetScene ();
		}
	}
}
