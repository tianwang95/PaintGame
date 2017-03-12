using System.Collections;
using UnityEngine;

public interface IWeapon {
	string GetDisplayName();
	//fire the weapon
	void MainFire();

	//fire a secondary function of the weapon, can be a no-op!
	void SecondaryFire();

	//get how many times this weapon can still be used
	int GetAmmoCount();

	//add ammo
	void AddAmmo(int count);

	//remove ammo
	void RemoveAmmo(int count);

	//Get a game object that corresponds to this weapon
	GameObject CreateGameObject ();

}