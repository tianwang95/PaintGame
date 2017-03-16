using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistManager : MonoBehaviour {

	private List<Object> storage;

	public int StoreObject(Object obj) {
		Object clone = Instantiate (obj);
		GameObject gameObj = clone as GameObject;
		if (gameObj != null) {
			gameObj.SetActive (false);
		}
		DontDestroyOnLoad (clone);
		storage.Add (clone);
		return storage.Count - 1;
	}

	public Object GetObject(int index) {
		if (index >= 0 && index < storage.Count) {
			return storage [index];
		} else {
			return null;
		}
	}
}
