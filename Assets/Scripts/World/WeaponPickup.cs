using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {
	public GameObject weapon;
	public float degsPerSec = 20f;

	private GameObject weaponHolder;

	public static List<Vector3> WeaponPickupPositions = new List<Vector3>();

	void Awake () {
		DontDestroyOnLoad (this);
		if (WeaponPickupPositions.Contains (gameObject.transform.position)) {
			DestroyImmediate (gameObject);
		} else {
			WeaponPickupPositions.Add (gameObject.transform.position);
		}
	}

	void Start() {
		weaponHolder = transform.FindChild (Props.WeaponPickupNames.WeaponHolder).gameObject;
		Instantiate (weapon, weaponHolder.transform.position, weapon.transform.rotation, weaponHolder.transform);
	}

	void Update() {
		if (weaponHolder != null) {
			weaponHolder.transform.Rotate (new Vector3 (0.0f, degsPerSec * Time.deltaTime, 0.0f));
		}
	}

	void OnTriggerEnter(Collider other) {
		if (weapon != null && other.transform.root.gameObject.CompareTag (Props.Tags.Player)) {
			Player playerController = other.transform.root.gameObject.GetComponent<Player> ();
			playerController.PickUpWeapon (weapon);
			weapon = null;
			Destroy (weaponHolder);
		}
	}
}