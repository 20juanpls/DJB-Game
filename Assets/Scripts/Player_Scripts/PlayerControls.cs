using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Rigidbody TransP;
	Transform Camera_Rot;
	RelativGrav JumpingC;
    GameObject punchHitBox;

	public float jumpSpeed = -10.0f;
    public float jumpSpeed_2 = -10.0f;
    public float moveSpeed = 0.5f;
    public float punchForce = 100.0f;
    //test
    private float punchCounter, OrigMoveSpeed;
    //end test
    public float rotationSpeed = 1.0f;
    private float forwardDist,downLedgeDist, oldforwardDist, forwardDistcounter;
    public int InitialmidAirJumpCount = 1;
    private float HorizLook, VertLook;

    private Vector3 moveDirection = Vector3.zero;
	private Vector3 lookDirection = Vector3.zero;
	private Vector3 rtY, TmD, /*FinalDirection,*/ForwardRotatedDirection, FallingDirection;
    public Vector3 rotatedDirection, FinalDirection;

    private Quaternion _lookRotation, surfaceAngle, templookRotation, surfaceAngleF,surfaceAngleD;

	private bool isGrounded, isMove, LedgeGrabbableF, LedgeGrabbableD, punchActive, hasLedgeGrabbed;
	public bool PlayerActiveMove , PlayerCanMove, CanMove, IWantToJump, hasJumped;
    private int CurrentMidAirJumpCount = 0;
	// Use this for initialization

	public GameObject theRunningGuy;
	Animation runner;


	void Start () {
        TransP = this.GetComponent<Rigidbody> ();
		Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        JumpingC = this.GetComponent<RelativGrav>();
        punchHitBox = this.gameObject.transform.GetChild(0).gameObject;
        isGrounded = false;
        isMove = false;
        CanMove = true;
        PlayerActiveMove = true;
        PlayerCanMove = true;
		runner = theRunningGuy.GetComponent<Animation> ();
        OrigMoveSpeed = moveSpeed;
        punchHitBox.SetActive(false);


    }
	void FixedUpdate ()
	{
		runner.Play ();

		ForwardMeasure();
        if (PlayerActiveMove == true)
        {
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

            //ForwardChecker();
            Punching();//player can punch
            MoveSpeedDecider();//Desides if distance from wall is suffecient enough to not move
            //LedgeGrab();// responsible for ledge grabbing(will only be activated on ledges and player cannot move on ledges)
            // if (/*Player*/CanMove == true)
            // {
            //if (CanMove == true) {
            //   rotatedDirection.x = 0.0f;
            //    rotatedDirection.y = 0.0f;
            //}
            ControlOrientation();//Orients the inputs to forward movement for player
                //ForwardMeasure();//Finds if there is anything in front of the player using raycast

            ApplyingDirection();//Applies direction to rotated forward vector
            //}
            JumpNow();//jump at any time...pls
            //oldforwardDist = forwardDist;<dont use this
        }
        //Physics.gravity = new Vector3(0.0f, -200.0f, 0.0f);

    }

	void ControlOrientation(){
		//Creates a Vector3 that only has a Z of the magnitude of both the Input Axis --Noah
		float VectMeasure = moveDirection.magnitude;
		Vector3 moveforward = new Vector3 (0.0f, 0.0f, VectMeasure);

		surfaceAngle = JumpingC.currentSurfaceAngle();
		//finds angle of camera relative to world & angle of surface
		float cameraRot = Camera_Rot.rotation.eulerAngles.y;
		//Debug.Log (surfaceAngle.eulerAngles);
		float EulerX = -surfaceAngle.eulerAngles.x;
		float EulerZ = -surfaceAngle.eulerAngles.z;

        Quaternion qx = Quaternion.AngleAxis(EulerX, Vector3.right);
		Quaternion qz = Quaternion.AngleAxis(EulerZ, Vector3.forward);
		Quaternion qy = Quaternion.AngleAxis(cameraRot, Vector3.up);
        Quaternion q = qx * qz * qy;

        //Added this so that the player stops moving with the camera if player doesn't give input
        if (isMove == false){
            rtY = qy * lookDirection;
        }

        TmD = q*moveDirection;

        _lookRotation = Quaternion.LookRotation (rtY);


		TransP.transform.rotation = Quaternion.Slerp (TransP.transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        // Check this
		rotatedDirection = new Vector3 (moveforward.x, 0.0f, moveforward.z);
        ForwardRotatedDirection = _lookRotation*Vector3.forward*2;
	}

    void ApplyingDirection() {
        Debug.DrawRay(Vector3.zero, JumpingC.fallLenght, Color.green);
        Debug.Log(isGrounded);

        //if (IWantToJump == true||isGrounded == false) {
            //Debug.Log("air is reached");
            FallingDirection = rotatedDirection * moveSpeed + JumpingC.fallLenght;
            TransP.transform.Translate(FallingDirection);
        //}
        /* if(isGrounded == true )
        {
            //Debug.Log("ground is reached");
            //DON'T MESS WITH THIS, THIS IS JUST TO SPABELIZE THE CHARACTER WHEN ITS GOING DOWNHILL!!!!
            if (TmD.y < 0)
            {
                TmD.y = TmD.y - 0.08f;
            }
            FinalDirection = new Vector3(rotatedDirection.x, TmD.y, rotatedDirection.z);

			TransP.transform.Translate(FinalDirection * moveSpeed);
            //TrzansP.AddRelativeForce(FinalDirection * moveSpeed);
            //Debug.Log(FinalDirection * moveSpeed);
        }*/

    }

    void ForwardMeasure() {
        RaycastHit hit;
        RaycastHit hit_2;
        //Debug.DrawRay(new Vector3((ForwardRotatedDirection.x) + TransP.transform.position.x,TransP.transform.position.y + 1.0f, (ForwardRotatedDirection.z) + TransP.transform.position.z), Vector3.down*2,Color.green);
        //Debug.DrawRay(TransP.transform.position, ForwardRotatedDirection, Color.red);
        //IMPORTANT: If I want to add a collider in front of player, I need to make sure the raycast ignores that collider...Tagging is key.
        if (Physics.Raycast(TransP.transform.position, ForwardRotatedDirection, out hit))
        {
			if (hit.transform.tag != "Coin") {
				forwardDist = hit.distance;
            //surfaceAngleF = Quaternion.FromToRotation(hit.normal, -ForwardRotatedDirection);<--NotYet
			}else{
				//Debug.Log("nothing hit :(");
				forwardDist = 0.0f;
			}
        }

        if (Physics.Raycast(new Vector3((ForwardRotatedDirection.x) + TransP.transform.position.x, TransP.transform.position.y + 1.0f, (ForwardRotatedDirection.z) + TransP.transform.position.z), Vector3.down * 2, out hit_2)) {
			if (hit_2.transform.tag != "Coin") {
				downLedgeDist = hit_2.distance;
				//surfaceAngleD = Quaternion.FromToRotation(hit.normal, -Vector3.down * 2);
			}
        }
    }

    void JumpNow() {
		if (Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11") && isGrounded == true)
        {
            JumpingC.setInitialSpeed(jumpSpeed, false);
            //IWantToJump = true;
            //isJumping = true;
            //TransP.AddForce(Vector3.up * jumpSpeed);
        }


        /*if ((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11")) && isGrounded == false && CurrentMidAirJumpCount > 0)
        {
            //Debug.Log("is this getting reached?");
            JumpingC.setFallAcceleration(0.0f);
            JumpingC.setCurrentFallSpeed(0.0f);
                JumpingC.setInitialSpeed(jumpSpeed_2, false);
            CurrentMidAirJumpCount--;
            IWantToJump = true;
        }

        if (JumpingC.airTime > 0.0f && isGrounded == false)
        {
            hasJumped = true;
        }
        /*else {
            hasJumped = false;
        }*/

        if (isGrounded == false){
            JumpingC.setFallAcceleration(JumpingC.fallAccel);
            JumpingC.setInitialSpeed(0.0f ,false);
            //FallingDirection = rotatedDirection * moveSpeed * 0.005f + JumpingC.fallLenght;
            //TransP.transform.Translate(FallingDirection);
        }
        //if (isGrounded == true) {
            //JumpingC.setCurrentFallSpeed(0.0f);
        //    JumpingC.setFallAcceleration(0.0f);
        //    JumpingC.setInitialSpeed(0.0f, true);
       // }

        /*if (isGrounded == true && hasJumped == true) {
            CurrentMidAirJumpCount = InitialmidAirJumpCount;
            IWantToJump = false;
            hasJumped = false;
            JumpingC.setInitialSpeed(0.0f, true);
            //isJumping = false;
        }*/
        isGrounded = JumpingC.IsItGrounded();
    }

   /* void LedgeGrab() {
        //Debug.Log(surfaceAngleF.eulerAngles.y);
        // bool Amistake = false;
        //Debug.Log(LedgeGrabbableF+","+ LedgeGrabbableD);
        Quaternion lDy = Quaternion.LookRotation(lookDirection, Vector3.up);
        Quaternion FDy = Quaternion.LookRotation(-ForwardRotatedDirection, Vector3.up);

        //Checks in front of ledge
        if (isGrounded == false && forwardDist <= 1.7f )//&& surfaceAngle.eulerAngles.y==0.0f)//surfaceAngleF.eulerAngles.y <= 10.0f && Amistake==false)
        {
            LedgeGrabbableF = true;
        }
        else {
            LedgeGrabbableF = false;
        }
        //Debug.Log(surfaceAngleD.eulerAngles.x);
        //Checks down towards the ledge
        if (isGrounded == false && downLedgeDist <= 1.5f && downLedgeDist >= 0.5f)// && surfaceAngle.eulerAngles.z == 0.0f)//surfaceAngleD.eulerAngles.z == 0.0f)
        {
            LedgeGrabbableD = true;
        }
        else
        {
            LedgeGrabbableD = false;
        }
		//Debug.Log (JumpingC.floorDist);
        if (LedgeGrabbableF == true && LedgeGrabbableD == true && JumpingC.floorDist > 4.0f)
        {
           // Debug.Log("ledge grabbed");
            JumpingC.setFallAcceleration(0.0f);
            JumpingC.setInitialSpeed(0.0f, true);
            PlayerCanMove = false;
            hasLedgeGrabbed = true;
            //if (Input.GetKey(KeyCode.S))
            //if (lDy.eulerAngles.y == FDy.eulerAngles.y - 5.0f )
            if (lDy.eulerAngles.y >= FDy.eulerAngles.y - 5.0f && lDy.eulerAngles.y <= FDy.eulerAngles.y + 5.0f)
            {
                PlayerCanMove = true;
                JumpingC.setFallAcceleration(JumpingC.fallAccel);
            }
            if (Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11"))
            {
                PlayerCanMove = true;
                JumpingC.setInitialSpeed(jumpSpeed, true);
                JumpingC.setFallAcceleration(JumpingC.fallAccel);
            }
        }
        else {
            JumpingC.setFallAcceleration(JumpingC.fallAccel);
            PlayerCanMove = true;
        }
        if(isGrounded == true) {
            hasLedgeGrabbed = false;
        }
    }*/
    public void setPlayerActivity(bool OnOrOff) {
        PlayerActiveMove = OnOrOff;
    }
    //Make sure that when the player cant move, means that the x and z vectors aren't allowed to move, but the y is!!!
    //v--- (12/6/16) Please Check this out ASAP!!!!!
    void MoveSpeedDecider() {
		//Debug.Log (moveSpeed);
		//Debug.Log (forwardDist);
		if (forwardDist < 1.4f)
        {
            CanMove = false;
            //PlayerCanMove = false;
            //moveSpeed = 0.0f;
        }
		if (VertLook <= -0.01 || forwardDist > 1.4f || forwardDist == 0.0f || hasLedgeGrabbed == true)
        {
            CanMove = true;
            //PlayerCanMove = true;
            //moveSpeed = OrigMoveSpeed;
        }

        if (CanMove == false)
        {
            moveSpeed = 0.0f;
        }
		else if (CanMove == true)
        {
            moveSpeed = OrigMoveSpeed;
        }
		//Debug.Log (CanMove);
	}
    //this checks if the forward dist remains trash for more than 3 frames, and sets the forwardist to zero;
    /*void ForwardChecker() {
        if (forwardDist == oldforwardDist)
        {
            forwardDistcounter++;
        }
        else
        {
            //forwardDistcounter = 0.0f;
            CanMove = true;
            //PlayerCanMove = true;
        }
        if (forwardDistcounter >= 3 && forwardDist < 1.1f)
        {
            //forwardDist = 0.0f;
			CanMove = true;
            //PlayerCanMove = true;
        }
    }*/
    void Punching() {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            //Debug.Log("is this a thing?");
            punchHitBox.SetActive(true);
            punchActive = true;
            //TransP.velocity = Vector3.zero;
        }

        if (punchActive == true) {
            //TransP.velocity = Vector3.zero;
            moveSpeed = punchForce;
            PlayerCanMove = false;
            punchCounter++;
        } else
        {
            punchCounter = 0.0f;
        }

        if (punchCounter >= 10.0f) {
            PlayerCanMove = true;
            moveSpeed = OrigMoveSpeed;
            TransP.velocity = Vector3.zero;
            punchHitBox.SetActive(false);
            punchActive = false;
        }
    }
}
