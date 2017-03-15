using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="CompoundMaterial", menuName="Compound Material")]
public class CompoundMaterial : ScriptableObject {
	public string materialName;
	public Material material;
	public PhysicMaterial physicMaterial;
	public float density = 1.0f;
	public float drag = 0.0f;
	public float angularDrag = 0.05f;
	public bool useGravity = true;
}
