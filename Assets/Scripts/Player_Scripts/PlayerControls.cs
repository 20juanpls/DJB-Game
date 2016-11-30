using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Rigidbody TransP;
	Transform Camera_Rot;
	RelativGrav JumpingC;

	public float jumpSpeed = -10.0f;
    public float moveSpeed = 0.5f;
    //test
    private float OrigMoveSpeed;
    //end test
    public float rotationSpeed = 1.0f;
    private float forwardDist,downLedgeDist;
    public int InitialmidAirJumpCount = 1;
    private float HorizLook, VertLook;

    private Vector3 moveDirection = Vector3.zero;
	private Vector3 lookDirection = Vector3.zero;
	private Vector3 rotatedDirection, rtY, TmD, FinalDirection,ForwardRotatedDirection;

    private Quaternion _lookRotation, surfaceAngle, templookRotation, surfaceAngleF,surfaceAngleD;

    private bool isGrounded, isMove, CanMove, LedgeGrabbableF, LedgeGrabbableD, hasLedgeGrabbed;
    public bool PlayerActiveMove, PlayerCanMove;
    private int CurrentMidAirJumpCount = 0;
	// Use this for initialization

	public GameObject theRunningGuy;
	Animation runner;


	void Start () {
        TransP = this.GetComponent<Rigidbody> ();
		Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        JumpingC = this.GetComponent<RelativGrav>();
        isGrounded = false;
        isMove = false;
        CanMove = true;
        PlayerActiveMove = true;
        PlayerCanMove = true;
		runner = theRunningGuy.GetComponent<Animation> ();
        OrigMoveSpeed = moveSpeed;


    }

	void Update () 
	{
		runner.Play ();
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

            if (PlayerCanMove == true)
            {
                ControlOrientation();//Orients the inputs to forward movement for player
                //ForwardMeasure();//Finds if there is anything in front of the player using raycast
                MoveSpeedDecider();//Desides if distance from wall is suffecient enough to not move
                ApplyingDirection();//Applies direction to rotated forward vector
            }
			ForwardMeasure();
            LedgeGrab();// responsible for ledge grabbing(will only be activated on ledges and player cannot move on ledges)
            JumpNow();//jump at any time...pls
        }
	
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
        if (isGrounded == true)
        {
            //DON'T MESS WITH THIS, THIS IS JUST TO SPABELIZE THE CHARACTER WHEN ITS GOING DOWNHILL!!!!
            if (TmD.y < 0)
            {
                TmD.y = TmD.y - 0.08f;
            }
            FinalDirection = new Vector3(rotatedDirection.x, TmD.y, rotatedDirection.z);

            TransP.transform.Translate(FinalDirection * moveSpeed);
        }
        else
        {
            TransP.transform.Translate(rotatedDirection * moveSpeed);
        }
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
		if (Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 11")){
            JumpingC.setInitialSpeed(jumpSpeed, false);
        }

        if (Input.GetKeyDown("space") && isGrounded == false && CurrentMidAirJumpCount >= 0)
        {
            JumpingC.setCurrentFallSpeed(0.0f);
                JumpingC.setInitialSpeed(jumpSpeed, false);
            CurrentMidAirJumpCount--;
        }

        if (isGrounded == false){
            JumpingC.setInitialSpeed(0.0f ,false);
        }

        if (isGrounded == true) {
            CurrentMidAirJumpCount = InitialmidAirJumpCount;
        }
        isGrounded = JumpingC.IsItGrounded();
    }

    void LedgeGrab() {
        //Debug.Log(surfaceAngleF.eulerAngles.y);
        // bool Amistake = false;
        //Debug.Log(LedgeGrabbableF+","+ LedgeGrabbableD);
        Quaternion lDy = Quaternion.LookRotation(lookDirection, Vector3.up);
        Quaternion FDy = Quaternion.LookRotation(-ForwardRotatedDirection, Vector3.up);

            /*if (surfaceAngleF.eulerAngles.y >= 30.0f && surfaceAngleF.eulerAngles.y <= 330.0f)
                {
                    Amistake = true;
                }
            else {
                    Amistake = false;
                }*/
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
        if (isGrounded == false && downLedgeDist <= 1.0f && downLedgeDist >= 0.8f)// && surfaceAngle.eulerAngles.z == 0.0f)//surfaceAngleD.eulerAngles.z == 0.0f)
        {
            LedgeGrabbableD = true;
        }
        else
        {
            LedgeGrabbableD = false;
        }

        if (LedgeGrabbableF == true && LedgeGrabbableD == true && JumpingC.floorDist > 4.0f)
        {
            //Debug.Log("ledge grabbed");
            JumpingC.setFallAcceleration(0.0f);
            JumpingC.setInitialSpeed(0.0f, true);
            PlayerCanMove = false;
            hasLedgeGrabbed = true;
            //if (Input.GetKey(KeyCode.S))
            //if (lDy.eulerAngles.y == FDy.eulerAngles.y - 5.0f )
            if (lDy.eulerAngles.y >= FDy.eulerAngles.y-5.0f&& lDy.eulerAngles.y <= FDy.eulerAngles.y + 5.0f)
            {
                PlayerCanMove = true;
                JumpingC.setFallAcceleration(JumpingC.fallAccel);
            }
            if (Input.GetKeyDown("space"))
            {
                PlayerCanMove = true;
                JumpingC.setInitialSpeed(jumpSpeed, true);
                JumpingC.setFallAcceleration(JumpingC.fallAccel);
            }
        }
        if(isGrounded == true) {
            hasLedgeGrabbed = false;
        }
    }

    public void setPlayerActivity(bool OnOrOff) {
        PlayerActiveMove = OnOrOff;
    }
    void MoveSpeedDecider() {
		//Debug.Log (forwardDist);
        if (forwardDist <= 1.1f)
        {
            CanMove = false;
        }
        if (VertLook <= -0.01 || forwardDist > 1.1f || forwardDist == 0.0f || hasLedgeGrabbed == true)
        {
            CanMove = true;
        }

        if (CanMove == false)
        {
            moveSpeed = 0.0f;
        }
        else
        {
            moveSpeed = OrigMoveSpeed;
        }
    }
}
