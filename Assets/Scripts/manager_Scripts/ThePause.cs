using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThePause : MonoBehaviour {

    GameObject ThePlayer;
    BrutusMechanimInputs MechAnim;
    PlayerMovement_Ver2 PlayerMove;

    public GameObject PauseCanvas;
	public GameObject OptionsCanvas;
    public bool Paused;

	public EventSystem ES;
	private GameObject storeSelected;

    // Use this for initialization
    void Start()
    {
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");  
        MechAnim  = ThePlayer.transform.GetChild(1).GetComponent<BrutusMechanimInputs>();

        PauseCanvas = GameObject.Find("PauseCanvas");
		OptionsCanvas = GameObject.Find ("OptionsCanvas");
        if (OptionsCanvas != null)
        {
            OptionsCanvas.SetActive(false);
        }
			
		ES.firstSelectedGameObject = GameObject.Find ("Resume Button");
		storeSelected = ES.firstSelectedGameObject;

    }

    public void AssignPlayer(GameObject _p) {
        ThePlayer = _p;
        MechAnim = ThePlayer.transform.GetChild(1).GetComponent<BrutusMechanimInputs>();
        Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsItPaused();
        MechAnim.OnPause(Paused);

        if (PauseCanvas.activeSelf)
        {
            if (Input.GetKeyDown("joystick button 4"))
            {
                Paused = false;
            }
        }

        if (Paused == true)
        {
            ThePlayer.GetComponent<PlayerMovement_Ver2>().Paused = true;
            Time.timeScale = 0.0f;
            PauseCanvas.SetActive(true);
        }
        else {
            ThePlayer.GetComponent<PlayerMovement_Ver2>().Paused = false;
            Time.timeScale = 1.0f;
            PauseCanvas.SetActive(false);
        }

        if (ThePlayer.GetComponent<PlayerHealth>().IsDead == true) {
            ThePlayer.GetComponent<PlayerMovement_Ver2>().Paused = true;
        }

		if (ES.currentSelectedGameObject != storeSelected)
		{
			if (ES.currentSelectedGameObject == null)
				ES.SetSelectedGameObject (storeSelected);
			else
				storeSelected = ES.currentSelectedGameObject;
		}
       
    }

    void IsItPaused() {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 4"))
        {
            if (Paused == false)
            {
                //Debug.Log("Paused");
                Paused = true;
            }
            else
            {
               // Debug.Log("Unpaused");
                Paused = false;
				OptionsCanvas.SetActive (false);
            }
        }
    }

	public void resume()
	{
		Paused = false;
		OptionsCanvas.SetActive (false);
	}
	public void options() {
		OptionsCanvas.SetActive (true);
	}

	public void unOptions() {
		OptionsCanvas.SetActive (false);
	}

	public void main()
	{
		SceneManager.LoadScene ("MainMenu");
	}
}
