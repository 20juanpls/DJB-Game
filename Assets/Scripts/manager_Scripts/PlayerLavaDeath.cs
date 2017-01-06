using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerLavaDeath : MonoBehaviour {

	public GameObject loseScreen;
	public GameObject gameOverScreen;
	public GameObject playerFolder;
    public GameObject player;
	Transform respawn;

	private int Deaths;

	// Use this for initialization
	void Start () {
		loseScreen = GameObject.Find ("LoseScreenCanvas");
		gameOverScreen = GameObject.Find ("GameOverCanvas");
        assignButton();
        loseScreen.SetActive (false);
		gameOverScreen.SetActive (false);
        player = GameObject.FindGameObjectWithTag("PlayerMesh");
		playerFolder = GameObject.FindGameObjectWithTag("PlayerFolder");
    }

    void Update() {
		Debug.Log ("Deaths:"+Deaths);
		if (player.GetComponent<PlayerHealth> ().Lives - 1 == Deaths && player.GetComponent<PlayerHealth> ().IsDead == true) {
			gameOverScreen.gameObject.SetActive (true);
			player.GetComponent<PlayerMovement_Ver2> ().DontMove = true;
        } else {	
			if (player.GetComponent<PlayerHealth> ().IsDead == true) {
				loseScreen.gameObject.SetActive (true);
			} else {
				loseScreen.gameObject.SetActive (false);
			}
		}
    }

    void AssignPlayer(GameObject p, GameObject pF)
    {
        playerFolder = pF.gameObject;
        player = p.gameObject;
        Debug.Log(player.ToString());
    }

	void assignButton(){
		Button b = loseScreen.transform.GetChild (0).transform.GetChild (1).GetComponent<Button> ();
        Button reStartHard = gameOverScreen.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();//GameObject.Find("RestartFromBeginning").GetComponent<Button>();
        Button BackToMenu = gameOverScreen.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();//GameObject.Find("BackToMenu").GetComponent<Button>();

        //Debug.Log ("Heyo!: "+ gameOverScreen.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>());

        b.onClick.AddListener (delegate {Restart ();});
        reStartHard.onClick.AddListener(delegate { RestartFromBeginning(); });
        BackToMenu.onClick.AddListener(delegate { BackToTheMenu(); });

    }

	public void checkpointReached(GameObject checkpointReached){
		respawn = checkpointReached.transform;
	}

    public void RestartFromBeginning() {
        //Reloading scene works ... the buttons I still need to fix tho ;-;
        SceneManager.LoadScene("Bugfixer_0_0_1");
    }

    public void BackToTheMenu() {
        SceneManager.LoadScene("FirstScene");
    }

	public void Restart(){

		Debug.Log("RESTARTING");

        //assign player to the new object (aka "Player")
        //player = this.gameObject;
        //Debug.Log ("new player assigneD");
        //New instance of Player is assigned to _p
        //folder is destroyed first!!!

        GameObject pF = (GameObject)Instantiate(playerFolder);
        //GameObject _p = (GameObject)Instantiate(player);
		//Debug.Log(_p.ToString());
        
        GameObject _p = pF.transform.FindChild("Player").gameObject;
        //Debug.Log(_p.ToString());

        //_p.GetComponent<PlayerLavaDeath> ().checkpointReached (respawn.gameObject);
        checkpointReached(respawn.gameObject);
        Debug.Log (_p.ToString ());
        //Debug.Log (respawn.ToString ());
        _p.transform.position = respawn.transform.position;

        _p.GetComponent<PlayerHealth>().PlayerHealthReset();

        //_p.GetComponent<PlayerHealth>().HealthReset();

        //Debug.Log ("New Player Instantiated at " + respawn.transform.position);
        //all checkpoints get player updated
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		for (int x = 0; x < checkpoints.Length; x++) {
			Debug.Log("Checking checkpoint " + x);
			checkpoints[x].GetComponent<Checkpoint>().updatePlayer(_p);
		}


        GameObject.FindGameObjectWithTag("MainCameraMovement").GetComponent<CameraScript> ().AssignPlayer (_p);
        GameObject.FindGameObjectWithTag("InRoom").GetComponent<InRoomScript>().AssignPlayer(_p);

        Destroy(playerFolder.gameObject);
        //loseScreen.gameObject.SetActive(false);

        AssignPlayer(_p, pF);

		Deaths++;
        //Debug.Log ("Old player destroyed");
        //assignes to main camera script the new player, _p
        //Debug.Log ("Camera re-assigned");

        //for each player, assign new player
        /*
		GameObject[] listOfNPCs = GameObject.FindGameObjectsWithTag ("NPC");
		for (int x = 0; x < listOfNPCs.Length; x++) {
			listOfNPCs [x].GetComponent<NPC_Follow> ().AssignPlayer (_p);
			Debug.Log ("Assignment attempted");
		}
		*/
    }
}
