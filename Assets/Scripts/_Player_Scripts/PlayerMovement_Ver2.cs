using UnityEngine;
using System.Collections;

public class PlayerMovement_Ver2 : MonoBehaviour {
    Rigidbody PlayerRb;
    Transform Camera_Rot;
    PlayerKnockback KnockBack;
    PlayerHealth PlayHealth;
    PlayerNPCKill PlayNPCK;

    private float HorizLook, VertLook, ActualSpeed, UpHillValue, currentRotationSpeed;

    public bool Paused, UnPaused, DontMove, forKnockBack, GroundCannotKill, InstaJamp, SlideSequence, QuickDeath;

    private bool isMove, JumpBack, JumpBackSeq, JumpSlide;
	public bool canJump, CantClimb, Sliding, Climbing;
    public bool hasJumped, JumpActiveButton, DJumpActive, isGrounded/*Do not erase yet...*/, IsGround_2;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero, HitWallVector, JumpBackVect, CslideDownVect;

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
    public float MaxSlideSpeed = 15.0f;

    float currentFallAccel;
	private float forwardDist, CurrJumpBTime, currentGrav;
	float CurrentMidAirJumpCount;
    public float airTime, initialAirSpeed, JumpBackTime;

    public float floorDist;

    public GameObject theRunningGuy;

	public bool jumpOnEnemy = false;

    void Start () {

        PlayerRb = this.GetComponent<Rigidbody>();
        Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        KnockBack = this.GetComponent<PlayerKnockback>();
        PlayHealth = this.GetComponent<PlayerHealth>();
        PlayNPCK = this.GetComponent<PlayerNPCKill>();
        //runner = theRunningGuy.GetComponent<Animation>();
        ActualSpeed = MoveSpeed;
		hasJumped = false;
		CurrentMidAirJumpCount = InitialMidAirJumpCount;
        CurrJumpBTime = JumpBackTime;
        _lookRotation = PlayerRb.transform.rotation;
        currentGrav = setGrav;
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
                //Debug.DrawRay(PlayerRb.position, PlayerRb.velocity, Color.green);
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



            if (PlayHealth.currentHealth== 0.0f) {
                moveDirection = Vector3.zero;
                lookDirection = Vector3.zero;
            }

            ControlOrientation();

            ApplyingDirection();

            JumpNow();

            //Debug.Log(isGrounded);

            //PlayerRb.velocity = vel;
            if (/*isGrounded*/IsGround_2 == true)
            {
                airTime = 0.0f;

                if (hasJumped == true && InstaJamp == true)
                {
                    forKnockBack = true;
                    //KnockBack.jumpedOn = false;
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
                InstaJamp = true;
            }
        }

    }

    void ControlOrientation()
    {
        //Creates a Vector3 that only has a Z of the magnitude of both the Input Axis --Noah
        float VectMeasure = moveDirection.magnitude;
        Vector3 moveforward = new Vector3(0.0f, 0.0f, VectMeasure);
        rotatedDirection = new Vector3(moveforward.x, 0.0f, moveforward.z);

        //surfaceAngle = JumpingC.currentSurfaceAngle();
        //Debug.Log((JumpBackVect + (_lookRotation*Vector3.forward*VectMeasure)).magnitude);
        //Debug.DrawRay(PlayerRb.position, JumpBackVect * 2.0f, Color.red);
        //Debug.DrawRay(PlayerRb.position, _lookRotation * Vector3.forward, Color.blue);
        
        //This is so that the player does not abuse of jumping on the climb section
        if (JumpBackSeq == true && (JumpBackVect + (_lookRotation * Vector3.forward * VectMeasure)).magnitude <= 1.0f) {
            VectMeasure = 0.0f;
        }
        /*if (CurrJumpBTime >= 0.2f && JumpBackSeq == true && (JumpBackVect.magnitude - moveDirection.magnitude) < 0.0f)
        {
            Debug.Log("ayy");
            VectMeasure = 0.0f;
            Debug.Log(VectMeasure);
        }*/

        //finds angle of camera relative to world & angle of surface
        float cameraRot = Camera_Rot.rotation.eulerAngles.y;
        //Debug.Log (surfaceAngle.eulerAngles);

        processedAngle = Quaternion.Inverse(surfaceAngle);//Quaternion.Euler(EulerX,180, EulerZ);

        Quaternion qy = Quaternion.AngleAxis(cameraRot, Vector3.up);

        //test
        //Debug.DrawRay(PlayerRb.position, processedAngle * _lookRotation * Vector3.forward*VectMeasure, Color.red);

        TheMovingPlaneVect = processedAngle * _lookRotation * Vector3.forward * VectMeasure;
        
        //Added this so that the player stops moving with the camera if player doesn't give input
        if (isMove == false)
        {
            rtY = qy * lookDirection;
        }

        if (rtY.magnitude != 0.0f)
        {
            _lookRotation = Quaternion.LookRotation(rtY);
        }

        Quaternion PlayRot = _lookRotation;

        if (Climbing == true)
        {
            PlayRot = Quaternion.LookRotation(new Vector3(HitWallVector.x,0.0f,HitWallVector.z));
        }

        if (KnockBack.collided == true) {
            PlayRot = KnockBack.hitRotation;
        }
        if (SlideSequence == true) {
            PlayRot = Quaternion.LookRotation(new Vector3(CslideDownVect.x, 0.0f, CslideDownVect.z));
            currentRotationSpeed = rotationSpeed * 0.3f;
        }

        if (DontMove == true)
        {
            //PlayerRb.transform.rotation = Quaternion.Slerp(PlayerRb.transform.rotation, PlayRot, Time.deltaTime * rotationSpeed);
            currentRotationSpeed = 0.0f;
        }

        if (!DontMove && !SlideSequence)
        {
            currentRotationSpeed = rotationSpeed;
        }
        //else
        //{
            PlayerRb.transform.rotation = Quaternion.Slerp(PlayerRb.transform.rotation, PlayRot, Time.deltaTime * currentRotationSpeed);
        //}

        //Debug.DrawRay(PlayerRb.position, PlayRot * Vector3.forward * 2.0f, Color.yellow);


    }

    void ApplyingDirection()
    {
        Vector3 vel = PlayerRb.velocity;

        if (Climbing == true)
        {
            ActualSpeed = MoveSpeed / 2.0f;
        }
        else
        {
            ActualSpeed = MoveSpeed;
        }
        Vector3 finalDirection = new Vector3(/*rotatedDirection.x*/TheMovingPlaneVect.x, TheMovingPlaneVect.y, TheMovingPlaneVect.z);
        FinalDirection = /* _lookRotation */ finalDirection * ActualSpeed;


        if (CantClimb == true) {
            //Debug.Log("considerfalling");
            IsGround_2 = false;
            FinalDirection = Vector3.zero;
            Vector3 TPlayRot = _lookRotation * Vector3.forward;
            Vector3 TWallVect = new Vector3(HitWallVector.x, 0.0f, HitWallVector.z);

            float AngleDiff_T = Quaternion.FromToRotation(TWallVect, TPlayRot).eulerAngles.y;
            //Debug.DrawRay(PlayerRb.position, _lookRotation * Vector3.forward * 10.0f, Color.blue);
            if (!(AngleDiff_T < 45 || AngleDiff_T > 315) || ((HitWallVector.y > HitWallVector.x) && (HitWallVector.y > HitWallVector.z))) {
                //Debug.Log("Let Go ... ;(");
                FinalDirection = finalDirection * ActualSpeed;
            }
        }

        if (Sliding == true)
        {
            SlideSequence = true;
            //All of this normalizes the vector downward direction
            Vector3 momentaryVectXZ = new Vector3(-HitWallVector.x, 0.0f, -HitWallVector.z);
            Vector3 momentaryVectY = new Vector3(0.0f, HitWallVector.y, 0.0f);
            Quaternion AngleXZ = Quaternion.LookRotation(momentaryVectXZ);
            Vector3 FVectXZ = momentaryVectXZ.normalized * momentaryVectY.magnitude;
            Vector3 FVectY = momentaryVectY.normalized * momentaryVectXZ.magnitude;

            Vector3 SlideDownVect = (FVectXZ + FVectY) * MaxSlideSpeed;

            CslideDownVect = Vector3.Lerp(CslideDownVect,SlideDownVect,2.0f*Time.deltaTime); //- new Vector3(0.0f, HitWallVector.y,0.0f);
            //Quaternion downAng = processedAngle;
            //Debug.DrawRay(PlayerRb.position, CslideDownVect, Color.yellow);
            // Debug.DrawRay(PlayerRb.position, ayyddabs*Vector3.right * 10.0f, Color.blue);
        }
        else {
            CslideDownVect = Vector3.Lerp(CslideDownVect, Vector3.zero, 2.0f * Time.deltaTime);
        }

        if (!SlideSequence) {
            JumpSlide = false;
        }

        //Decides to jump bek or nah
        if (JumpBack == true)
        {
            //Debug.Log("Begin - BEKWARD JEMP sequence!!");
            //Debug.DrawRay(PlayerRb.position, -HitWallVector, Color.green);
            JumpBackVect = new Vector3(-HitWallVector.x, 0.0f, -HitWallVector.z);
            JumpBackSeq = true;
        }

        if (JumpBackSeq == true)
        {
            CurrJumpBTime -= Time.deltaTime;
            ExForceVelocity = CurrJumpBTime*JumpBackVect * 15.0f;

            if (CurrJumpBTime <= 0.0f)
            {
                CurrJumpBTime = JumpBackTime;
                JumpBackSeq = false;
                ExForceVelocity = Vector3.zero;
            }
        }

        //Debug.Log(PlayNPCK.InCollider);
        if (PlayNPCK.InCollider == true)
        {
            currentGrav = 0.0f;
            airTime = 0.0f;
            if (IsGround_2 == true|| initialAirSpeed==0.0f) {
                initialAirSpeed = JumpSpeed;
            }
        }
        else {
            currentGrav = setGrav;
        }

        if (KnockBack.InCollision == true)
        {
            currentGrav = 0.0f;
            airTime = 0.0f;
            initialAirSpeed = KnockBack.KnockBackJumpForce;
            if (IsGround_2 == true || initialAirSpeed == 0.0f)
            {
                initialAirSpeed = KnockBack.KnockBackJumpForce;
            }
        }
        else
        {
            currentGrav = setGrav;
        }

        //Important: this is so the momentum doesn't gather up when close to ledges...
        if (PlayerRb.velocity.magnitude <= 0.1f && airTime > 0.1f)
        {
            airTime = 0.0f;
            initialAirSpeed = 0.0f;
            //Debug.Log("ImStuck!!!"); potentially create a function where the player is slightly pushed back away
            //from the edge, just make sure it doesn't do that on walls...
        }
        //Test
        //Vector3 CurrFinalVel = Vector3.Lerp(new Vector3(PlayerRb.velocity.x, 0.0f, PlayerRb.velocity.z),
        //    new Vector3(FinalDirection.x,0.0f,FinalDirection.z), 30.0f*Time.deltaTime);
        //pre-vel
        vel = new Vector3(FinalDirection.x, FinalDirection.y + fallLenght.y,FinalDirection.z) + CslideDownVect;

        if (DontMove == true) {
            vel = new Vector3(KnockBack.FinalKnockBack.x, fallLenght.y, KnockBack.FinalKnockBack.z);
        }

        //Debug.DrawRay(PlayerRb.position, CurrFinalVel*5.0f, Color.green);
        //ForMechanim
        VelRelativeToPlay = vel;

        FinalVel = vel + BottomPlatVel + ExForceVelocity;

        PlayerRb.velocity = FinalVel;

    }

	/*void OnTriggerEnter(Collider other)
	{
		if (other.tag == "JumpCollider")
		{
            Debug.Log("InCollider");
		}
		
	}
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "JumpCollider")
        {
            Debug.Log("OutOfCollider");
        }
    }*/

    void JumpNow() {
        if (DontMove == false)
        {
            if (((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11")) && canJump == true) /*|| KnockBack.jumpedOn == true*/)
            {
                JumpActiveButton = true;
                if (!JumpSlide)
                    initialAirSpeed = JumpSpeed;


                if (Climbing == true)
                {
                    JumpBack = true;
                }
                if (SlideSequence == true)
                {
                    JumpSlide = true;
                }
            }
            else {
                JumpActiveButton = false;
            }

            if ((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11")) && IsGround_2 == false && CurrentMidAirJumpCount > 0)
            {
                DJumpActive = true;
                initialAirSpeed = JumpSpeed;
                airTime = 0.0f;
                CurrentMidAirJumpCount--;
            }
            else {
                DJumpActive = false;
            }

            //if (hasJumped == true)
        }
        if (airTime > 0.0f)
        {
            hasJumped = true;
            JumpBack = false;
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
            if (hit.transform.tag == "hazard" || hit.transform.tag == "EpicentralHazard")
            {
                QuickDeath = true;
            }
            else {
                QuickDeath = false;
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

        currentfallSpeed = initialAirSpeed + (currentGrav * airTime);

        fallLenght = Vector3.down * currentfallSpeed;

        if (currentfallSpeed >= terminalSpeed) {
            fallLenght.y = -terminalSpeed;
        }

        /*
        if (Sliding) {
            SlidingTime += Time.deltaTime;
            currentSlidingSpeed = 
        }*/

    }

	void OnCollisionEnter(Collision collision){
		//Debug.Log (collision.relativeVelocity);
		/*if (collision.gameObject) {
		}	*/
	}

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
            //else {
            //    CantClimb = false;
            //}

            if (Other_Tag == "slide")
            {
                Sliding = true;
                HitWallVector = -contact.normal;
            }

            if (Other_Tag != "slide") {
                SlideSequence = false;
            }

            if (Other_Tag == "Untagged" || Other_Tag == "StompNPC" || Other_Tag == "climb"||Other_Tag == "slide")
            {
                IsGround_2 = true;

				if (Other_Tag == "Untagged") {
					Climbing = false;
					//IsGround_2 = true;
					Sliding = false;
				}
                else if (contact.otherCollider.gameObject.GetComponent<Rigidbody>() != null)
                {
                    BottomPlatVel = contact.otherCollider.gameObject.GetComponent<Rigidbody>().velocity;
                }

                if (Other_Tag == "climb")
                {
                    Climbing = true;
                    HitWallVector = -contact.normal;
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
            Climbing = false;
            CantClimb = false;
            Sliding = false;
            surfaceAngle = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            BottomPlatVel = Vector3.zero;
    }

}
