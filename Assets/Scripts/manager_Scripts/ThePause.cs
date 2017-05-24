using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThePause : MonoBehaviour {

    GameObject ThePlayer;
    PlayerLavaDeath SpawnManager;
    BrutusMechanimInputs MechAnim;
    PlayerMovement_Ver2 PlayerMove;

    public GameObject PauseCanvas;
	public GameObject OptionsCanvas;
    public bool Paused, CinematicPause, PlayerCantPause;
    bool PauseButtonPressed, TotalFreeze;

	public EventSystem ES;
	private GameObject storeSelected;

	private bool isOptions = false;

	private GameObject resumeButton;
	private GameObject mainMenuButton;
	private GameObject settingsButton;

	public AudioSource background;

    // Use this for initialization
    void Start()
    {
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");  
        MechAnim  = ThePlayer.transform.GetChild(1).GetComponent<BrutusMechanimInputs>();
        SpawnManager = GameObject.Find("SpawnManager").GetComponent<PlayerLavaDeath>();

        PauseCanvas = GameObject.Find("PauseCanvas");
		OptionsCanvas = GameObject.Find ("OptionsCanvas");
        if (OptionsCanvas != null)
        {
            OptionsCanvas.SetActive(false);
        }





        ES = GameObject.Find("EventSystem").GetComponent<EventSystem>();



			
        ES = GameObject.Find("EventSystem").GetComponent<EventSystem>();


		ES.firstSelectedGameObject = GameObject.Find ("ResumeButton");
		storeSelected = ES.firstSelectedGameObject;

		resumeButton = GameObject.Find ("ResumeButton");
		Button butonn = resumeButton.GetComponent<Button> ();
		butonn.onClick.AddListener (resume);

		mainMenuButton = GameObject.Find ("Main Menu Button");
		Button burton = mainMenuButton.GetComponent<Button> ();
		burton.onClick.AddListener (main);

		settingsButton = GameObject.Find ("Settings Button");
		Button buntern = settingsButton.GetComponent<Button> ();
		buntern.onClick.AddListener (options);

		background = GetComponent<AudioSource>();
    }

    public void AssignPlayer(GameObject _p) {
        ThePlayer = _p;
        MechAnim = ThePlayer.transform.GetChild(1).GetComponent<BrutusMechanimInputs>();
        Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnManager.LoseScreenActive || SpawnManager.gameOverScreenActive)
            PlayerCantPause = true;
        else
            PlayerCantPause = false;

        if (!PlayerCantPause)
        {

            IsItPaused();

            if (PauseCanvas.activeSelf)
            {
                if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("unpaused");
                    Paused = false;
                }
            }
				
        if (Paused)
        {
            ThePlayer.GetComponent<PlayerMovement_Ver2>().Paused = true;
            TotalFreeze = true;
            PauseCanvas.SetActive(true);
			background.volume = 0.3f;
        }
        else {
            ThePlayer.GetComponent<PlayerMovement_Ver2>().Paused = false;
            TotalFreeze = false;
            PauseCanvas.SetActive(false);
			background.volume = 1.0f;
        } 

            if (TotalFreeze)
                Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;

            if (ThePlayer.GetComponent<PlayerHealth>().IsDead == true)
            {
                ThePlayer.GetComponent<PlayerMovement_Ver2>().Paused = true;
            }

            if (Input.GetKeyDown("joystick button 12") && isOptions == true)
            {
                OptionsCanvas.SetActive(false);
            }

            if (ES.currentSelectedGameObject != storeSelected && !isOptions)
            {
                if (ES.currentSelectedGameObject == null)
                    ES.SetSelectedGameObject(storeSelected);
                else
                    storeSelected = ES.currentSelectedGameObject;
            }
        }
    }

    void IsItPaused() {
        //Debug.Log(PauseButtonPressed);
        if (!Paused&&(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 4")))
            PauseButtonPressed = true;
        else
            PauseButtonPressed = false;

        if (PauseButtonPressed && Paused)
        {
            Paused = false;
            OptionsCanvas.SetActive(false);
        }
        else if (PauseButtonPressed && !Paused)
        {
            Paused = true;
        }
    }

	public void resume()
	{
		Paused = false;
		OptionsCanvas.SetActive (false);
	}
	public void options() {
		OptionsCanvas.SetActive (true);
		isOptions = true;
	}

	public void unOptions() {
		OptionsCanvas.SetActive (false);

		ES.firstSelectedGameObject = GameObject.Find ("Settings Button");
		storeSelected = ES.firstSelectedGameObject;
	}

	public void main()
	{
		SceneManager.LoadScene ("MainMenu");
	}
}
