﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundMaterialComponent : MonoBehaviour {

	public CompoundMaterial compoundMaterial;

	public CompoundMaterial prevMaterial;

	void Start() {
		if (compoundMaterial != null) {
			SetMaterial (compoundMaterial);
		}
	}

	public void SetMaterial(CompoundMaterial newMaterial) {
		prevMaterial = compoundMaterial;
		compoundMaterial = newMaterial;
		ApplyMaterialProperties (newMaterial);
	}

	private void ApplyMaterialProperties(CompoundMaterial mat) {
		if (!gameObject.CompareTag (Props.GroupTag)) {
			Debug.LogError ("Cannot set material for something that is not a \"Group\"");
		}
		Rigidbody rig = gameObject.GetComponent<Rigidbody> ();
		rig.SetDensity (mat.density);
		rig.useGravity = mat.useGravity;
		foreach(Transform child in gameObject.transform) { 
			if (child.gameObject.CompareTag (Props.ObjectTag)) {
				Collider coll = child.gameObject.GetComponent<Collider> ();
				coll.material = mat.physicMaterial;
				Renderer rend = child.gameObject.GetComponent<Renderer> ();
				rend.material = mat.material;
			}
		}
	}
}