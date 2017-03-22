using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject[] initWeapons;

	[SerializeField]
	private List<GameObject> weapons = new List<GameObject>();
	[SerializeField]
	private float amountForward;
	[SerializeField]
	private float amountRight;
	[SerializeField]
	private float amountLeft;
	private GameObject currWeapon;
	private int weaponIdx = 0;

	void Awake() {
//		DontDestroyOnLoad (gameObject);
		if (FindObjectsOfType (GetType ()).Length > 1) {
			DestroyImmediate (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		foreach (GameObject weapon in initWeapons) {
			PickUpWeapon (weapon);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			//fire the weapon
			if (currWeapon != null && currWeapon.GetComponent(typeof(IWeapon)) != null) {
				IWeapon weapon = (IWeapon) currWeapon.GetComponent(typeof(IWeapon));
				weapon.MainFire ();
			}
		}
		if (Input.GetButtonDown ("Fire2")) {
			if (currWeapon != null && currWeapon.GetComponent(typeof(IWeapon)) != null) {
				IWeapon weapon = (IWeapon) currWeapon.GetComponent(typeof(IWeapon));
				weapon.SecondaryFire ();
			}
		}
		if (Input.GetButtonDown("SwitchWeapon") && Input.GetAxis("SwitchWeapon") > 0) {
			CycleWeapon (1);
		}
		if (Input.GetButtonDown("SwitchWeapon") && Input.GetAxis ("SwitchWeapon") < 0) {
			CycleWeapon (-1);
		}
	}

	void PickUpWeapon(GameObject weapon) {
		if (weapon.GetComponent<IWeapon> () == null) {
			Debug.LogError ("Tried to pick up GameObject without IWeapon as a weapon");
			return;
		}
		GameObject existingWeapon = FindWeapon (weapon);
		if (existingWeapon != null) {
			existingWeapon.GetComponent<IWeapon> ().AddAmmo (weapon.GetComponent<IWeapon> ().GetAmmoCount ());
		} else {
			GameObject weaponClone = Instantiate (weapon,
				transform.position + transform.forward * amountForward + transform.right * amountRight,
				transform.rotation);
			weaponClone.transform.parent = Camera.main.transform;
			weaponClone.SetActive (false);
			weapons.Add (weaponClone);
			CycleWeapon (1);
		}
	}

	void CycleWeapon(int index) {
		if (weapons.Count > 0) {
			weaponIdx = ((weaponIdx + index) % weapons.Count + weapons.Count) % weapons.Count;
			GameObject nextWeapon = weapons [weaponIdx];
			if (currWeapon != null) {
				currWeapon.SetActive (false);
			}
			nextWeapon.SetActive (true);
			currWeapon = nextWeapon;
		}
	}

	GameObject FindWeapon(GameObject weapon) {
		IWeapon weaponComponent = weapon.GetComponent<IWeapon> ();
		if (weaponComponent == null) {
			Debug.LogError ("Tried to check GameObject without IWeapon as a weapon");
			return null;
		} else {
			foreach (GameObject w in weapons) {
				if (w.GetComponent<IWeapon>().GetType () == weapon.GetComponent<IWeapon>().GetType ()) {
					return w;
				}
			}
			return null;
		}
	}
}