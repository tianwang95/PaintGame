using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
	public void NextLevelButton(int index)
	{
		SceneManager.LoadScene(index);
	}

	public void NextLevelButton(string levelName)
	{
		SceneManager.LoadScene (levelName);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

