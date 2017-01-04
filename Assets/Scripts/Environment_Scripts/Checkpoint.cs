using UnityEngine;
using System.Collections;

//gameobject should have a flag object attatched to it
//gameobject MUST BE TAGGED AS CHECKPOINT

public class Checkpoint : MonoBehaviour {

	GameObject flag;
    GameObject player;
    GameObject SpawnManager;
	public float setDistance;

	// Use this for initialization
	void Start () {
		flag = this.gameObject.transform.FindChild("Flag").gameObject;
		flag.SetActive (false);
        SpawnManager = GameObject.FindGameObjectWithTag ("SpawnManager");
        player = GameObject.Find("Player");

		if (setDistance == 0.0f) {
			Debug.Log ("Auto-set 'setDistance' to 10.0f in " + this.ToString ());
			setDistance = 10.0f;
		}
        if (SpawnManager == null) {
            Debug.Log("SpawnManager not found in " + this.ToString());
        }
		if (player == null) {
			Debug.Log ("player not found in " + this.ToString());
		}
	}


	void Update(){
		//if player is within setDistance away
		//Debug.Log(this.ToString());
		//Debug.Log(player.ToString());
		//Debug.Log(flag.ToString());
		if (Vector3.Distance (this.transform.position, player.transform.position) <= setDistance && !flag.activeSelf) {
				Debug.Log ("CHECKPOINT REACHED: " + this.ToString());
				flag.SetActive (true);
				SpawnManager.GetComponent<PlayerLavaDeath> ().checkpointReached (this.gameObject);
		}
	}

	public void updatePlayer(GameObject _p){
		player = _p;
	}


}
