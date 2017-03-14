﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collide : MonoBehaviour {

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
			name.CompareTo(collision.gameObject.name) >= 0) {
				return;
			}

		//Get Collision object color
		CompoundMaterial colMaterial = FindMaterial(collision.gameObject);
		if(material == colMaterial) {
			Merge(collision.gameObject);
		}
	
	}

	void Merge(GameObject col) {
		foreach (Transform child in gameObject.transform) {
			child.parent = col.transform;
		}
		//Set new Rigidbody Constraints and Gravity? 
	}
}