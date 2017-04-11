using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Transform TransP;
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

    private Quaternion _lookRotation, surfaceAngle, surfaceAngleF,surfaceAngleD;

	private bool isGrounded, isMove, LedgeGrabbableF, LedgeGrabbableD, punchActive, hasLedgeGrabbed, saveFlag;
	public bool PlayerActiveMove , PlayerCanMove, CanMove, IWantToBelieve, hasJumped, isJumping;
    private int CurrentMidAirJumpCount = 0;
    static Quaternion templookRotation;
	// Use this for initialization

	public GameObject theRunningGuy;
	Animation runner;


	void Start () {
		TransP = this.GetComponent<Transform> ();
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
		JumpingC.setCurrentFallSpeed(0.0f);


		//FOUND MAJOR THREAT... USING RIGIDBOD WITHOUD PROGRAMER'S PREMISSION!!!!
		//Debug.Log (TransP.velocity);
		//TransP.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		//FOUND MAJOR THREAT...ETC
        

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
            //Punching();//player can punch
            //MoveSpeedDecider();//Desides if distance from wall is suffecient enough to not move
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
		rotatedDirection = new Vector3 (moveforward.x, 0.0f, moveforward.z);

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
		//

        ForwardRotatedDirection = _lookRotation*Vector3.forward*2;
	}

    void ApplyingDirection() {
		Debug.DrawRay(Vector3.zero, FallingDirection, Color.green);
        //Debug.Log(isGrounded);
        //FallingDirection = rotatedDirection * moveSpeed + JumpingC.fallLenght;

		//does not affect glitch
        if (isGrounded == true)
        {
            //Debug.Log("ground is reached");
            //DON'T MESS WITH THIS, THIS IS JUST TO SPABELIZE THE CHARACTER WHEN ITS GOING DOWNHILL!!!!
            if (TmD.y < 0)
            {
                TmD.y = TmD.y - 0.08f;
            }

            if (IWantToBelieve == true)
            {
                FinalDirection = new Vector3(rotatedDirection.x * moveSpeed, 0.0f, rotatedDirection.z * moveSpeed) + JumpingC.fallLenght;
            }
            else
            {
                FinalDirection = new Vector3(rotatedDirection.x * moveSpeed, TmD.y*moveSpeed, rotatedDirection.z * moveSpeed);
            }
            FallingDirection = FinalDirection;
            //TrzansP.AddRelativeForce(FinalDirection * moveSpeed);
            //Debug.Log(FinalDirection * moveSpeed);
        }
        else {
            FallingDirection = new Vector3(rotatedDirection.x*moveSpeed, 0.0f, rotatedDirection.z*moveSpeed) + JumpingC.fallLenght;
        }
        //FallingDirection = FinalDirection * moveSpeed;
        TransP.transform.Translate(FallingDirection);


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
        //Debug.Log("Do I believe?:" + IWantToBelieve);
		if (Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11") && isGrounded == true)
        {
			//NOT CALLING INIT SPEED IN GLITCH
            JumpingC.setInitialSpeed(jumpSpeed, false);
            IWantToBelieve= true;
            //isJumping = true;
            //hasJumped = true;
            //TransP.AddForce(Vector3.up * jumpSpeed);
        }


        if ((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11")) && isGrounded == false && CurrentMidAirJumpCount > 0)
        {
            Debug.Log("is this getting reached?");
            IWantToBelieve = true;
            JumpingC.setFallAcceleration(0.0f);
            //JumpingC.setCurrentFallSpeed(0.0f);
                //JumpingC.setInitialSpeed(jumpSpeed_2, false);
            CurrentMidAirJumpCount--;
        }

        /*if (JumpingC.airTime > 0.0f && isGrounded == false)
        {
            hasJumped = true;
        }*/
        /*else {
            hasJumped = false;
        }*/


		//POTENTIAL THREAT
        if (isGrounded == false){
            JumpingC.setFallAcceleration(JumpingC.fallAccel);
			//POTENTIAL THREAT 
            //JumpingC.setInitialSpeed(0.0f ,false);
			
		    IWantToBelieve = false;
            //FallingDirection = rotatedDirection * moveSpeed * 0.005f + JumpingC.fallLenght;
            //TransP.transform.Translate(FallingDirection);
        }
		//POTENTIAL THREAT


        //if (isGrounded == true) {
            //JumpingC.setCurrentFallSpeed(0.0f);
        //    JumpingC.setFallAcceleration(0.0f);
        //    JumpingC.setInitialSpeed(0.0f, true);
       // }
		if (isGrounded == true){
			CurrentMidAirJumpCount = InitialmidAirJumpCount;
			//IWantToBelieve = false;
		}

        /*if (isGrounded == true && hasJumped == true) {
            CurrentMidAirJumpCount = InitialmidAirJumpCount;
            IWantToJump = false;
            hasJumped = false;
            JumpingC.setInitialSpeed(0.0f, true);
            //isJumping = false;
        }*/
        isGrounded = JumpingC.IsItGrounded();
    }
	
    public void setPlayerActivity(bool OnOrOff) {
        PlayerActiveMove = OnOrOff;
    }
    //Make sure that when the player cant move, means that the x and z vectors aren't allowed to move, but the y is!!!
    //v--- (12/6/16) Please Check this out ASAP!!!!!
    void MoveSpeedDecider() {
        Vector3 thisDown = new Vector3(TransP.transform.position.x, -1.0f, TransP.transform.position.z);
        Vector3 downward = surfaceAngle* thisDown;
        float AngleDiff = Vector3.Angle(thisDown, downward);
        float AngleDiff2 = templookRotation.eulerAngles.y - TransP.transform.rotation.eulerAngles.y;
        //Debug.Log (AngleDiff2);
        /*if (forwardDist < 1.4f)
        {
            CanMove = false;
            //saveFlag = true;
            //Debug.Log(templookRotation.eulerAngles.y - TransP.transform.rotation.eulerAngles.y);
            //templookRotation = TransP.transform.rotation;
            //Debug.Log(templookRotation.eulerAngles.y);
            //PlayerCanMove = false;
            //moveSpeed = 0.0f;
        }*/
       // else {
        //    templookRotation = TransP.transform.rotation;
        //}

		/*if (AngleDiff >= 55.0f&&isGrounded == true) {
            CanMove = false;
            Debug.Log("Too Steep!");
            //JumpingC.setFallAcceleration(0.0f);
        }*/
        /*if (-AngleDiff2 < 45.0f && -AngleDiff2 >0.0f|| -AngleDiff2 > 315.0f&& -AngleDiff2< 360.0f||AngleDiff2 < 45.0f && AngleDiff2 > 0.0f || AngleDiff2 > 315.0f && AngleDiff2 < 360.0f||forwardDist == 0.0f) {
           CanMove = true;
        }*/
        /*if (VertLook <= -0.01 || forwardDist > 1.4f || forwardDist == 0.0f /*||AngleDiff <= 55.0f)
        {
            CanMove = true;
            //PlayerCanMove = true;
            //moveSpeed = OrigMoveSpeed;
        }*/
        //if (saveFlag == true) {
        //    templookRotation = TransP.transform.rotation;
        //}

        if (CanMove == false)
        {
            moveSpeed = 0.0f;
           // saveFlag = false;
            //templookRotation = TransP.transform.rotation;
        }
		else if (CanMove == true)
        {
            moveSpeed = OrigMoveSpeed;
            templookRotation = TransP.transform.rotation;
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
            //TransP.velocity = Vector3.zero;
            punchHitBox.SetActive(false);
            punchActive = false;
        }
    }
}
