using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NewGame()
	{
		SceneManager.LoadScene ("Stage_Hub");
	}

	public void LoadGame()
	{
		
	}

	public void Quit()
	{
		Application.Quit ();
	}
}
