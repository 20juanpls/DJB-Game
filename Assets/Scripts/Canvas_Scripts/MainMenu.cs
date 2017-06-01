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
    private GameObject creditsButton;
    private GameObject creditsCanvas;

    bool CreditsOpen;

	void Start()
	{
		ES.firstSelectedGameObject = GameObject.Find ("NewGame_Button");
		storeSelected = ES.firstSelectedGameObject;

        creditsCanvas = GameObject.Find("MainMenu_Credits");
        creditsCanvas.SetActive(false);

		newGameButton = GameObject.Find ("NewGame_Button");
		Button btn = newGameButton.GetComponent<Button> ();
		btn.onClick.AddListener (NewGame);

		quitButton = GameObject.Find ("Quit_Button");
		Button butonn = quitButton.GetComponent<Button> ();
		butonn.onClick.AddListener (Quit);

        creditsButton = GameObject.Find("Settings_Button");
        Button butnon = creditsButton.GetComponent<Button>();
        butnon.onClick.AddListener(LoadCredits);
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
        creditsCanvas.SetActive(CreditsOpen);
    }

	public void NewGame()
	{
		SceneManager.LoadScene ("Stage_Hub");
	}

	public void LoadCredits()
	{
        if (!CreditsOpen)
            CreditsOpen = true;
        else if (CreditsOpen)
            CreditsOpen = false;
	}

	public void Quit()
	{
		Application.Quit ();
	}
}