using UnityEngine;
using System.Collections;

public class PlayerMovement_Ver2 : MonoBehaviour {
    Rigidbody PlayerRb;
    Transform Camera_Rot;
    PlayerKnockback KnockBack;

    private float HorizLook, VertLook, ActualSpeed, UpHillValue, currentRotationSpeed;

    public bool Paused, UnPaused, DontMove, forKnockBack, GroundCannotKill;

    private bool isMove;
	public bool canJump, CantClimb, Sliding, Climbing;
    public bool hasJumped, isGrounded/*Do not erase yet...*/, IsGround_2;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero, HitWallVector;

    private Vector3 rotatedDirection, FinalDirection, /*UseThis*/TheMovingPlaneVect, rtY, fallLenght, BottomPlatVel;
	public Vector3 FinalVel,VelRelativeToPlay, ExForceVelocity, CurrentOldVel;

    private Quaternion _lookRotation;

    public Quaternion surfaceAngle, processedAngle;


    public float rotationSpeed = 20.0f;
    public float MoveSpeed = 10.0f;
    public float setGrav = 10.0f;
    public float JumpSpeed = 10.0f;
    public float currentfallSpeed;
    public float terminalSpeed = 10.0f;
	public float InitialMidAirJumpCount = 1.0f;
    public float AcceptedFloorDist = 1.7f;

    float currentFallAccel;
	private float forwardDist;
	float CurrentMidAirJumpCount;
    public float airTime, initialAirSpeed;

    public float floorDist;

    public GameObject theRunningGuy;

    void Start () {

        PlayerRb = this.GetComponent<Rigidbody>();
        Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        KnockBack = this.GetComponent<PlayerKnockback>();
        //runner = theRunningGuy.GetComponent<Animation>();
        ActualSpeed = MoveSpeed;
		hasJumped = false;
		CurrentMidAirJumpCount = InitialMidAirJumpCount;
        _lookRotation = PlayerRb.transform.rotation;
    }

    public void ActualSpeedSetter(float MoveSped) {
        ActualSpeed = MoveSped;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("is it grounded?: "+isGrounded);
        
        if (Paused == true)
        {
                //Debug.Log("IsPaused???");
                Debug.DrawRay(PlayerRb.position, PlayerRb.velocity, Color.green);
                PlayerRb.velocity = Vector3.zero;
                UnPaused = false;
        }
        else
        {
            if (UnPaused == false) {
                PlayerRb.velocity = CurrentOldVel;
                UnPaused = true;
            }
            CurrentOldVel = PlayerRb.velocity;
            //PlayerRb.velocity = CurrentOldVel;

            GravityApplyer();

            //Animator();

            FloorMeasure();
            IsGrounded();

            float HorizMov = Input.GetAxis("Horizontal");
            float VertMov = Input.GetAxis("Vertical");
            if (HorizMov != 0.00f || VertMov != 0.00f)
            {
                HorizLook = HorizMov;
                VertLook = VertMov;
                isMove = false;
            }
            else
            {
                isMove = true;
            }

            moveDirection = new Vector3(HorizMov, 0, VertMov);
            lookDirection = new Vector3(HorizLook, 0, VertLook);
            ControlOrientation();

            ApplyingDirection();

            JumpNow();

            //Debug.Log(isGrounded);

            //PlayerRb.velocity = vel;
            if (/*isGrounded*/IsGround_2 == true)
            {
                airTime = 0.0f;

                if (hasJumped == true)
                {
                    forKnockBack = true;
                    KnockBack.jumpedOn = false;
                    initialAirSpeed = 0.0f;
                    hasJumped = false;
                }
                else
                {
                    CurrentMidAirJumpCount = InitialMidAirJumpCount;
                    forKnockBack = false;
                }
                canJump = true;
            }
            else
            {
                airTime += Time.deltaTime;
                canJump = false;
            }
        }

        //Physics.gravity = new Vector3(0.0f, setGrav, 0.0f);
    }

    void ControlOrientation()
    {
        //Creates a Vector3 that only has a Z of the magnitude of both the Input Axis --Noah
        float VectMeasure = moveDirection.magnitude;
        Vector3 moveforward = new Vector3(0.0f, 0.0f, VectMeasure);
        rotatedDirection = new Vector3(moveforward.x, 0.0f, moveforward.z);

        //surfaceAngle = JumpingC.currentSurfaceAngle();

        //finds angle of camera relative to world & angle of surface
        float cameraRot = Camera_Rot.rotation.eulerAngles.y;
        //Debug.Log (surfaceAngle.eulerAngles);

        processedAngle = Quaternion.Inverse(surfaceAngle);//Quaternion.Euler(EulerX,180, EulerZ);

        Quaternion qy = Quaternion.AngleAxis(cameraRot, Vector3.up);

        //test
        Debug.DrawRay(PlayerRb.position, processedAngle * _lookRotation * Vector3.forward*moveDirection.magnitude, Color.red);

        TheMovingPlaneVect = processedAngle * _lookRotation * Vector3.forward * moveDirection.magnitude;
        //Added this so that the player stops moving with the camera if player doesn't give input
        if (isMove == false)
        {
            rtY = qy * lookDirection;
        }

        if (rtY.magnitude != 0.0f)
        {
            _lookRotation = Quaternion.LookRotation(rtY);
        }

        if (DontMove == true)
        {
            currentRotationSpeed = 0.0f;
        }
        else {
            currentRotationSpeed = rotationSpeed;
        }

        PlayerRb.transform.rotation = Quaternion.Slerp(PlayerRb.transform.rotation, _lookRotation, Time.deltaTime * currentRotationSpeed);


    }

    void ApplyingDirection()
    {
		Vector3 vel = PlayerRb.velocity;

		Vector3 finalDirection = new Vector3(/*rotatedDirection.x*/TheMovingPlaneVect.x, TheMovingPlaneVect.y, TheMovingPlaneVect.z);
        FinalDirection = /* _lookRotation */ finalDirection * ActualSpeed;

        if (/*(IsGround_2 == true && floorDist > 5.0f && Climbing == false)||*/ CantClimb == true) {
            //Debug.Log("considerfalling");
            IsGround_2 = false;
            FinalDirection = Vector3.zero;
            //Debug.Log(AngleHitWall);
            //Debug.Log(_lookRotation.y*Mathf.Rad2Deg);
            Vector3 TPlayRot = _lookRotation * Vector3.forward;
            Vector3 TWallVect = new Vector3(HitWallVector.x, 0.0f, HitWallVector.z);

            float AngleDiff_T = Quaternion.FromToRotation(TWallVect, TPlayRot).eulerAngles.y;

            Debug.DrawRay(PlayerRb.position, _lookRotation*Vector3.forward*10.0f, Color.blue);
            Debug.DrawRay(PlayerRb.position, new Vector3(HitWallVector.x,0.0f,HitWallVector.z) * 10.0f, Color.red);
            //Debug.Log(AngleDiff_T);

            if (!(AngleDiff_T < 45 || AngleDiff_T > 315)) {
                //Debug.Log("Let Go ... ;(");
                FinalDirection = finalDirection * ActualSpeed;
            }
        }

		//DrawRAY!!!!!!
        //Debug.DrawRay(PlayerRb.position, FinalDirection, Color.green);
        ///Debug.DrawRay(PlayerRb.position, PlayerRb.velocity, Color.blue);

        if (PlayerRb.velocity.magnitude <= 0.1f && airTime > 0.1f) {
            airTime = 0.0f;
            initialAirSpeed = 0.0f;
        }

        vel = new Vector3(FinalDirection.x, FinalDirection.y + fallLenght.y,FinalDirection.z);

        

        //ForMechanim
        VelRelativeToPlay = vel;
        //ForMechanim
        FinalVel = vel + BottomPlatVel + ExForceVelocity;

        // KnockBack Will move the player instead of the player Itself...
        if (DontMove == true)
        {
            FinalVel = new Vector3(KnockBack.FinalKnockBack.x, (UpHillValue * ActualSpeed * 1.05f) + fallLenght.y, KnockBack.FinalKnockBack.z);
        }

        PlayerRb.velocity = FinalVel;

    }

    void JumpNow() {
        if (DontMove == false)
        {
            if (((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 0")) && canJump == true)||KnockBack.jumpedOn == true)
            {
                initialAirSpeed = JumpSpeed;
            }

			if ((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 0")) && isGrounded == false && CurrentMidAirJumpCount > 0)
            {
                initialAirSpeed = JumpSpeed;
                airTime = 0.0f;
                CurrentMidAirJumpCount--;
            }
        }
        if (airTime > 0.0f)
        {
            hasJumped = true;
        }
    }

    void FloorMeasure()
    {
        RaycastHit hit;

        //Debug.DrawRay(new Vector3 (PlayerRb.position.x, PlayerRb.position.y-1.0f,PlayerRb.position.z), _lookRotation * Vector3.forward*10.0f, Color.red);
        //Debug.DrawRay(new Vector3(PlayerRb.position.x, PlayerRb.position.y + 1.0f, PlayerRb.position.z), _lookRotation * Vector3.forward * 10.0f, Color.yellow);

        if (Physics.Raycast(PlayerRb.position, new Vector3(0.0f,-1.0f,0.0f), out hit))
        {
			if (hit.transform.tag == "Untagged" || hit.transform.tag == "Kill" || hit.transform.tag == "StompNPC"/*|| hit.transform.tag == "NPC_charge"*/)
            {
                    floorDist = hit.distance;
                //surfaceAngle = Quaternion.FromToRotation(hit.normal, new Vector3(0.0f, -1.0f, 0.0f));
                /*if (hit.rigidbody && isGrounded == true)
                    BottomPlatVel = hit.rigidbody.velocity;
                else
                    BottomPlatVel = Vector3.zero;
                */
                if (hit.transform.tag != "Untagged" && hit.transform.tag != "StompNPC")
                {
                    GroundCannotKill = false;
                }
                else {
                    GroundCannotKill = true;
                }
            }
        }

    }

    void IsGrounded() {
        //Debug.Log(floorDist);
        if (floorDist <= AcceptedFloorDist && GroundCannotKill != false)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    void GravityApplyer() {

        currentfallSpeed = initialAirSpeed + (setGrav * airTime);

        fallLenght = Vector3.down * currentfallSpeed;

        if (currentfallSpeed >= terminalSpeed) {
            fallLenght.y = -terminalSpeed;
        }

    }

	void OnCollisionEnter(Collision collision){
		//Debug.Log (collision.relativeVelocity);
		/*if (collision.gameObject) {
		}	*/
	}


    //ThisIs a test
    //PLEASE CONSIDER THE POTENTIAL HERE!!!!!!
    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("hello?");
        /*if (collision.gameObject)
        {
            touching = true;
            Debug.Log(collision.gameObject.transform.tag);
        }
        else {
            touching = false;
        }*/
        //Debug.Log(collision.contacts.Length);

        
        foreach (ContactPoint contact in collision.contacts)
        {
            string Other_Tag = contact.otherCollider.gameObject.transform.tag;
            //Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
            if (Other_Tag == "wall")
            {
                //Debug.Log("We Need to build a wall");
                CantClimb = true;
                HitWallVector = -contact.normal;

            }
            else {
                CantClimb = false;
            }

            if (Other_Tag == "slide")
            {
                Sliding = true;
            }
            else {
                Sliding = false;
            }

            if (Other_Tag == "Untagged" || Other_Tag == "StompNPC" || Other_Tag == "climb")
            {
                IsGround_2 = true;

                if (contact.otherCollider.gameObject.GetComponent<Rigidbody>() != null)
                {
                    BottomPlatVel = contact.otherCollider.gameObject.GetComponent<Rigidbody>().velocity;
                }
                else {
                    BottomPlatVel = Vector3.zero;
                }

                if (Other_Tag == "climb")
                {
                    Climbing = true;
                }
                else
                {
                    Climbing = false;
                }
            }

            if (Other_Tag != "wall")
                surfaceAngle = Quaternion.FromToRotation(contact.normal, new Vector3(0.0f, 1.0f, 0.0f));
        }
    }

    void OnCollisionExit(Collision collision)
    {
        /*if (collision.gameObject)
        {
        }*/

            IsGround_2 = false;
            surfaceAngle = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

}
