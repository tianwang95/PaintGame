using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlatformPlayerHolder : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag (Props.PlayerTag)) {
			FirstPersonController fpc = other.gameObject.GetComponent<FirstPersonController> ();
			fpc.SetPlatform (gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag(Props.PlayerTag)) {
			FirstPersonController fpc = other.gameObject.GetComponent<FirstPersonController> ();
			fpc.SetPlatform (null);
		}
	}
}
