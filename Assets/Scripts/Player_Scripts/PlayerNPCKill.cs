using UnityEngine;
using System.Collections;

public class PlayerNPCKill : MonoBehaviour {
	NPC_Death nd;

	void Start(){
	}
	void Update(){
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "JumpCollider"){
			nd = other.gameObject.transform.parent.gameObject.GetComponent<NPC_Death>();
			//nd.activateDeath();
            this.gameObject.GetComponent<PlayerKnockback>().jumpedOn = true;
		}
	}
	void OnTriggerExit(Collider other){
		if (other.tag == "JumpCollider"){
            //NPC_Death nd = other.gameObject.transform.parent.gameObject.GetComponent<NPC_Death>();
            if (nd != null)
            {
                nd.activateDeath();
            }
			this.gameObject.GetComponent<PlayerKnockback>().jumpedOn = true;
		}
	}
}
