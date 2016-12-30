using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerLavaDeath : MonoBehaviour {

	public GameObject loseScreen;
	public GameObject player;
	Transform respawn;

	// Use this for initialization
	void Start () {
		assignButton();
		loseScreen = GameObject.Find ("LoseScreenCanvas");
		loseScreen.SetActive (false);
		player = this.gameObject;
	}

	void assignButton(){
		Button b = loseScreen.transform.GetChild (0).transform.GetChild (1).GetComponent<Button> ();
		Debug.Log (b);
		b.onClick.AddListener (delegate {
			Restart ();
		});
	}

	void OnTriggerEnter(Collider other){
		//on trigger collision with tagged "kill"
		if (other.tag == "Kill") {
			loseScreen.gameObject.SetActive (true);
		}
	}

	public void checkpointReached(GameObject checkpointReached){
		respawn = checkpointReached.transform;
	}

	public void Restart(){

		Debug.Log("RESTARTING");

		//assign player to the new object (aka "Player")
		//player = this.gameObject;
		//Debug.Log ("new player assigneD");
		//New instance of Player is assigned to _p
		GameObject _p = (GameObject)Instantiate(player);
		//Debug.Log(_p.ToString());
		_p.GetComponent<PlayerLavaDeath> ().checkpointReached (respawn.gameObject);
		//Debug.Log (_p.ToString ());
		//Debug.Log (respawn.ToString ());
		_p.transform.position = respawn.transform.position;
		//Debug.Log ("New Player Instantiated at " + respawn.transform.position);
		//all checkpoints get player updated
		GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		for (int x = 0; x < checkpoints.Length; x++) {
			Debug.Log("Checking checkpoint " + x);
			checkpoints[x].GetComponent<Checkpoint>().updatePlayer(_p);
		}


		Camera.main.GetComponent<CameraScript> ().AssignPlayer (_p);

		//destroy the old player, leaves the folder(?)
		Destroy (this.gameObject);


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
