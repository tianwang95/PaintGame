using UnityEngine;
using System.Collections;

public class changeScene : MonoBehaviour {
	public void NextLevelButton(int index)
	{
		Application.LoadLevel(index);
	}

	public void NextLevelButton(string levelName)
	{
		Application.LoadLevel(levelName);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

