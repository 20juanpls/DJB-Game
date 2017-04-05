using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNPC_HeadMovement : MonoBehaviour {
    Rigidbody FlyNPC_Head;
    Transform PlayerT;
    Transform Home;
    Transform Target;
    Transform Propeller;
    CollisionIndicator CollHitBox;

    public float HLookRotSpeed, IdleRotSpeed, HStunnedRotSpeed, HomeDistance = 5.0f, AttakSpeed, IdleSpeed, CoolDownTime;
    public bool lookInactive, Attack, IsCoolTime, CanLookAtPlayer, AmStuck, StartTurning1, StartTurning2;
    Vector3 FinalVel, FinalHeight, AntiWall;

    Quaternion HLook, VLook, FHorizLook, FVertLook;//, FinalLookRot;

    float CurrentRotationSpeed, PlayDistFromHome, NPCFDistFromHome, CurrentSpeed, groundDist, PropellerSpeed, CurrentCooldownT;
    float DownRayPosY;
    // Use this for initialization
	void Start () {
        FlyNPC_Head = this.gameObject.GetComponent<Rigidbody>();
        PlayerT = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        Home = this.transform.parent.gameObject.GetComponent<Transform>();
        Propeller = this.transform.GetChild(0).gameObject.GetComponent<Transform>();
        CollHitBox = Propeller.GetComponent<CollisionIndicator>();

        CurrentCooldownT = CoolDownTime;

    }
	
	// Update is called once per frame
	void Update () {
        HomeDistanceMeasurer();
        FloorMeasure();
        IsThereCoolDown();

        //Debug.Log(IsCoolTime);

        if (PlayDistFromHome <= HomeDistance && CanLookAtPlayer == true)
            Attack = true;

        if (((NPCFDistFromHome >= HomeDistance)&&(PlayDistFromHome>= HomeDistance)) || IsCoolTime == true || CanLookAtPlayer == false)
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
        FlyNPC_Head.position = new Vector3(FlyNPC_Head.position.x, FinalHeight.y, FlyNPC_Head.position.z);
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
            
            //EVERYTHING BELOW IS BOUND TO BE USED!!!!
            /*
            if (AmStuck == true) {
                //HLook = Quaternion.LookRotation(new Vector3(AntiWall.x,0.0f, AntiWall.y));
                StartTurning1 = true;
                IsCoolTime = true;
                Debug.Log("StartTime");
            }
            if (AmStuck == false && FlyNPC_Head.velocity.magnitude <= 0.1f) {
                //HLook = Quaternion.LookRotation(-TarGate);
                StartTurning2 = true;
                IsCoolTime = true;
                Debug.Log("StartTime");
            }

            if (StartTurning1) {
                if (!IsCoolTime) {
                    Debug.Log("EndTime");
                }
            }
            if (StartTurning2) {
                if (!IsCoolTime)
                {
                    Debug.Log("EndTime");
                }
            }*/


            Debug.DrawRay(FlyNPC_Head.position, HLook * Vector3.forward * 5.0f, Color.blue);

            //VLook = Quaternion.LookRotation(Target.position - FlyNPC_Head.position);

            FHorizLook = Quaternion.Slerp(FlyNPC_Head.rotation, HLook, Time.deltaTime * CurrentRotationSpeed);

            //Debug.DrawRay(FlyNPC_Head.position, VLook* Vector3.forward * 10.0f, Color.green);

            FlyNPC_Head.rotation = FHorizLook;
        }

    }
    void VelocitySetter() {
        float TargetPosY;
        float DecentSpeed;

        if (Target == PlayerT)
        {
            DecentSpeed = 2.0f;
            TargetPosY = PlayerT.position.y + 1.0f;
        }
        else {
            DecentSpeed = 1.0f;
            TargetPosY = Target.position.y;
        }

        if (groundDist <= 2.0f) {
            TargetPosY = -DownRayPosY + 2.0f;
            //Debug.Log("DragRacin!: " + DownRayPosY);
        }

        FinalVel = (FlyNPC_Head.rotation * Vector3.forward * CurrentSpeed);
        FinalHeight = Vector3.Lerp(new Vector3(0.0f, FlyNPC_Head.position.y, 0.0f), new Vector3(0.0f, TargetPosY, 0.0f), DecentSpeed * Time.deltaTime);
        //Debug.Log(FinalVel);
    }

    void HomeDistanceMeasurer() {
        PlayDistFromHome = (PlayerT.transform.position - Home.transform.position).magnitude;
        NPCFDistFromHome = (FlyNPC_Head.transform.position - Home.transform.position).magnitude;

    }

    void FloorMeasure()
    {
        RaycastHit PlayHit;
        RaycastHit Homehit;
        RaycastHit hit;
        if (Physics.Raycast(FlyNPC_Head.position, PlayerT.position - FlyNPC_Head.position, out PlayHit))
        {
            //Debug.Log(PlayHit.transform.tag);
            //Debug.DrawRay(FlyNPC_Head.position, PlayerT.position - FlyNPC_Head.position, Color.red);
            if (PlayHit.transform.tag != "PlayerMesh")
            {
                //Debug.Log("is this happening?");
                CanLookAtPlayer = false;
            }
            else {
                CanLookAtPlayer = true;
            }
        }
        if (Physics.Raycast(FlyNPC_Head.position, Home.position - FlyNPC_Head.position, out Homehit)) {
            if (Homehit.transform.tag != "PlayerMesh" && Attack == false) {
                if (Homehit.distance <= 3.0f)
                {
                    Debug.Log("AmStuck");
                    AmStuck = true;
                    AntiWall = Homehit.normal;
                }
                else {
                    AmStuck = false;
                }
            }

        }
        if (Physics.Raycast(FlyNPC_Head.position, new Vector3(0.0f, -1.0f, 0.0f), out hit))
        {
            if (hit.transform.tag != "PlayerMesh" )
            {
                groundDist = hit.distance;
                DownRayPosY = hit.transform.position.y;
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

    void IsThereCoolDown() {
        if (CollHitBox.HitsPlayer == true)
        {
            IsCoolTime = true;
        }

        //Debug.Log(CurrentCooldownT);
        if (IsCoolTime == true)
        {
            CurrentCooldownT -= Time.deltaTime;
            if (CurrentCooldownT <= 0.0f)
            {
                IsCoolTime = false;
            }
        }
        else {
            CurrentCooldownT = CoolDownTime;
        }
    }
}
