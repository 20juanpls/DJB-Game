using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNPC_HeadMovement : MonoBehaviour {
    Rigidbody FlyNPC_Head;
    Transform PlayerT;
    Transform Home;
    Transform Target;
    Transform Propeller;

    public float HLookRotSpeed, IdleRotSpeed, HStunnedRotSpeed, HomeDistance = 5.0f, AttakSpeed, IdleSpeed, CoolDownTime;
    public bool lookInactive, Attack, CurrentCoolTime;
    Vector3 FinalVel;

    Quaternion HLook, VLook, FHorizLook, FVertLook;//, FinalLookRot;

    float CurrentRotationSpeed, PlayDistFromHome, NPCFDistFromHome, CurrentSpeed, groundDist, PropellerSpeed;

	// Use this for initialization
	void Start () {
        FlyNPC_Head = this.gameObject.GetComponent<Rigidbody>();
        PlayerT = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        Home = this.transform.parent.gameObject.GetComponent<Transform>();
        Propeller = this.transform.GetChild(0).gameObject.GetComponent<Transform>();

    }
	
	// Update is called once per frame
	void Update () {
        HomeDistanceMeasurer();
        FloorMeasure();

        if (PlayDistFromHome <= HomeDistance)
            Attack = true;

        if (NPCFDistFromHome >= HomeDistance)
            Attack = false;

        if (Attack == true)
        {
            Target = PlayerT;
            CurrentSpeed = AttakSpeed;
            CurrentRotationSpeed = HLookRotSpeed;
        }
        else {
            Target = Home;
            CurrentSpeed = IdleSpeed;
            CurrentRotationSpeed = IdleRotSpeed;
        }

        PropellerSpinner();
        LookingRotation();
        VelocitySetter();
        FlyNPC_Head.velocity = FinalVel;
    }

    void LookingRotation() {
        if (lookInactive == false)
        {
            Vector3 TarGate = new Vector3(Target.position.x, 0.0f, Target.position.z) - new Vector3(FlyNPC_Head.position.x, 0.0f, FlyNPC_Head.position.z);
            if (TarGate.magnitude <= 0.5f) {
                TarGate = Vector3.forward;
            }
            //Debug.Log(TarGate.magnitude);
            HLook = Quaternion.LookRotation(TarGate);

            //VLook = Quaternion.LookRotation(Target.position - FlyNPC_Head.position);

            FHorizLook = Quaternion.Slerp(FlyNPC_Head.rotation, HLook, Time.deltaTime * CurrentRotationSpeed);

            //Debug.DrawRay(FlyNPC_Head.position, VLook* Vector3.forward * 10.0f, Color.green);

            FlyNPC_Head.rotation = FHorizLook;
        }

    }
    void VelocitySetter() {
        Vector3 DecendingSpeed;
        if (Target == PlayerT)
        {
            //Debug.Log(groundDist);
            DecendingSpeed = new Vector3(0.0f, -4.0f, 0.0f);
            if (groundDist <= 2.0f)
            {
                DecendingSpeed = Vector3.zero;
            }
        }
        else {
            if (FlyNPC_Head.position.y <= Home.position.y)
            {
                DecendingSpeed = new Vector3(0.0f, 2.0f, 0.0f);
            }
            else
            {
                DecendingSpeed = Vector3.zero;
            }
        }
        FinalVel = (FlyNPC_Head.rotation * Vector3.forward * CurrentSpeed)+(DecendingSpeed);

        //Debug.Log(FinalVel);
    }

    void HomeDistanceMeasurer() {
        PlayDistFromHome = (PlayerT.transform.position - Home.transform.position).magnitude;
        NPCFDistFromHome = (FlyNPC_Head.transform.position - Home.transform.position).magnitude;

    }

    void FloorMeasure()
    {
        RaycastHit hit;
        if (Physics.Raycast(FlyNPC_Head.position, new Vector3(0.0f, -1.0f, 0.0f), out hit))
        {
            if (hit.transform.tag != "PlayerMesh" )
            {
                groundDist = hit.distance;
            }
        }
    }

    void PropellerSpinner() {
        if (Attack == true)
        {
            PropellerSpeed = AttakSpeed * 300.0f;
        }
        else {
            PropellerSpeed = IdleSpeed * 300.0f;
        }

        Propeller.RotateAround(Propeller.position, Propeller.right, PropellerSpeed * Time.deltaTime);

    }
}
