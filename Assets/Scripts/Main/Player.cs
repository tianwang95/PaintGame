using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	private GameObject weaponBase;
	[SerializeField]
	private float amountForward;
	[SerializeField]
	private float amountRight;
	private GameObject currWeapon;
	private int weaponIdx = 0;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<Inventory> ();
		currWeapon = Instantiate (weaponBase, transform.position + transform.forward * amountForward + transform.right * amountRight, transform.rotation);
		currWeapon.transform.parent = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1")) {
			//fire the weapon
			if (currWeapon != null && currWeapon.GetComponent(typeof(IWeapon)) != null) {
				IWeapon weapon = (IWeapon) currWeapon.GetComponent(typeof(IWeapon));
				weapon.MainFire ();
			}
		}
		//TODO switch weapons
	}

	void PickUpWeapon(IWeapon weapon) {
		Inventory inv = (Inventory) gameObject.GetComponent(typeof(Inventory));
		int initSize = inv.size ();
		inv.AddWeapon (weapon);
		if (inv.size () > initSize) {
			//it's a new weapon
			currWeapon = weapon.CreateGameObject ();
		}
	}
}
