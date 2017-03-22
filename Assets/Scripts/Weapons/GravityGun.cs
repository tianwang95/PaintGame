using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour, IWeapon {
	// Use this for initialization
	public float thrust;
	public float grabStrength;
	public float grabDistance;
	public float holdDistance;
	public float maxThrustVelocity = 100.0f;

	private float forceTransitionPoint = 3.0f;
	private float hookesConst;

	private GameObject controlledObject;
	private Vector3 controlPointLocal;
	private bool prevUsingGravity = false;
	private float prevAngularDrag = 0.05f;
	private float dampenDrag;

	void Start () {
		hookesConst = Mathf.Min (grabStrength / (forceTransitionPoint * forceTransitionPoint), grabStrength) / forceTransitionPoint;
	}

	public string GetDisplayName() {
		return "Gravity Gun";
	}

	//fire the weapon
	public void MainFire() {
		if (controlledObject != null) {
			ResetControlledObject ();
			Rigidbody rig = controlledObject.GetComponent<Rigidbody> ();
			//max allowable thrust to contain velocity
			float maxThrust = rig.mass * maxThrustVelocity / Time.fixedDeltaTime;
			rig.AddForce (Camera.main.transform.forward * Mathf.Min(maxThrust, thrust));
			controlledObject = null;
		}
	}

	//fire a secondary function of the weapon, can be a no-op!
	public void SecondaryFire() {
		if (controlledObject != null) {
			ResetControlledObject ();
			controlledObject = null;
		} else {
			Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit hit;
			Physics.Raycast (ray, out hit);
			//check distance to hit object
			if (hit.rigidbody != null && (hit.point - Camera.main.transform.position).magnitude < grabDistance) {
				Debug.Log (hit.transform.gameObject);
				GameObject hitObject = hit.transform.gameObject;
				Rigidbody hitBody = hitObject.GetComponent<Rigidbody> ();
				//check that we can move this thing
				if (hitBody != null &&
					!hitBody.isKinematic &&
					hitObject.CompareTag(Props.GroupTag)) {
					//Store prevs
					prevUsingGravity = hitBody.useGravity;
					prevAngularDrag = hitBody.angularDrag;
					//Change to not use gravity
					hitBody.useGravity = false;
					// Store object and associated tether point
					controlledObject = hitObject;
					controlPointLocal = hitObject.transform.InverseTransformPoint (hit.point);
					// Compute drag to use during spring force
					dampenDrag = 2.0f * Mathf.Sqrt (hitBody.mass * hookesConst);
				}
			}
		}
	}

	void FixedUpdate () {
		if (controlledObject == null) {
			return;
		}
		Vector3 holdPoint = Camera.main.transform.position + Camera.main.transform.forward.normalized * holdDistance;
		Vector3 controlPointWorld = controlledObject.transform.TransformPoint (controlPointLocal);
		Vector3 path = holdPoint - controlPointWorld;
		float dist = path.magnitude;

		Rigidbody rig = controlledObject.GetComponent<Rigidbody> ();
		Vector3 force;
		if (dist <= 3f) {
			rig.angularDrag = 1.0f;
			force = dist * hookesConst * path.normalized - dampenDrag * rig.velocity;
		} else {
			rig.angularDrag = prevAngularDrag;
			force = Mathf.Min (grabStrength / (dist * dist), grabStrength) * path.normalized;
		}
		rig.AddForceAtPosition (force, controlPointWorld);
	}

	public void ResetControlledObject() {
		Rigidbody rig = controlledObject.GetComponent<Rigidbody> ();
		rig.useGravity = prevUsingGravity;
		rig.angularDrag = prevAngularDrag;
	}

	//get how many times this weapon can still be used
	public int GetAmmoCount() {
		return 1;
	}

	//add ammo
	public void AddAmmo(int count) {
		//no op
	}

	//remove ammo
	public void RemoveAmmo(int count) {
		//no op
	}

	//Get a game object that corresponds to this weapon
	public GameObject CreateGameObject () {
		return gameObject;
	}

}