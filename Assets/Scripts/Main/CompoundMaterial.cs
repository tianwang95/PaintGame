using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="CompoundMaterial", menuName="Compound Material")]
public class CompoundMaterial : ScriptableObject {
	public string materialName;
	public Material material;
	public PhysicMaterial physicMaterial;
	public float density = 1.0f;
	public bool useGravity = true;
}
