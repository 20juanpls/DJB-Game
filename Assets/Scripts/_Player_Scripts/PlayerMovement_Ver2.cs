using UnityEngine;
using System.Collections;

public class PlayerMovement_Ver2 : MonoBehaviour {
    Rigidbody PlayerRb;
    GameObject RotatingParent;
    Transform Camera_Rot;
    PlayerKnockback KnockBack;
    PlayerHealth PlayHealth;
    PlayerNPCKill PlayNPCK;

    private float HorizLook, VertLook, ActualSpeed, UpHillValue, currentRotationSpeed;

    public bool Paused, UnPaused, DontMove, DontClimb, Freeze, forKnockBack, GroundCannotKill, SlideSequence, InRotatingPlat/*,ClimbSequence*/, QuickDeath;

    private bool isMove, JumpBack, JumpBackSeq, JumpSlide, /*HoldClimb,*/ ClimbBugPatch_1;
	public bool canJump, CantClimb, Sliding, Climbing;
    public bool hasJumped, JumpActiveButton, DJumpActive, isGrounded/*Do not erase yet...*/, IsGround_2;

    private Vector3 moveDirection = Vector3.zero/*, LastClimbDir*/;
    private Vector3 lookDirection = Vector3.zero, HitWallVector, JumpBackVect, CslideDownVect;

    private Vector3 rotatedDirection, FinalDirection, /*UseThis*/TheMovingPlaneVect, rtY, fallLenght, BottomPlatVel, moveforward;
	public Vector3 FinalVel,VelRelativeToPlay, ExForceVelocity, CurrentOldVel;

    private Quaternion _lookRotation, _PlaneRotation, PlayRot;

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

    //int HierchyNum;

    void Start () {

        PlayerRb = this.GetComponent<Rigidbody>();

        //HeierchyNumber(this.gameObject);
        //Debug.Log(HeierchyNumber(this.gameObject));
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

	void Update () {
        //Debug.Log(InRotatingPlat);
        if (PlayHealth.IsDead)
            InRotatingPlat = false;

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
            InRotationPlatform();

            CurrentOldVel = PlayerRb.velocity;
            GravityApplyer();

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
            if (/*isGrounded*/IsGround_2 == true /*|| ClimbSequence*/)
            {
                airTime = 0.0f;

                if (hasJumped == true)
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

            }
        }

    }

    void InRotationPlatform() {
        Debug.Log(InRotatingPlat);
        if (InRotatingPlat)
        {
            PlayerRb.transform.parent = RotatingParent.transform;
        }
        else
        {
            PlayerRb.transform.parent = null;
        }
    }

    void ControlOrientation()
    {
        //Creates a Vector3 that only has a Z of the magnitude of both the Input Axis --Noah
        float VectMeasure = moveDirection.magnitude;
        if (moveDirection.magnitude > 1.0f) {
            VectMeasure = 1.0f;
        }

        //This is so that the player does not abuse of jumping on the climb section
        if (JumpBackSeq == true && (JumpBackVect + (_lookRotation * Vector3.forward * VectMeasure)).magnitude <= 1.0f)
        {
            VectMeasure = 0.0f;
        }

        moveforward = new Vector3(0.0f, 0.0f, VectMeasure);
        rotatedDirection = new Vector3(moveforward.x, 0.0f, moveforward.z);
        
        /*if (CurrJumpBTime >= 0.2f && JumpBackSeq == true && (JumpBackVect.magnitude - moveDirection.magnitude) < 0.0f)
        {
            Debug.Log("ayy");
            VectMeasure = 0.0f;
            Debug.Log(VectMeasure);
        }*/

        //finds angle of camera relative to world & angle of surface
        float cameraRot = Camera_Rot.rotation.eulerAngles.y;
        //Debug.Log (surfaceAngle.eulerAngles);

        //if (!ClimbSequence)
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

        PlayRot = _lookRotation;

        if (/*ClimbSequence*/Climbing == true)
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

        if (DontMove == true || Freeze == true)
        {
            //PlayerRb.transform.rotation = Quaternion.Slerp(PlayerRb.transform.rotation, PlayRot, Time.deltaTime * rotationSpeed);
            currentRotationSpeed = 0.0f;
        }

        if (!DontMove && !SlideSequence && !Freeze)
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
            //ClimbSequence = true;
            //this is supposed to prevent the zelda botw swim up waterfall glitch
            if (vel.magnitude > (MoveSpeed / 2.0f)+0.5f) {
                ClimbBugPatch_1 = true;
            }
            else{
                ClimbBugPatch_1 = false;
            }
        }
        else
        {
            ActualSpeed = MoveSpeed;

            ClimbBugPatch_1 = false;
            /*if (ClimbSequence == true && !isGrounded)
            {
                HoldClimb = true;
            }

            if (JumpBack || KnockBack.InCollision || isGrounded)
            {
                HoldClimb = false;
                ClimbSequence = false;
            }*/
        }

        //Debug.DrawRay(PlayerRb.position, LastClimbDir*10.0f, Color.yellow);
        //Debug.DrawRay(PlayerRb.position, FinalDirection * 10.0f, Color.red);

            /*if (HoldClimb)
            {
                DontClimb = true;
                //Debug.Log(Vector3.Dot(FinalDirection, LastClimbDir));
                if (Vector3.Dot(FinalDirection, LastClimbDir) < 0)
                {
                    HoldClimb = false;
                    DontClimb = false;
                }
            }
            else {
                LastClimbDir = vel;
                DontClimb = false;
            }*/

        //Debug.Log("DontClimb: "+DontClimb);
        //Debug.Log("Climb Sequence: "+ClimbSequence);
        /*if (ClimbSequence)
        {
            ActualSpeed = MoveSpeed / 2.0f;
            currentGrav = 0.0f;
            if (JumpBack || KnockBack.InCollision || isGrounded)
            {
                HoldClimb = false;
                ClimbSequence = false;
            }
        }
        else {
            ActualSpeed = MoveSpeed;
        }*/


        Vector3 finalDirection = new Vector3(/*rotatedDirection.x*/TheMovingPlaneVect.x, TheMovingPlaneVect.y, TheMovingPlaneVect.z);
        _PlaneRotation = Quaternion.LookRotation(finalDirection);
        FinalDirection = _PlaneRotation * moveforward * ActualSpeed;

       //Debug.Log(FinalDirection.magnitude);
       //Debug.Log(PlayerRb.velocity.magnitude);


        if (CantClimb == true) {
            //Debug.Log("considerfalling");
            /*if (isGrounded == false)
            {
                IsGround_2 = false;
            }
            else {
                IsGround_2 = true;
            }*/
            FinalDirection = new Vector3(0.0f,FinalDirection.y,0.0f);
            Vector3 TPlayRot = _lookRotation * Vector3.forward;
            Vector3 TWallVect = new Vector3(HitWallVector.x, 0.0f, HitWallVector.z);

            float AngleDiff_T = Quaternion.FromToRotation(TWallVect, TPlayRot).eulerAngles.y;
            //Debug.DrawRay(PlayerRb.position, _lookRotation * Vector3.forward * 10.0f, Color.blue);
            if (!(AngleDiff_T < 45 || AngleDiff_T > 315) || ((HitWallVector.y > HitWallVector.x) && (HitWallVector.y > HitWallVector.z))) {
                //Debug.Log("Let Go ... ;(");
                FinalDirection = finalDirection * ActualSpeed;
            }
        }

        //Make A slippery slope that you can stand on if the surface angle is just rite...
        //Debug.Log((surfaceAngle.x*Mathf.Rad2Deg)+",0.0f,"+(surfaceAngle.z*Mathf.Rad2Deg));


        if (Sliding == true)
        {
            SlideSequence = true;
            isGrounded = true;
            //All of this normalizes the vector downward direction
            Vector3 momentaryVectXZ = new Vector3(-HitWallVector.x, 0.0f, -HitWallVector.z);
            Vector3 momentaryVectY = new Vector3(0.0f, HitWallVector.y, 0.0f);
            Quaternion AngleXZ = Quaternion.LookRotation(momentaryVectXZ);
            Vector3 FVectXZ = momentaryVectXZ.normalized * momentaryVectY.magnitude;
            Vector3 FVectY = momentaryVectY.normalized * momentaryVectXZ.magnitude;

            Vector3 SlideDownVect = (FVectXZ + FVectY) * MaxSlideSpeed;

            CslideDownVect = Vector3.Lerp(CslideDownVect,SlideDownVect,2.0f*Time.deltaTime); //- new Vector3(0.0f, HitWallVector.y,0.0f);

            if (Climbing || CantClimb) {
                CslideDownVect = Vector3.zero;
                SlideSequence = false;
            }
            
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
            //Debug.Log("Begin - BEKWARD JEMP sequence!!")
            //if (Climbing)
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

        if (!KnockBack.InCollision && !PlayNPCK.InCollider /*&& !ClimbSequence*/)
            currentGrav = setGrav;

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

        if (DontMove||DontClimb) {
            vel = new Vector3(KnockBack.FinalKnockBack.x, fallLenght.y, KnockBack.FinalKnockBack.z);
        }

        Debug.DrawRay(PlayerRb.position, vel*5.0f, Color.green);
        //ForMechanim
        VelRelativeToPlay = vel;

        FinalVel = vel + BottomPlatVel + ExForceVelocity;

        if (!Freeze)
        {
            PlayerRb.velocity = FinalVel;
        }
        else {
            PlayerRb.velocity = Vector3.zero;
        }

    }

    void JumpNow() {
        if (DontMove == false)
        {
            if (((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11")) && canJump == true) /*|| KnockBack.jumpedOn == true*/)
            {
                JumpActiveButton = true;
                if (!JumpSlide)
                    initialAirSpeed = JumpSpeed;


                if (/*ClimbSequence*/Climbing == true)
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
        if (ClimbBugPatch_1 == true)
        {
            initialAirSpeed = 0.0f;
        }

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
        foreach (ContactPoint contact in collision.contacts)
        {
            GameObject Other_GmObj = contact.otherCollider.gameObject;
            string Other_Tag = Other_GmObj.transform.tag;

            if (Other_Tag == "wall")
            {
                CantClimb = true;
                HitWallVector = -contact.normal;
            }

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

            if (FindParentWithTag(Other_GmObj, "ItRotates") != null) {
                RotatingParent = FindParentWithTag(Other_GmObj, "ItRotates");
                InRotatingPlat = true;
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        InRotatingPlat = false;
        IsGround_2 = false;
        Climbing = false;
        CantClimb = false;
        Sliding = false;
        surfaceAngle = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        BottomPlatVel = Vector3.zero;
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

}
