using UnityEngine;
using System.Collections;

public class SPIN : MonoBehaviour {

	private float degSpun = 0;
	public float nRotations = 3;
	public float rotSpeed = 20f;

	public float growSpeed = 1.2f;
	public float startSize = 0.1f;
	private float buttonEndSize;

	public GameObject button1;




	// Use this for initialization
	void Start () {
		buttonEndSize = button1.transform.localScale.x;

		button1.transform.localScale = new Vector3(button1.transform.localScale.x * startSize, 
			button1.transform.localScale.y*startSize, 
			button1.transform.localScale.z * startSize);
		button1.GetComponent<Renderer>().enabled = false;

		//titleEndSize = transform.localScale.x;
		transform.localScale = new Vector3(transform.localScale.x * startSize, 
											transform.localScale.y * startSize, 
												transform.localScale.z * startSize);
	}
	
	// Update is called once per frame
	void Update () {
		if (degSpun <= 360 * nRotations) {
			transform.Rotate(0,1 * rotSpeed,0);
			Vector3 unit = new Vector3(1, 1, 1);
			transform.localScale *= growSpeed;
			degSpun += rotSpeed;
		} else if (button1.transform.localScale.x <= buttonEndSize) {
			button1.GetComponent<Renderer>().enabled = true;
			button1.transform.localScale *=  1.2f*growSpeed;
		}
	}
}
