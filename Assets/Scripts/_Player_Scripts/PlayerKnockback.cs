using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour {

    PlayerMovement_Ver2 PlayerP;
    PlayerHealth PlayerH;
    Transform HazardT, ForceDir;
    Rigidbody PlayerRb;

    public bool collided, Inactive = false, cantTakeDamage, jumpedOn, DangerousFall, HasFallen, InstaJamp;//, takeAwayHealth;
    public Vector3 FinalKnockBack;
    public float knockbackMultiplier, RecoverTime, KnockBackJumpForce, MinFloorDistFallDamage, recovOngroundT;
    public int totalDamage;

    private float TimeLeft, currentKnockBackJumpForce;
    private Vector3 KnockBackOrientation, hitVector, ExForceVector;

    public Quaternion hitRotation;

    // Use this for initialization
    void Start () {
        PlayerP = this.GetComponent<PlayerMovement_Ver2>();
        PlayerRb = this.GetComponent<Rigidbody>();
        PlayerH = this.GetComponent<PlayerHealth>();
        collided = false;

        TimeLeft = RecoverTime;
        recovOngroundT = RecoverTime * 0.4f;
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(DangerousFall);
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
        if (PlayerP.Paused == false)
        {
            /*if (PlayerH.IsDead == true)
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
            }*/

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

                        currentKnockBackJumpForce = KnockBackJumpForce;
                }

            }
        }

        }

	    void FallDamage(){
                if (PlayerP.floorDist >= MinFloorDistFallDamage && PlayerP.CurrentOldVel.y <= -PlayerP.terminalSpeed)
                {
                    DangerousFall = true;
                }

                if (PlayerP.CurrentOldVel.y >= 0.0f && DangerousFall == true && PlayerP.IsGround_2 == false)
                {
                    DangerousFall = false;
                }

            if (DangerousFall == true && PlayerP.IsGround_2 == true && PlayerP.GroundCannotKill == true)
            {
                totalDamage += 1;
                DangerousFall = false;
                HasFallen = true;
            }

	}

    void OnTriggerEnter(Collider other){

        if (other.tag == "EpicentralHazard"|| other.tag =="BoulderHazard") {
            collided = true;
           // takeAwayHealth = true;
            HazardT = other.GetComponent<Transform>();
            hitRotation = Quaternion.LookRotation(new Vector3(HazardT.transform.position.x - PlayerRb.transform.position.x, 0.0f, HazardT.transform.position.z - PlayerRb.transform.position.z));
            hitVector = hitRotation * Vector3.forward;
            KnockBackOrientation = hitVector*-1.0f;
            if (cantTakeDamage == false)
            {
                totalDamage += 1;
            }
            //KnockBackOrientation = HazardT.transform.rotation * Vector3.forward;
        }
        /*if (other.tag == "BoulderHazard") {
            collided = true;
            // takeAwayHealth = true;
            HazardT = other.GetComponent<Transform>();
            Vector3 tempVect = new Vector3(HazardT.transform.position.x - PlayerRb.transform.position.x, 0.0f, HazardT.transform.position.z - PlayerRb.transform.position.z);
            hitVector = Quaternion.LookRotation(tempVect) * Vector3.forward;
            KnockBackOrientation = hitVector * -1.0f;
            if (cantTakeDamage == false)
            {
                totalDamage += 1;
            }
        }*/
        if (other.tag == "hazard")
        {
            collided = true;
            HazardT = other.GetComponent<Transform>();
            Vector3 direction = HazardT.transform.rotation * -Vector3.forward;
            hitRotation = Quaternion.LookRotation(direction);
            KnockBackOrientation = hitRotation * Vector3.forward*-1;
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
        //if (PlayerP.IsGround_2 == true)
        //{
            TimeLeft -= Time.deltaTime;

            FinalKnockBack = KnockBackOrientation * (TimeLeft*0.6f) * knockbackMultiplier;
            PlayerP.initialAirSpeed = currentKnockBackJumpForce;

            if (PlayerP.IsGround_2 == false)
                InstaJamp = true;


            if (InstaJamp == true && PlayerP.IsGround_2 == true)
            {
                currentKnockBackJumpForce = 0.0f;
                InstaJamp = false;
            }

            if (PlayerP.IsGround_2 == true) {
                recovOngroundT -= Time.deltaTime;
                if (recovOngroundT <= 0.0f) {
                    recovOngroundT = 0.4f * RecoverTime;
                    collided = false;
                }
            }


            if (TimeLeft <= 0.0f )
            {
                TimeLeft = 0.0f;
                if (collided == false) {
                    TimeLeft = RecoverTime;
                }
            }
        //}

        //Debug.DrawRay(PlayerP.transform.position,new Vector3(FinalKnockBack.x, FinalKnockBack.y, FinalKnockBack.z),Color.blue);
    }


}
