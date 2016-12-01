using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour {

    PlayerControls PlayerP;
    RelativGrav JumpingC;
    Rigidbody npcRB;
    Rigidbody TransP;

    public bool collided, itStoppedMoving;
    private Vector3 KnockBackOrientation;
	public float knockbackMultiplier, collidedTrueCounter;

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
        //Debug.Log("iscollided: "+collided);
        if (collided == true )//&& itStoppedMoving == false)
        {
            collidedTrueCounter++;
            //this is just to stop pushing me off to neverland
            if (collidedTrueCounter >= 4.0f) {
                collided = false;
                collidedTrueCounter = 0.0f;
            }
            TransP.AddForce(KnockBackOrientation*knockbackMultiplier);
            JumpingC.setInitialSpeed(-15.0f,false);
            StartCoroutine(KnockBackTime(0.4f));
        }
        //JumpingC.IsItGrounded();

    }

	void OnTriggerEnter(Collider other){

		//check for collision with anything tagged "NPC_Collider"
		if (other.tag == "NPC_Collider") {
            collided = true;
			//Get NPC Rigidbody then reverse it's velocity
			npcRB = other.GetComponentInParent<Rigidbody>();
            /*if (npcRB.velocity.magnitude < 2.0f)
            {
                itStoppedMoving = true;
            }
            else {
                itStoppedMoving = false;
            }*/
            npcRB.velocity = -npcRB.velocity;//*0.8f;
            KnockBackOrientation = npcRB.transform.rotation*Vector3.forward;
            //Debug.Log(KnockBackOrientation.magnitude);
        }
	}
    void OnTriggerExit(Collider other)
    {
        //check for collision with anything tagged "NPC_Collider"
        if (other.tag == "NPC_Collider")
        {
            //Debug.Log("Collision with player from NPC face detected!");

            collided = false;
        }
    }

    IEnumerator KnockBackTime( float KnockTime){
        PlayerP.setPlayerActivity(false);
        yield return new WaitForSeconds(KnockTime);
        PlayerP.setPlayerActivity(true);
    }

}
