using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPaintTool : MonoBehaviour, IWeapon {

	public CompoundMaterial paintMaterial;

	private int ammoCount;

	public string GetDisplayName() {
		return "Paint Bucket";
	}

	public void MainFire() {
		GameObject objGroup = GetClicked ();
		if (objGroup != null) {
			paintMaterial = FindMaterial(objGroup);
		}
	}

	public void SecondaryFire() {
		GameObject objGroup = GetClicked ();
		if (objGroup != null && paintMaterial != null) {
			SetMaterial (objGroup, paintMaterial);
		}
	}

	CompoundMaterial FindMaterial(GameObject objGroup) {
		Debug.Log (paintMaterial);
		return objGroup.GetComponent<CompoundMaterialComponent> ().compoundMaterial;
	}

	void SetMaterial(GameObject objGroup, CompoundMaterial c) { 
		objGroup.GetComponent<CompoundMaterialComponent> ().SetMaterial (c);
	}

	GameObject GetClicked() {
		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;

		if(Physics.Raycast (ray, out hit))
		{	
			GameObject found = hit.transform.gameObject;
			if (found == null) {
				return found; 
			}
			if (found.CompareTag (Props.GroupTag)) {
				return found;
			}
			if (found.transform.parent == null) {
				return null;
			}
			return found.transform.parent.gameObject;
		}
		return null;
	}

	public int GetAmmoCount() {
		return ammoCount;
	}

	public void AddAmmo(int count) {
		ammoCount += count;
	}

	public void RemoveAmmo(int count) {
		ammoCount -= count;
	}

	public GameObject CreateGameObject() {
		return gameObject;
	}
}
