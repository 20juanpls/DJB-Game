using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour {

	GameObject play;
	public float boost;

	void Start(){
		play = this.gameObject;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.LeftShift)) {
			play.GetComponent<PlayerControls> ().moveSpeed += boost;
		}

	}

}
