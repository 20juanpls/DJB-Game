using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour {

    PlayerMovement_Ver2 PlayerP;
    Transform HazardT;
    Rigidbody PlayerRb;

    public bool collided, Inactive = false, cantTakeDamage, jumpedOn, DangerousFall, HasFallen;//, takeAwayHealth;
    public Vector3 FinalKnockBack;
    public float knockbackMultiplier, RecoverTime, KnockBackJumpForce, MinFloorDistFallDamage;
    public int totalDamage;

    private float TimeLeft, currentKnockBackJumpForce;
    private Vector3 KnockBackOrientation, hitVector;

    // Use this for initialization
    void Start () {
        PlayerP = this.GetComponent<PlayerMovement_Ver2>();
        PlayerRb = this.GetComponent<Rigidbody>();
        collided = false;

        TimeLeft = RecoverTime;
    }

    // Update is called once per frame
    void Update() {
        //ThisIsATest();
        //Debug.Log("Collided?: "+collided);
        if (this.GetComponent<PlayerHealth>().IsDead == true)
        {
            Inactive = true;
            KnockBackOrientation = Vector3.zero;
            currentKnockBackJumpForce = 0.0f;
            ForceAdder();
        }
        else {
            Inactive = false;
        }

        if (Inactive == false)
        {

			FallDamage ();


            if (collided == true)//&& itStoppedMoving == false)
            {

                PlayerP.DontMove = true;
                ForceAdder();
                
            }
            else
            {
                if (this.GetComponent<PlayerHealth>().IsDead == false && this.GetComponent<PlayerHealth>().Crushing == false)
                {
                    PlayerP.DontMove = false;
                }
                currentKnockBackJumpForce = KnockBackJumpForce;
            }

			//Debug.Log (PlayerP.forKnockBack);
			//Debug.Log ("de jamp:"+jumpedOn);
			if (jumpedOn == true) {
				PlayerP.initialAirSpeed = currentKnockBackJumpForce*2.0f;

				if (PlayerP.forKnockBack == true)
				{
					PlayerP.initialAirSpeed = 0.0f;
					jumpedOn = false;
				}
			}
            //JumpingC.IsItGrounded();
        }

    }

	void FallDamage(){
		//Debug.Log (PlayerRb.velocity.y);
		if (PlayerP.floorDist >= MinFloorDistFallDamage && PlayerRb.velocity.y <= -PlayerP.terminalSpeed){
			DangerousFall = true;
		}

		if (PlayerRb.velocity.y >= 0.0f && DangerousFall == true && PlayerP.isGrounded == false){
			DangerousFall = false;
		}

		if (DangerousFall == true && PlayerP.isGrounded == true) {
			totalDamage += 1;
			DangerousFall = false;
			HasFallen = true;
		} 
	}

    void OnTriggerEnter(Collider other){

        if (other.tag == "EpicentralHazard") {
            collided = true;
           // takeAwayHealth = true;
            HazardT = other.GetComponent<Transform>();
            hitVector = new Vector3(HazardT.transform.position.x - PlayerRb.transform.position.x, 0.0f, HazardT.transform.position.z - PlayerRb.transform.position.z);
            KnockBackOrientation = hitVector*-1.0f;
            if (cantTakeDamage == false)
            {
                totalDamage += 1;
            }
            //KnockBackOrientation = HazardT.transform.rotation * Vector3.forward;
        }
        if (other.tag == "hazard")
        {
            collided = true;
           // takeAwayHealth = true;
            HazardT = other.GetComponent<Transform>();
            //hitVector = new Vector3(HazardT.transform.position.x - PlayerRb.transform.position.x, 0.0f, HazardT.transform.position.z - PlayerRb.transform.position.z);
            //KnockBackOrientation = hitVector * -1.0f;
            KnockBackOrientation = HazardT.transform.rotation * Vector3.forward;
            if (cantTakeDamage == false)
            {
                totalDamage += 1;
            }
        }

        if (other.tag == "HeartHP")
        {
            if (totalDamage > 0)
            {
                totalDamage -= 1;
            }
        }

		if (other.tag == "JumpCollider") {
			jumpedOn = true;
			currentKnockBackJumpForce = KnockBackJumpForce*1.2f;
		}

    }

    /*  void OnTriggerExit(Collider other)
       {
           //check for collision with anything tagged "NPC_Collider"
           if (other.tag == "EpicentralHazard")
           {
            takeAwayHealth = false;
           }
           if (other.tag == "hazard")
           {
            takeAwayHealth = false;
           }
       }*/

    void ForceAdder() {
        if (PlayerP.isGrounded == true)
        {
            TimeLeft -= Time.deltaTime;

            FinalKnockBack = KnockBackOrientation * TimeLeft * knockbackMultiplier;
            PlayerP.initialAirSpeed = currentKnockBackJumpForce;

            if (PlayerP.forKnockBack == true)
            {
                currentKnockBackJumpForce = 0.0f;
            }


            if (TimeLeft <= 0.0f)
            {
                TimeLeft = RecoverTime;
                collided = false;
            }
        }
    }


}
