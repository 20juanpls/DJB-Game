using UnityEngine;
using System.Collections;

public class ThePause : MonoBehaviour {

    GameObject ThePlayer;
    BrutusMechanimInputs MechAnim;
    PlayerMovement_Ver2 PlayerMove;

    public GameObject PauseCanvas;
    public bool Paused;

    // Use this for initialization
    void Start()
    {
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");  
        MechAnim  = ThePlayer.transform.GetChild(1).GetComponent<BrutusMechanimInputs>();

        PauseCanvas = GameObject.Find("PauseCanvas");

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
            if (Input.GetKeyDown("joystick button 7"))
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


       
    }

    void IsItPaused() {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 7"))
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
            }
        }
    }
}
