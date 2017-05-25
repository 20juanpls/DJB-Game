using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerLavaDeath : MonoBehaviour {

    public GameObject loseScreen;
    public GameObject gameOverScreen;
    public bool LoseScreenActive;
    public bool gameOverScreenActive;
	//public GameObject playerFolder;
  	public GameObject player;
	Transform respawn;
  	Quaternion PlayerRot;

	//Coin spawning variables
	GameObject[] coins;
	public GameObject coinPrefab;

	//heart spawning variables
	GameObject[] hearts;
	public GameObject heartPrefab;

    public bool PlayerWinState;

	public PlayerMovement_Ver2 playerMovementVer2;

	public int Deaths;

	public AudioSource background;
	public AudioClip deathClip;
	public AudioClip backgroundClip;

	bool playingDed = false;

	// Use this for initialization
	void Start () {
		//loseScreen = GameObject.Find ("LoseScreenCanvas");
		//gameOverScreen = GameObject.Find ("GameOverCanvas");
        //assignButton();
        //loseScreen.SetActive (false);
		//gameOverScreen.SetActive (false);
        player = GameObject.FindGameObjectWithTag("PlayerMesh");
		//playerFolder = GameObject.FindGameObjectWithTag("PlayerFolder");
        PlayerRot = player.transform.rotation;

		//coin resetting stuff
		coins = GameObject.FindGameObjectsWithTag("Coin");

		//heart resetting stuff
		hearts = GameObject.FindGameObjectsWithTag("HeartHP");

		background = this.GetComponent<AudioSource> ();

		background.clip = backgroundClip;
		background.Play();
    }

    void Update() {

		//Debug.Log ("Deaths:"+Deaths);
		if (player.GetComponent<PlayerHealth> ().Lives - 1 == Deaths && player.GetComponent<PlayerHealth> ().IsDead == true) {
			gameOverScreen.gameObject.SetActive (true);
			player.GetComponent<PlayerMovement_Ver2> ().DontMove = true;
			if (!playingDed){
				playingDed = true;
				Debug.Log ("i am ded");
				background.Stop ();
				background.clip = deathClip;
				background.Play();
				loseScreen.gameObject.SetActive (true);	
			}
            //Deaths++;
        } else {
			if (player.GetComponent<PlayerHealth> ().IsDead == true) {
				if (!playingDed){
					playingDed = true;
					Debug.Log ("i am ded");
					background.Stop ();
					background.clip = deathClip;
					background.Play();
					loseScreen.gameObject.SetActive (true);	
				}
                //Deaths++;
            } else {
				loseScreen.gameObject.SetActive (false);
				playingDed = false;
			}
		}

        if (PlayerWinState == true) {
            Debug.Log("GOAL!!!");
            SceneManager.LoadScene("Stage_Hub");
        }

		/*if (loseScreen.gameObject.activeSelf) {
			if (Input.GetKeyDown("joystick button 11") || Input.GetKeyDown("joystick button 0")|| Input.GetKeyDown(KeyCode.Return)){
				loseScreen.transform.GetComponentInChildren<Button>().onClick.Invoke();
			}
		}
		if (gameOverScreen.gameObject.activeSelf) {
			if (Input.GetKeyDown("joystick button 11") || Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Return))
            {
                //gameOverScreen.transform.GetComponentInChildren<Button>().onClick.Invoke();
                gameOverScreen.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
            }
            if (Input.GetKeyDown("joystick button 1")|| Input.GetKeyDown(KeyCode.Backspace)) {
                gameOverScreen.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().onClick.Invoke();
            }
		}*/
    }

    void AssignPlayer(GameObject p)
    {
        //playerFolder = pF.gameObject;
        player = p.gameObject;
        //Debug.Log(player.ToString());
    }

	/*void assignButton(){
		Button b = loseScreen.transform.GetChild (0).transform.GetChild (1).GetComponent<Button> ();
		if (b == null) {
			Debug.Log ("ERR");
		}
        Button reStartHard = gameOverScreen.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();//GameObject.Find("RestartFromBeginning").GetComponent<Button>();
        Button BackToMenu = gameOverScreen.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();//GameObject.Find("BackToMenu").GetComponent<Button>();

        //Debug.Log ("Heyo!: "+ gameOverScreen.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>());

		b.onClick.AddListener (delegate {Restart ();});
        reStartHard.onClick.AddListener(delegate { RestartFromBeginning(); });
        BackToMenu.onClick.AddListener(delegate { BackToTheMenu(); });

    }*/

	public void checkpointReached(GameObject checkpointReached){
		respawn = checkpointReached.transform;
	}

    public void RestartFromBeginning() {
        //Reloading scene works ... the buttons I still need to fix tho ;-;
        //SceneManager.LoadScene("Bugfixer_0_0_1");
        SceneManager.LoadScene("Stage_Hub");
    }

    /*public void BackToTheMenu() {
        SceneManager.LoadScene("FirstScene");
    }*/

	public void Restart(){

		background.clip = backgroundClip;
		background.Play ();
		//Reset coins
		ResetCollectables();

        //assign player to the new object (aka "Player")
        //player = this.gameObject;
        //Debug.Log ("new player assigneD");
        //New instance of Player is assigned to _p
        //folder is destroyed first!!!

        GameObject _p = (GameObject)Instantiate(player);
        //GameObject _p = (GameObject)Instantiate(player);
		//Debug.Log(_p.ToString());

        //GameObject _p = pF.transform.FindChild("Player").gameObject;
        //Debug.Log(_p.ToString());

		//>>>>
        //GameObject _SptSh = pF.transform.FindChild("SpotShadow").gameObject;
		//<<<<

        //_p.GetComponent<PlayerLavaDeath> ().checkpointReached (respawn.gameObject);
        checkpointReached(respawn.gameObject);
        //Debug.Log (_p.ToString ());
        //Debug.Log (respawn.ToString ());


        _p.transform.position = respawn.transform.position;
        _p.transform.rotation = PlayerRot;

        _p.GetComponent<PlayerHealth>().PlayerHealthReset();


		//>>>>
        //_SptSh.GetComponent<SpotShadowScript>().ResetShadow(_p);
		//<<<<

        //Debug.Log ("New Player Instantiated at " + respawn.transform.position);
        //all checkpoints get player updated
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		for (int x = 0; x < checkpoints.Length; x++) {
			//Debug.Log("Checking checkpoint " + x);
			checkpoints[x].GetComponent<Checkpoint>().updatePlayer(_p);
		}


        //Both of these are cameraMovements;
        //GameObject.FindGameObjectWithTag("MainCameraMovement").GetComponent<CameraScript> ().AssignPlayer (_p);
        //Both of these are cameraMovements;
		GameObject.FindGameObjectWithTag ("CamMovement_2.0").GetComponent<OfficialCameraMovement> ().AssignPlayer (_p);
        //Both of these are cameraMovements;

        this.GetComponent<ThePause>().AssignPlayer(_p);

        //GameObject.FindGameObjectWithTag("InRoom").GetComponent<InRoomScript>().AssignPlayer(_p);
        GameObject.Find("HealthCanvas").GetComponent<HeartContainer_Script>().PlayerHeartIllustratorReset(_p);

        //GameObject.FindGameObjectWithTag("P_SpotShadow").GetComponent<SpotShadowScript>().AssignPlayer(_p);

        Destroy(player.gameObject);
        //loseScreen.gameObject.SetActive(false);

        Deaths++;

        AssignPlayer(_p);

        //Debug.Log ("Old player destroyed");
        //assignes to main camera script the new player, _p
        //Debug.Log ("Camera re-assigned");

        //playerMovementVer2.IsGround_2 = false;

        //for each player, assign new player
        GameObject[] StompEnemies = GameObject.FindGameObjectsWithTag("StompNPC");
        for (int i = 0; i < StompEnemies.Length; i++)
        {
            if (StompEnemies[i].GetComponent<NPCStomper>() == null)
                StompEnemies[i].GetComponent<NewNPCStompScript>().AssignPlayer(_p);
            else
                StompEnemies[i].GetComponent<NPCStomper>().AssignPlayer(_p);
        }

        GameObject[] FlyNPCs = GameObject.FindGameObjectsWithTag("FlyEnemy");
        for (int i = 0; i < FlyNPCs.Length; i++)
        {
            FlyNPCs[i].transform.GetChild(0).GetComponent<FlyingNPC_HeadMovement>().AssignPlayer(_p);
        }

        GameObject[] listOfNPCs = GameObject.FindGameObjectsWithTag ("NPC_charge");
		for (int x = 0; x < listOfNPCs.Length; x++) {
			listOfNPCs [x].GetComponent<NPC_Follow> ().AssignPlayer (_p);
			//Debug.Log ("Assignment attempted");
		}

        GameObject[] Teleporters = GameObject.FindGameObjectsWithTag("teleporter");
        for (int j = 0; j < Teleporters.Length; j++) {
            Teleporters[j].GetComponent<TeleporterScript>().AssignPlayer(_p);
        }

        /*GameObject[] BoulderSpawners = GameObject.FindGameObjectsWithTag("BowlderSpawner");
        for (int j = 0; j < BoulderSpawners.Length; j++)
        {
            BoulderSpawners[j].GetComponent<BowlderSpawner>().AssignPlayer(_p);
        }*/



    }


	void ResetCollectables(){
		for (int x = 0; x < coins.Length; x++){
			coins[x].GetComponent<MeshRenderer>().enabled = true;
            coins[x].GetComponent<Collider>().enabled = true;
        }
		for (int y = 0; y < hearts.Length; y++){
			hearts[y].GetComponent<MeshRenderer>().enabled = true;
            hearts[y].GetComponent<Collider>().enabled = true;
        }
	}


}
