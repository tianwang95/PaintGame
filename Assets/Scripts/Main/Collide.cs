using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour {

	private CompoundMaterial material;
	private List <Collision> currentCollisions = new List <Collision> ();

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
			gameObject.GetInstanceID() >= collision.gameObject.GetInstanceID()) {
				return;
			}

		//Get Collision object color
		CompoundMaterial colMaterial = FindMaterial(collision.gameObject);
		if(material == colMaterial) {
			Merge(collision.gameObject);
		}
	
	}

	void Merge(GameObject col) {
		GameObject currGroup;
		GameObject nextGroup;

		Rigidbody currRb = gameObject.GetComponent<Rigidbody>();
		if (currRb.isKinematic ||
			currRb.constraints == RigidbodyConstraints.FreezeAll ||
		    currRb.constraints == RigidbodyConstraints.FreezePosition ||
		    currRb.constraints == RigidbodyConstraints.FreezeRotation) {
			//flip
			currGroup = col;
			nextGroup = gameObject;
		} else {
			currGroup = gameObject;
			nextGroup = col;
		}

		foreach (Transform child in currGroup.transform) {
			child.parent = nextGroup.transform;
		}
		//Set material again
		nextGroup.GetComponent<CompoundMaterialComponent> ().ResetMaterial ();
	}
}