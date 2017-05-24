using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNPC_HeadMovement : MonoBehaviour
{
    Rigidbody FlyNPC_Head;
    Transform PlayerT;
    Transform Home;
    Transform Target;
    Transform Propeller;
    CollisionIndicator CollHitBox, DeathHitBox;

    public bool AbNormalSpin;
    public float HLookRotSpeed, IdleRotSpeed, HStunnedRotSpeed, HomeDistance = 5.0f, AttakSpeed, IdleSpeed, CoolDownTime;
    public bool lookInactive, Attack, IsCoolTime, CanLookAtPlayer, AmStuck, StartTurning1, StartTurning2, IsDead, isMoving, PlayerHasIntruded;
    Vector3 FinalVel, FinalHeight, AntiWall, OriginalPos;

    Quaternion HLook, VLook, FHorizLook, FVertLook,OriginalRot;//, FinalLookRot;

    float CurrentRotationSpeed, PlayDistFromHome, NPCFDistFromHome, CurrentSpeed, groundDist, PropellerSpeed, CurrentCooldownT;
    float DownRayPosY;
    // Use this for initialization
    void Start()
    {
        FlyNPC_Head = this.gameObject.GetComponent<Rigidbody>();
        PlayerT = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        Home = this.transform.parent.gameObject.GetComponent<Transform>();
        Propeller = this.transform.GetChild(0).gameObject.GetComponent<Transform>();
        CollHitBox = Propeller.GetComponent<CollisionIndicator>();
        DeathHitBox = this.transform.GetChild(1).gameObject.GetComponent<CollisionIndicator>();

        OriginalPos = FlyNPC_Head.position;
        OriginalRot = FlyNPC_Head.rotation;

        CurrentCooldownT = CoolDownTime;

        isMoving = true;

    }

    // Update is called once per frame
    void Update()
    {

        HomeDistanceMeasurer();
        FloorMeasure();
        IsThereCoolDown();

        //Debug.Log(IsCoolTime);
        //This is for Sound
        if (PlayDistFromHome < HomeDistance) {
            PlayerHasIntruded = true;
        }
        if (PlayDistFromHome > HomeDistance && Attack == false) {
            PlayerHasIntruded = false;
        }
        //only for sound

        if (PlayDistFromHome <= HomeDistance && CanLookAtPlayer == true)
            Attack = true;

        if (((NPCFDistFromHome >= HomeDistance) && (PlayDistFromHome >= HomeDistance)) || IsCoolTime == true || CanLookAtPlayer == false)
            Attack = false;

        if (Attack == true)
        {
            Target = PlayerT;
            CurrentSpeed = AttakSpeed;
            CurrentRotationSpeed = HLookRotSpeed;
        }
        else
        {
            Target = Home;
            CurrentSpeed = IdleSpeed;
            CurrentRotationSpeed = IdleRotSpeed;
        }

        PropellerSpinner();
        LookingRotation();
        VelocitySetter();
        IsItDead();

        if (isMoving == true)
        {
            FlyNPC_Head.velocity = FinalVel;
            FlyNPC_Head.position = new Vector3(FlyNPC_Head.position.x, FinalHeight.y, FlyNPC_Head.position.z);
        }
    }
    public void AssignPlayer(GameObject p) {

        PlayerT = p.transform;
        FlyNPC_Head.position = OriginalPos;
        FlyNPC_Head.rotation = OriginalRot;

        IsDead = false;
        FlyNPC_Head.GetComponent<MeshRenderer>().enabled = true;
        FlyNPC_Head.GetComponent<Collider>().enabled = true;
        for (int j = 0; j < this.transform.childCount; j++)
        {
            this.transform.GetChild(j).gameObject.SetActive(true);
        }
        isMoving = true;
        lookInactive = false;

    }
    void LookingRotation()
    {
        if (lookInactive == false)
        {
            Vector3 TarGate = new Vector3(Target.position.x, 0.0f, Target.position.z) - new Vector3(FlyNPC_Head.position.x, 0.0f, FlyNPC_Head.position.z);
            if (TarGate.magnitude <= 0.5f)
            {
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
    void VelocitySetter()
    {
        float TargetPosY;
        float DecentSpeed;

        if (Target == PlayerT)
        {
            DecentSpeed = 1.5f;
            TargetPosY = PlayerT.position.y + 1.0f;
        }
        else
        {
            DecentSpeed = 0.5f;
            TargetPosY = Target.position.y;
        }

        if (groundDist <= 2.0f)
        {
            TargetPosY = -DownRayPosY + 2.0f;
            //Debug.Log("DragRacin!: " + DownRayPosY);
        }

        FinalVel = (FlyNPC_Head.rotation * Vector3.forward * CurrentSpeed);
        FinalHeight = Vector3.Lerp(new Vector3(0.0f, FlyNPC_Head.position.y, 0.0f), new Vector3(0.0f, TargetPosY, 0.0f), DecentSpeed * Time.deltaTime);
        //Debug.Log(FinalVel);
    }
    void HomeDistanceMeasurer()
    {
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
            else
            {
                CanLookAtPlayer = true;
            }
        }
        if (Physics.Raycast(FlyNPC_Head.position, Home.position - FlyNPC_Head.position, out Homehit))
        {
            if (Homehit.transform.tag != "PlayerMesh" && Attack == false)
            {
                if (Homehit.distance <= 3.0f)
                {
                    //Debug.Log("AmStuck");
                    AmStuck = true;
                    AntiWall = Homehit.normal;
                }
                else
                {
                    AmStuck = false;
                }
            }

        }
        if (Physics.Raycast(FlyNPC_Head.position, new Vector3(0.0f, -1.0f, 0.0f), out hit))
        {
            if (hit.transform.tag != "PlayerMesh")
            {
                groundDist = hit.distance;
                DownRayPosY = hit.transform.position.y;
            }
        }
    }
    void PropellerSpinner()
    {
        if (Attack == true)
        {
            PropellerSpeed = AttakSpeed * 300.0f;
        }
        else
        {
            PropellerSpeed = IdleSpeed * 300.0f;
        }

		if (IsDead == true) {
			PropellerSpeed = 0.0f;
		}
        if (AbNormalSpin)
            Propeller.RotateAround(Propeller.position, Propeller.forward, PropellerSpeed * Time.deltaTime);
        else
            Propeller.RotateAround(Propeller.position, Propeller.right, PropellerSpeed * Time.deltaTime);

    }
    void IsThereCoolDown()
    {
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
        else
        {
            CurrentCooldownT = CoolDownTime;
        }
    }
    void IsItDead() {
        if (DeathHitBox.HitsPlayer == true)
            IsDead = true;

        if (IsDead == true) {
            BeginDeathSequence();

            FlyNPC_Head.GetComponent<MeshRenderer>().enabled = false;
            FlyNPC_Head.GetComponent<Collider>().enabled = false;
            for (int j = 0; j < this.transform.childCount; j++) {
				if (this.transform.GetChild (j).gameObject.tag == "JumpCollider") {
					if (this.transform.GetChild (j).gameObject.GetComponent<CollisionIndicator> ().HitsPlayer == false) {
						this.transform.GetChild (j).gameObject.SetActive (false);
					}
				} else if (this.transform.GetChild (j).gameObject.tag == "EpicentralHazard") {
					if (this.transform.GetChild (j).gameObject.GetComponent<CollisionIndicator> ().HitsPlayer == false) {
						this.transform.GetChild (j).gameObject.SetActive (false);
					}
				} else {
					this.transform.GetChild (j).gameObject.SetActive (false);
				}
                    
				
            }
            isMoving = false;
            lookInactive = true;
            FlyNPC_Head.velocity = Vector3.zero;
        }
    }
    void BeginDeathSequence() {

    }
}
