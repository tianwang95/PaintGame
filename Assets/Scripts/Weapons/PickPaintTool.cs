using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPaintTool : MonoBehaviour, IWeapon {

	public CompoundMaterial paintMaterial;

	private GameObject fullBucket;
	private GameObject emptyBucket;

	private Renderer paintRend;
	private int ammoCount = 1;
	private Animator anim;

	void Start() {
		fullBucket = transform.FindChild (Props.PaintBucketNames.FullBucket).gameObject;
		emptyBucket = transform.FindChild (Props.PaintBucketNames.EmptyBucket).gameObject;
		paintRend = fullBucket.GetComponent<Renderer> ();
		anim = gameObject.GetComponent<Animator> ();
		SwitchToEmpty ();
	}

	public string GetDisplayName() {
		return "Paint Bucket";
	}

	public void MainFire() {
		GameObject objGroup = GetClicked ();
		if (objGroup != null && paintMaterial != null) {
			SetMaterial (objGroup, paintMaterial);
			paintMaterial = null;
			SwitchToEmpty ();
		}
		Animator canvasAnim = GameObject.FindWithTag (Props.Tags.ReticleCanvas).GetComponent<Animator> ();
		canvasAnim.SetTrigger (Props.CanvasTriggers.ContractReticle);
		anim.SetTrigger (Props.PaintBucketTriggers.TiltDown);
	}

	public void SecondaryFire() {
		GameObject objGroup = GetClicked ();
		if (objGroup != null) {
			SwitchToFull ();
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

	void SwitchToEmpty() {
		fullBucket.SetActive (false);
		emptyBucket.SetActive (true);
	}

	void SwitchToFull() {
		fullBucket.SetActive (true);
		emptyBucket.SetActive (false);
	}
}
