using UnityEngine;
using System.Collections;

public class PlayerNPCKill : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.name == "JumpCollider"){
			NPC_Death nd = other.gameObject.transform.parent.gameObject.GetComponent<NPC_Death>();
			nd.activateDeath();
            this.gameObject.GetComponent<PlayerKnockback>().jumpedOn = true;
		}
	}

}
