using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour {

	//List of waypoints
	public List<GameObject> wayPoints = new List<GameObject>();
	public float desiredSpeed = 1.0f;
	public float maxForce = 200.0f;

	private int currIndex = 0;
	private GameObject currTarget;

	void Start () {
		if (wayPoints.Count > 0) {
			currTarget = wayPoints [0];
		}
	}

	// Update currTarget
	void Update () {
		if (currTarget == null && wayPoints.Count > 0) {
			currTarget = wayPoints [0];
		}
	}

	public void CollideWithWaypoint(GameObject waypoint) {
		if (currTarget != null && wayPoints.Count > 0 && waypoint == currTarget) {
			currIndex = (currIndex + 1) % wayPoints.Count;
			currTarget = wayPoints [currIndex];
		}
	}

	void FixedUpdate() {
		if (currTarget != null) {
			Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
			Vector3 towardsTarget = (currTarget.transform.position - gameObject.transform.position).normalized;
			if (rb.isKinematic) {
				rb.MovePosition (transform.position + towardsTarget * desiredSpeed * Time.fixedDeltaTime);
			} else {
				Vector3 velocityChange = towardsTarget * desiredSpeed - rb.velocity;
				Vector3 force = rb.mass * velocityChange / Time.fixedDeltaTime;
				if (force.magnitude > maxForce) {
					force = force.normalized * maxForce;
				}
				rb.AddForce (force);
			}
		}
	}
}
