﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag(Props.Tags.Player)) {
			Debug.Log("DEATTHHHHHH");
			other.gameObject.GetComponent<ResetManager> ().ResetAll ();
		}
	}
}
