﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour {
	private bool hasResolvedMerge = true;

	void Awake() {
		if (FindObjectsOfType (GetType ()).Length > 1) {
			DestroyImmediate (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			ResetScene ();
		}
		if (!hasResolvedMerge && SceneManager.sceneCount > 1 && SceneManager.GetSceneAt(1).isLoaded) {
			Scene newScene = SceneManager.GetSceneAt (1);
			Debug.Log (newScene.isLoaded);
			SceneManager.MergeScenes (SceneManager.GetSceneAt(1), SceneManager.GetActiveScene());
			hasResolvedMerge = true;
		}
	}

	public void ResetScene() {
		DeleteAllExceptPlayer ();
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene().name, LoadSceneMode.Additive);
		hasResolvedMerge = false;
	}

	void DeleteAllExceptPlayer(){
		foreach (GameObject o in SceneManager.GetActiveScene().GetRootGameObjects()) {
			if (!o.CompareTag("Player") && !o.CompareTag(Props.PersistTag)) {
				Destroy (o);
			}
		}
	}
}
