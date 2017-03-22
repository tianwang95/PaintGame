using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		WaypointMovement move = other.attachedRigidbody.gameObject.GetComponent<WaypointMovement> ();
		Debug.Log (other.attachedRigidbody.gameObject);
		if (move != null) {
			Debug.Log ("trying to alert waypoint movement");
			move.CollideWithWaypoint (gameObject);
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, 0.5f);
	}
}