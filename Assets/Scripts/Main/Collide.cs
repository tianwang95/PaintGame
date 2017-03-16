using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour {

	private CompoundMaterial material;
	private List <Collision> currentCollisions = new List<Collision> ();
	private List <Collision> toMerge = new List<Collision> ();
	private List <Collision> toRemove = new List<Collision> ();
	private List <FixedJoint> jointsToDelete = new List<FixedJoint>();

	// Use this for initialization
	void Start () {
		//Set color 
		material = FindMaterial(gameObject);

		//Check that this is a Group Object
		if (!gameObject.CompareTag (Props.GroupTag)) {
			Debug.LogError (name + " does not have a 'Group' tag, and therefore cannot be treated as a Group object");
		}
	}

	// Update is called once per frame
	void Update () {
//		Vector3 vel = gameObject.GetComponent<Rigidbody> ().angularVelocity;
//		gameObject.GetComponentInChildren<Renderer> ().material.color = new Vector4 (vel[0], vel[1], vel[2], 1.0f);
		//reset color
		foreach (FixedJoint joint in jointsToDelete) {
			Destroy (joint);
		}
		jointsToDelete.Clear ();

		if (toMerge.Count > 0) {
			foreach (Collision col in toMerge) {
				if (col.gameObject != null) {
					MergeWithPhysics (col.gameObject);
				}
			}
			toMerge.Clear ();
		}

		foreach (Collision col in toRemove) {
			currentCollisions.Remove (col);
		}
		toRemove.Clear ();

		material = FindMaterial(gameObject);

		//Delete empty groups
		if (gameObject.transform.childCount <= 0) {
			Destroy (gameObject);
		}

		//Check all collisions for merge
		foreach(Collision col in currentCollisions) {
			if (col.transform.gameObject.CompareTag(Props.GroupTag)) {
				CheckMerge (col);
			}
		}
	}

	CompoundMaterial FindMaterial(GameObject objGroup) {
		return objGroup.GetComponent<CompoundMaterialComponent> ().compoundMaterial;
	}

	void OnCollisionEnter (Collision col) {
		currentCollisions.Add (col);
	}

	void OnCollisionExit (Collision col) {
		currentCollisions.Remove (col);
	}

	void CheckMerge (Collision collision) {
		//Check that Collision object is Valid
		if (collision.transform.childCount <= 0 ||
			!collision.gameObject.CompareTag(Props.GroupTag)) {
			return;
		}

		//Check that Collision object is not this / avoid duplicate collisions
		if(gameObject == collision.gameObject ||
			gameObject.GetInstanceID() < collision.gameObject.GetInstanceID()) {
				return;
			}

		//Get Collision object color
		CompoundMaterial colMaterial = FindMaterial(collision.gameObject);
		if(material == colMaterial && collision.gameObject != null) {
			GameObject col = collision.gameObject;
			if (col == null) {
				return;
			}
			//If either is frozen, then we will combine into the other automatically
			Rigidbody selfRb = gameObject.GetComponent<Rigidbody>();
			Rigidbody colRb = col.GetComponent<Rigidbody>();
			if (selfRb.isKinematic ||
				selfRb.constraints == RigidbodyConstraints.FreezeAll ||
			    selfRb.constraints == RigidbodyConstraints.FreezePosition ||
			    selfRb.constraints == RigidbodyConstraints.FreezeRotation) {
				//flip
				Merge (col, gameObject);
			} else if (colRb.isKinematic ||
				colRb.constraints == RigidbodyConstraints.FreezeAll ||
			    colRb.constraints == RigidbodyConstraints.FreezePosition ||
			    colRb.constraints == RigidbodyConstraints.FreezeRotation) {
				Merge (gameObject, col);
			} else {
				MergeJoints (collision.gameObject);
				toMerge.Add (collision);
			}
			toRemove.Add (collision);
		}
	
	}

	void Merge(GameObject currGroup, GameObject nextGroup) {
		foreach (Transform child in currGroup.transform) {
			child.parent = nextGroup.transform;
		}
		//Set material again
		nextGroup.GetComponent<CompoundMaterialComponent> ().ResetMaterial ();
	}

	void MergeJoints(GameObject col) {
		FixedJoint joint = gameObject.AddComponent<FixedJoint>();
		joint.connectedBody = col.GetComponent<Rigidbody> ();
		jointsToDelete.Add (joint);
	}

	void MergeWithPhysics(GameObject col) {
		Rigidbody selfRb = gameObject.GetComponent<Rigidbody> ();
		Rigidbody colRb = col.GetComponent<Rigidbody> ();
		Vector3 avgVelocity = (selfRb.mass * selfRb.velocity + colRb.mass * colRb.velocity) / (selfRb.mass + colRb.mass);
		Vector3 avgAngularVelocity = (selfRb.mass * selfRb.angularVelocity + colRb.mass * colRb.angularVelocity) / (selfRb.mass + colRb.mass);
		colRb.velocity = avgVelocity;
		colRb.angularVelocity = avgAngularVelocity;
		Merge (gameObject, col);
	}
}