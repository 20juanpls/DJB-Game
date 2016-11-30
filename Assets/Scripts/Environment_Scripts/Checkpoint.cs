using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	GameObject flag;

	// Use this for initialization
	void Start () {
		flag = this.gameObject.transform.FindChild("Flag").gameObject;
		flag.SetActive (false);
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "PlayerMesh") {
			flag.SetActive (true);
			other.gameObject.GetComponent<PlayerLavaDeath> ().checkpointReached (this.gameObject);
			Debug.Log ("CHECKPOINT REACHED");
		}
	}

}
