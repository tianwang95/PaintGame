using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tools : MonoBehaviour {

	private string GroupTag = "Group";
	private string ObjectTag = "Object";

	private bool eyedropper = true;
	private bool bucket = false;
	public Color paintColor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () { 
		if (Input.GetMouseButtonDown (0)) {
			GameObject objGroup = getClicked ();
			if (objGroup != null) {
				if (eyedropper) {
					paintColor = findColor(objGroup);
				}
				if (bucket) {
					setColors (objGroup, paintColor);
				}
			}
				
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			eyedropper = !eyedropper;
			bucket = !bucket;
		}
	}

	Color findColor(GameObject objGroup) {
		foreach(Transform child in objGroup.transform) { 
			if (child.gameObject.tag.CompareTo (ObjectTag) == 0) {
				return child.transform.GetComponent<Renderer> ().material.color;
			}
		}
		return Color.clear;
	}

	void setColors(GameObject objGroup, Color c) { 
		foreach(Transform child in objGroup.transform) { 
			if (child.gameObject.tag.CompareTo (ObjectTag) == 0) {
				child.transform.GetComponent<Renderer> ().material.color = c;
			}
		}
	}
		
	GameObject getClicked() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(Physics.Raycast (ray, out hit))
		{	
			GameObject found = hit.transform.gameObject;
			if (found == null) {
				return found; 
			}
			if (found.tag.CompareTo (GroupTag) == 0) {
				return found;
			}
			if (found.transform.parent == null) {
				return null;
			}
			return found.transform.parent.gameObject;
		}
	
		return null;
	}

}



