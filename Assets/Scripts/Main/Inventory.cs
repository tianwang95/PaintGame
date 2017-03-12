using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO: manages a player inventory
 */
public class Inventory : MonoBehaviour {
	private List<IWeapon> weapons;

	public IWeapon AddWeapon(IWeapon weapon) {
		foreach (IWeapon w in weapons) {
			if (w.GetType () == weapons.GetType ()) {
				w.AddAmmo (weapon.GetAmmoCount());
				return w;
			}
		}
		weapons.Add (weapon);
		return weapon;
	}

	public List<IWeapon> GetWeapons() {
		return weapons;
	}

	public int size() {
		return weapons.Count;
	}

	public bool containsWeapon(IWeapon weapon) {
		foreach (IWeapon w in weapons) {
			if (w.GetType () == weapon.GetType ()) {
				return true;
			}
		}
		return false;
	}
}