using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour {

    PlayerMovement_Ver2 PlayerP;
    PlayerHealth PlayerH;
    Transform HazardT, ForceDir;
    Rigidbody PlayerRb;

    public bool collided, Inactive = false, cantTakeDamage, jumpedOn, DangerousFall, HasFallen;//, takeAwayHealth;
    public Vector3 FinalKnockBack;
    public float knockbackMultiplier, RecoverTime, AirRecovTime, KnockBackJumpForce, MinFloorDistFallDamage;
    public int totalDamage;

    private float TimeLeft, AirRecovLeft, currentKnockBackJumpForce;
    private Vector3 KnockBackOrientation, hitVector, ExForceVector;

    // Use this for initialization
    void Start () {
        PlayerP = this.GetComponent<PlayerMovement_Ver2>();
        PlayerRb = this.GetComponent<Rigidbody>();
        PlayerH = this.GetComponent<PlayerHealth>();
        collided = false;

        TimeLeft = RecoverTime;
		AirRecovLeft = AirRecovTime;
    }

    // Update is called once per frame
    void Update() {
        if (PlayerP.Paused == false)
        {
            if (PlayerH.IsDead == true)
            {
                Inactive = true;
                KnockBackOrientation = Vector3.zero;
                currentKnockBackJumpForce = 0.0f;
                ForceAdder();
                DangerousFall = false;
            }
            else
            {
                Inactive = false;
            }

            if (Inactive == false)
            {

                FallDamage();


                if (collided == true)//&& itStoppedMoving == false)
                {

                    PlayerP.DontMove = true;
                    ForceAdder();

                }
                //
                //
                else
                {
                    if (this.GetComponent<PlayerHealth>().IsDead == false && this.GetComponent<PlayerHealth>().Crushing == false)
                    {
                        PlayerP.DontMove = false;
                    }
                    //partofsqueze
                    /*if (jumpedOn == true)
                    {
                        PlayerP.initialAirSpeed = currentKnockBackJumpForce * 2.0f;
                        //Debug.Log("ayy wot?");

						if (PlayerP.forKnockBack == true && PlayerP.isGrounded == true)
                        {
                            //Debug.Log("why tho?");
                            PlayerP.initialAirSpeed = 0.0f;
                            jumpedOn = false;
                        }
                    }
                    else
                    {*/
                    //---------------
                        currentKnockBackJumpForce = KnockBackJumpForce;
                    //----------------
                    //}
                    //--------------------
                }

                //Debug.Log (PlayerP.forKnockBack);
                //Debug.Log ("de jamp:"+jumpedOn);
                if (jumpedOn == true)
                {
					AirRecovLeft -= Time.deltaTime;
                    PlayerP.initialAirSpeed = currentKnockBackJumpForce * 2.0f;
					/*if (PlayerP.forKnockBack == true || PlayerP.initialAirSpeed < currentKnockBackJumpForce*2.0f)
                    {
                        PlayerP.initialAirSpeed = 0.0f;
                        jumpedOn = false;
                    }*/

					//Testing!!!
					if (PlayerP.forKnockBack == true && PlayerP.initialAirSpeed == currentKnockBackJumpForce*2.0f && AirRecovLeft < 0.0f && PlayerP.isGrounded == true){
						//Debug.Log ("Landing!!");
						AirRecovLeft = AirRecovTime;
						PlayerP.initialAirSpeed = 0.0f;
						jumpedOn = false;
					}

					if (PlayerP.forKnockBack == true && !PlayerP.isGrounded) {
						Debug.Log ("Jumping again!!");
					}
					//Testing!!!
                }

                //JumpingC.IsItGrounded();
            }
        }

        }

	    void FallDamage(){
                if (PlayerP.floorDist >= MinFloorDistFallDamage && PlayerP.CurrentOldVel.y <= -PlayerP.terminalSpeed)
                {
                    DangerousFall = true;
                }

                if (PlayerP.CurrentOldVel.y >= 0.0f && DangerousFall == true && PlayerP.isGrounded == false)
                {
                    DangerousFall = false;
                }

            if (DangerousFall == true && PlayerP.isGrounded == true && PlayerP.GroundCannotKill == true)
            {
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
        if (other.tag == "BoulderHazard") {
            collided = true;
            // takeAwayHealth = true;
            HazardT = other.GetComponent<Transform>();
            hitVector = new Vector3(HazardT.transform.position.x - PlayerRb.transform.position.x, 0.0f, HazardT.transform.position.z - PlayerRb.transform.position.z);
            KnockBackOrientation = hitVector * -1.0f;
            if (cantTakeDamage == false)
            {
                totalDamage += 1;
            }
        }
        if (other.tag == "hazard")
        {
            collided = true;
            HazardT = other.GetComponent<Transform>();
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

		/*if (other.tag == "JumpCollider") {
			jumpedOn = true;
			currentKnockBackJumpForce = KnockBackJumpForce*1.2f;
		}*/

        if (other.tag == "WindyArea")
        {
            ForceDir = other.GetComponent<Transform>();
            ExForceVector = ForceDir.rotation* Vector3.forward * other.GetComponent<WindSetter>().WindSpeed;
            //Debug.Log(ForceDir.rotation.y);
            PlayerP.ExForceVelocity = ExForceVector;
        }

    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "WindyArea")
        {
            PlayerP.ExForceVelocity = Vector3.zero;
        }
    }

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
