using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerLavaDeath : MonoBehaviour {

	public GameObject loseScreen;
	GameObject player;
	Transform respawn;

	// Use this for initialization
	void Start () {
		loseScreen = GameObject.Find ("LoseScreenCanvas");
		loseScreen.SetActive (false);
		player = GameObject.Find ("Player_folder");
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Kill") {
			loseScreen.gameObject.SetActive (true);
		}
	}

	public void checkpointReached(GameObject checkpointReached){
		respawn = checkpointReached.transform;
		Debug.Log ("Respawn point is now assigned to " + respawn.ToString ());
	}

	public void Restart(){
		Instantiate (player, respawn);
	}
}
