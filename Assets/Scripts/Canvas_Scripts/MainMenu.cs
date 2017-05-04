﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

	public EventSystem ES;
	private GameObject storeSelected;

	void Start()
	{
		ES.firstSelectedGameObject = GameObject.Find ("NewGame_Button");
		storeSelected = ES.firstSelectedGameObject;
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