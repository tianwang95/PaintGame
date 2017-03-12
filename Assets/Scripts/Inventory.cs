using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO: manages a player inventory
 */
public class Inventory : MonoBehaviour {
	private List<IWeapon> weapons;

	void AddWeapon(IWeapon weapon) {
		foreach (IWeapon w in weapons) {
			if (w.GetType () == weapons.GetType ()) {
				return;
			}
		}
	}

	List<IWeapon> GetWeapons() {
		return weapons;
	}
}

