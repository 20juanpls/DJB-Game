using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	GameObject flag;
	GameObject player;
	public float setDistance;

	// Use this for initialization
	void Start () {
		flag = this.gameObject.transform.FindChild("Flag").gameObject;
		flag.SetActive (false);
		player = GameObject.Find ("Player");

		if (setDistance == 0.0f) {
			Debug.Log ("Auto-set 'setDistance' to 10.0f in " + this.ToString ());
			setDistance = 10.0f;
		}
		if (player == null) {
			Debug.Log ("player not found in " + this.ToString());
		}
	}


	void Update(){
		//if player is within setDistance away
		if (Vector3.Distance (this.transform.position, player.transform.position) <= setDistance && !flag.activeSelf) {

			if (!flag.activeSelf) {
				Debug.Log ("CHECKPOINT REACHED: " + this.ToString());
			}

			flag.SetActive (true);
			player.GetComponent<PlayerLavaDeath> ().checkpointReached (this.gameObject);

		}
	}

	public void updatePlayer(GameObject _p){
		player = _p;
	}
		

}
