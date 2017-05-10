using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

	public EventSystem ES;
	private GameObject storeSelected;

	private GameObject newGameButton;
	private GameObject quitButton;

	void Start()
	{
		ES.firstSelectedGameObject = GameObject.Find ("NewGame_Button");
		storeSelected = ES.firstSelectedGameObject;

		newGameButton = GameObject.Find ("NewGame_Button");
		Button btn = newGameButton.GetComponent<Button> ();
		btn.onClick.AddListener (NewGame);

		quitButton = GameObject.Find ("Quit_Button");
		Button butonn = quitButton.GetComponent<Button> ();
		butonn.onClick.AddListener (Quit);

	}

	void Update()
	{
		if (ES.currentSelectedGameObject != storeSelected)
		{
			if (ES.currentSelectedGameObject == null)
				ES.SetSelectedGameObject (storeSelected);
			else
				storeSelected = ES.currentSelectedGameObject;
		}
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