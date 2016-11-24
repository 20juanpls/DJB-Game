using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour {

    PlayerControls PlayerP;
    RelativGrav JumpingC;
    Rigidbody npcRB;
    Rigidbody TransP;

    private bool collided, onGround;
    private Vector3 KnockBackOrientation;

	// Use this for initialization
	void Start () {
        PlayerP = this.GetComponent<PlayerControls>();
        JumpingC = this.GetComponent<RelativGrav>();
        //^Use for JumpingC.Isitgrounded();
        TransP = this.GetComponent<Rigidbody>();
        collided = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (collided == true) {
            TransP.AddForce(KnockBackOrientation*150.0f);
            JumpingC.setInitialSpeed(-10.0f);
            StartCoroutine(KnockBackTime(0.7f));
        }
        onGround = JumpingC.IsItGrounded();

    }

	void OnTriggerEnter(Collider other){

		//check for collision with anything tagged "NPC_Collider"
		if (other.tag == "NPC_Collider") {
			Debug.Log ("Collision with player from NPC face detected!");

            collided = true;
			//Get NPC Rigidbody then reverse it's velocity
			npcRB = other.GetComponentInParent<Rigidbody>();
			npcRB.velocity = -npcRB.velocity*0.8f;
            KnockBackOrientation = npcRB.transform.rotation*Vector3.forward;

        }
	}
    void OnTriggerExit(Collider other)
    {
        //check for collision with anything tagged "NPC_Collider"
        if (other.tag == "NPC_Collider")
        {
            Debug.Log("Collision with player from NPC face detected!");

            collided = false;
        }
    }
    
    IEnumerator KnockBackTime( float KnockTime){
        PlayerP.setPlayerActivity(false);
        yield return new WaitForSeconds(KnockTime);
        PlayerP.setPlayerActivity(true);
    }

}
