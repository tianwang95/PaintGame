using System.Collections;
using UnityEngine;

public interface IWeapon {
	void GetDisplayName();
	//fire the weapon
	void MainFire();

	//fire a secondary function of the weapon, can be a no-op!
	void SecondaryFire();

	//get how many times this weapon can still be used
	void GetAmmoAmount();

	//addAmmo
	void AddAmmo(int amount);

	void RemoveAmmo(int amount);
}