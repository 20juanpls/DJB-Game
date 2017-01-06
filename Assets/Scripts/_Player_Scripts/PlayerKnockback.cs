﻿using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour {

    PlayerMovement_Ver2 PlayerP;
    Transform HazardT;
    Rigidbody PlayerRb;

    public bool collided, Inactive = false, cantTakeDamage;//, takeAwayHealth;
    public Vector3 FinalKnockBack;
    public float knockbackMultiplier, RecoverTime, KnockBackJumpForce;
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
        //Debug.DrawRay(PlayerRb.position, hitVector, Color.red);
        if (this.GetComponent<PlayerHealth>().IsDead == true)
        {
            Inactive = true;
            KnockBackOrientation = Vector3.zero;
            ForceAdder();
        }
        else {
            Inactive = false;
        }

        if (Inactive == false)
        {
            if (collided == true)//&& itStoppedMoving == false)
            {

                PlayerP.DontMove = true;
                ForceAdder();
                
            }
            else
            {
                if (this.GetComponent<PlayerHealth>().IsDead == false)
                {
                    PlayerP.DontMove = false;
                }
                currentKnockBackJumpForce = KnockBackJumpForce;
            }
            //JumpingC.IsItGrounded();
        }

    }

     void OnTriggerEnter(Collider other){

           //check for collision with anything tagged "NPC_Collider"
           /*if (other.tag == "NPC_Collider") {
               collided = true;
               //Get NPC Rigidbody then reverse it's velocity
               npcRB = other.GetComponentInParent<Rigidbody>();
               npcRB.velocity = -npcRB.velocity;//*0.8f;
               KnockBackOrientation = npcRB.transform.rotation*Vector3.forward;
               //Debug.Log(KnockBackOrientation.magnitude);
           }*/
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
