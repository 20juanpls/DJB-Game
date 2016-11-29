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
		loseScreen = GameObject.Find ("LoseScreenCanvas");
		loseScreen.SetActive (false);
		player = this.gameObject.transform.parent.gameObject;
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
		GameObject _p = (GameObject)Instantiate (player, respawn);
		Destroy (this.gameObject.transform.parent.gameObject);
		Camera.main.GetComponent<CameraScript> ().AssignPlayer (_p);
		GameObject[] listOfNPCs = GameObject.FindGameObjectsWithTag ("NPC");
		for (int x = 0; x < listOfNPCs.Length; x++) {
			listOfNPCs [x].GetComponent<NPC_Follow> ().AssignPlayer (_p);
		}
	}
}
