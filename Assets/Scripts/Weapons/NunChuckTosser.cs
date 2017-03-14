using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NunChuckTosser : MonoBehaviour, IWeapon {

	[SerializeField]
	private GameObject nunChuckProjectile;

	[SerializeField]
	private float thrust;

	[SerializeField]
	private int ammoCount = 0;

	// Use this for initialization
	void Start () {
		if (nunChuckProjectile == null) {
			Debug.LogError ("Projectile or Weapon Object is null!");
		}
	}

	public string GetDisplayName() {
		return "Nun Chuck Chucker";
	}

	public void MainFire() {
		if (ammoCount >= 1) {
			GameObject clone = Instantiate (nunChuckProjectile, transform.position + transform.forward, Random.rotation);
			Rigidbody[] rigid_body = clone.GetComponentsInChildren<Rigidbody> ();
			rigid_body[0].AddForce (transform.forward * thrust);
			Vector3 torque = new Vector3 (Random.Range (-1, 1), Random.Range (-1, 1), Random.Range (-1, 1));
			torque = torque * 100.0F;
			rigid_body [0].AddTorque (torque);
			//fire
		} else {
			ammoCount -= 1;
		}
	}

	public void SecondaryFire() {
		//no op
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