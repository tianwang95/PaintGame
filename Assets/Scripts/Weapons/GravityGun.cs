using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour, IWeapon {
	// Use this for initialization
	public float thrust;
	public float grabStrength;
	public float grabDistance;
	public float holdDistance;

	private float forceTransitionPoint = 3.0f;
	private float hookesConst;

	private GameObject controlledObject;
	private Vector3 controlPointLocal;
	private bool prevUsingGravity = false;
	private float prevDrag;
	private float prevAngularDrag;

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
			rig.AddForce (Camera.main.transform.forward * thrust);
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
			if ((hit.point - Camera.main.transform.position).magnitude < grabDistance) {
				GameObject hitObject = hit.transform.gameObject;
				Rigidbody hitBody = hitObject.GetComponent<Rigidbody> ();
				//check that we can move this thing
				if (hitBody != null &&
					hitBody.constraints == RigidbodyConstraints.None &&
					hitObject.CompareTag(Props.GroupTag)) {
					prevUsingGravity = hitBody.useGravity;
					prevDrag = hitBody.drag;
					prevAngularDrag = hitBody.angularDrag;
					hitBody.useGravity = false;
					controlledObject = hitObject;
					controlPointLocal = hitObject.transform.InverseTransformPoint (hit.point);
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
		float forceMagnitude;
		if (dist <= 3f) {
			rig.drag = 3f;
			rig.angularDrag = 3f;
			forceMagnitude = dist * hookesConst;
		} else {
			rig.drag = prevDrag;
			rig.angularDrag = prevAngularDrag;
			forceMagnitude = Mathf.Min (grabStrength / (dist * dist), grabStrength);
		}
		rig.AddForceAtPosition (path.normalized * forceMagnitude, controlPointWorld);
	}

	public void ResetControlledObject() {
		Rigidbody rig = controlledObject.GetComponent<Rigidbody> ();
		rig.useGravity = prevUsingGravity;
		rig.drag = prevDrag;
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