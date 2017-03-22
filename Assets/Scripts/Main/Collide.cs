using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour {

	private CompoundMaterial material;
	private List <GameObject> currentCollisions = new List<GameObject> ();
	private List <GameObject> toMerge = new List<GameObject> ();
	private List <GameObject> toRemove = new List<GameObject> ();
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
		//reset color
		foreach (FixedJoint joint in jointsToDelete) {
			Destroy (joint);
		}
		jointsToDelete.Clear ();

		if (toMerge.Count > 0) {
			foreach (GameObject col in toMerge) {
				if (col != null) {
					MergeWithPhysics (col);
				}
			}
			toMerge.Clear ();
		}

		foreach (GameObject col in toRemove) {
			currentCollisions.RemoveAll (x => x == col);
		}
		toRemove.Clear ();

		material = FindMaterial(gameObject);

		//Delete empty groups
		if (gameObject.transform.childCount <= 0) {
//			Destroy (gameObject);
			gameObject.SetActive(false);
		}

		//Check all collisions for merge
		foreach(GameObject col in currentCollisions) {
			if (col.CompareTag(Props.GroupTag)) {
				CheckMerge (col);
			}
		}


	}

	CompoundMaterial FindMaterial(GameObject objGroup) {
		return objGroup.GetComponent<CompoundMaterialComponent> ().compoundMaterial;
	}

	void OnCollisionEnter (Collision col) {
		currentCollisions.Add (col.gameObject);
	}

	void OnCollisionExit (Collision col) {
		currentCollisions.RemoveAll (x => x == col.gameObject);
	}

	void CheckMerge (GameObject col) {
		//Check that Collision object is Valid
		if (col == null) {
			return;
		}
		if (col.transform.childCount <= 0 ||
			!col.CompareTag(Props.GroupTag)) {
			return;
		}

		//Check that Collision object is not this / avoid duplicate collisions
		if(gameObject == col ||
			gameObject.GetInstanceID() < col.GetInstanceID()) {
				return;
			}

		//Get Collision object color
		CompoundMaterial colMaterial = FindMaterial(col);
		if(material == colMaterial) {
			//If either is frozen, then we will combine into the other automatically
			Rigidbody selfRb = gameObject.GetComponent<Rigidbody>();
			Rigidbody colRb = col.GetComponent<Rigidbody>();
			CompoundMaterialComponent selfComp = gameObject.GetComponent<CompoundMaterialComponent> ();
			CompoundMaterialComponent colComp = col.GetComponent<CompoundMaterialComponent> ();
			if (selfComp.isFrozen) {
				Merge (col, gameObject);
			} else if (colComp.isFrozen) {
				Merge (gameObject, col);
			} else if (selfRb.isKinematic ||
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
				MergeJoints (col);
				toMerge.Add (col);
			}
			toRemove.Add (col);
		}
	
	}

	void Merge(GameObject currGroup, GameObject nextGroup) {
		List<Transform> children = new List<Transform> ();
		foreach (Transform child in currGroup.transform) {
			children.Add (child);
		}
		foreach (Transform child in children) {
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