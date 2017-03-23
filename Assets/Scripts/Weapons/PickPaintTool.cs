using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPaintTool : MonoBehaviour, IWeapon {

	public CompoundMaterial paintMaterial;

	private Renderer paintRend;
	private int ammoCount = 1;
	private Animator anim;

	void Start() {
		paintRend = gameObject.GetComponentsInChildren<Renderer> () [0];
		anim = gameObject.GetComponent<Animator> ();

	}

	public string GetDisplayName() {
		return "Paint Bucket";
	}

	public void MainFire() {
		GameObject objGroup = GetClicked ();
		if (objGroup != null && paintMaterial != null) {
			SetMaterial (objGroup, paintMaterial);
		}
		Animator canvasAnim = GameObject.FindWithTag (Props.Tags.ReticleCanvas).GetComponent<Animator> ();
		canvasAnim.SetTrigger (Props.CanvasTriggers.ContractReticle);
		anim.SetTrigger (Props.PaintBucketTriggers.TiltDown);
	}

	public void SecondaryFire() {
		GameObject objGroup = GetClicked ();
		if (objGroup != null) {
			paintMaterial = FindMaterial(objGroup);
			paintRend.material = paintMaterial.material;
		}
		Animator canvasAnim = GameObject.FindWithTag (Props.Tags.ReticleCanvas).GetComponent<Animator> ();
		canvasAnim.SetTrigger (Props.CanvasTriggers.ExpandReticle);
		anim.SetTrigger (Props.PaintBucketTriggers.TiltUp);
	}

	CompoundMaterial FindMaterial(GameObject objGroup) {
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
			if (found.CompareTag (Props.Tags.Group)) {
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
