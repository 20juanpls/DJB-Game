using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour {

	Rigidbody npcRB;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){

		//check for collision with anything tagged "NPC_Collider"
		if (other.tag == "NPC_Collider") {
			Debug.Log ("Collision with player from NPC face detected!");

			//Get NPC Rigidbody then reverse it's velocity
			npcRB = other.GetComponentInParent<Rigidbody>();
			npcRB.velocity = -npcRB.velocity;
		}
	}

}
