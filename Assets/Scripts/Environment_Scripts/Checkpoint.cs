using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public bool activated;
	GameObject flag;

	// Use this for initialization
	void Start () {
		activated = false;
		flag = this.gameObject.transform.FindChild("Flag").gameObject;
		flag.SetActive (false);
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "PlayerMesh") {
			flag.SetActive (true);
			GameObject.Find ("Player").GetComponent<PlayerLavaDeath> ().checkpointReached (this.gameObject);
		}
	}

}
