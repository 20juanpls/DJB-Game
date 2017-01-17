using UnityEngine;
using System.Collections;

public class PlayerMovement_Ver2 : MonoBehaviour {
    Rigidbody PlayerRb;
    Transform Camera_Rot;
    PlayerKnockback KnockBack;

    private float HorizLook, VertLook, /*floorDist,*/ ActualSpeed, UpHillValue, currentRotationSpeed;

    public bool DontMove, forKnockBack;

    private bool isMove, lastYSpeed, touching, canJump;
    public bool hasJumped, isGrounded;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;

    private Vector3 rotatedDirection, FinalDirection, rtY, TmD, ForwardRotatedDirection, fallLenght, BottomPlatVel;

    private Quaternion _lookRotation, surfaceAngle;

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
    Animation runner;


    // Use this for initialization
    void Start () {

        PlayerRb = this.GetComponent<Rigidbody>();
        Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        KnockBack = this.GetComponent<PlayerKnockback>();
        runner = theRunningGuy.GetComponent<Animation>();
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
        GravityApplyer();

        Animator();

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
        if (isGrounded == true)
        {
            airTime = 0.0f;

            if (hasJumped == true)
            {
                forKnockBack = true;
                initialAirSpeed = 0.0f;
                hasJumped = false;
            }
            else {
                CurrentMidAirJumpCount = InitialMidAirJumpCount;
                forKnockBack = false;
            }
            canJump = true;
        }
        else {
            airTime += Time.deltaTime;
            canJump = false;
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

        float EulerX = surfaceAngle.eulerAngles.x;
        float EulerZ = surfaceAngle.eulerAngles.z;

        Quaternion qx = Quaternion.AngleAxis(EulerX, Vector3.right);
        Quaternion qz = Quaternion.AngleAxis(EulerZ, Vector3.forward);
        Quaternion qy = Quaternion.AngleAxis(cameraRot, Vector3.up);
        Quaternion q = qx * qz * qy;

        //Added this so that the player stops moving with the camera if player doesn't give input
        if (isMove == false)
        {
            rtY = qy * lookDirection;
        }

        TmD = q * moveDirection;

        // adds uphill/downhill only if on ground
        if (isGrounded == true)
            {
                UpHillValue = TmD.y;
            }
            else {
                UpHillValue = 0.0f;
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
        // Check this
        //

        //ForwardRotatedDirection = _lookRotation * Vector3.forward * 2;
    }

    void ApplyingDirection()
    {

		Vector3 vel = PlayerRb.velocity;

		Vector3 finalDirection = new Vector3(rotatedDirection.x, (UpHillValue*ActualSpeed*1.05f)+fallLenght.y, rotatedDirection.z);
		FinalDirection = _lookRotation * finalDirection;



		///DrawRAY!!!!!!
        Debug.DrawRay(PlayerRb.position, PlayerRb.velocity, Color.green);

        //Debug.Log ("TmD.y = " + TmD.y);
        //Debug.Log(surfaceAngle.eulerAngles.x + "," + surfaceAngle.eulerAngles.z); -- not yeet
        //PlayerRb.AddRelativeForce(finalDirection *ActualSpeed);
        if (PlayerRb.velocity.magnitude <= 0.1f && airTime > 0.1f) {
            airTime = 0.0f;
            initialAirSpeed = 0.0f;
        }

        vel = new Vector3(FinalDirection.x * ActualSpeed, FinalDirection.y , FinalDirection.z * ActualSpeed);

        if (touching == true || forwardDist <= 1.0f)
        {
            if (isGrounded == false && DontMove == false)
            {
                //Debug.Log ("WallKick?");
                vel = new Vector3(0.0f, FinalDirection.y, 0.0f);
                if (forwardDist >= 1.2f)
                {
                    vel = new Vector3(FinalDirection.x * ActualSpeed, FinalDirection.y, FinalDirection.z * ActualSpeed);
                }

            }
        }
       

        vel = vel + BottomPlatVel;

        // KnockBack Will move the player instead of the player Itself...
        if (DontMove == true)
        {
            vel = new Vector3(KnockBack.FinalKnockBack.x, (UpHillValue * ActualSpeed * 1.05f) + fallLenght.y, KnockBack.FinalKnockBack.z);
        }

        PlayerRb.velocity = vel;

    }

    void JumpNow() {
        if (DontMove == false)
        {
            if ((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 0")) && canJump == true)
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
        RaycastHit hit_2;
        RaycastHit hit_3;

        //Debug.DrawRay(new Vector3 (PlayerRb.position.x, PlayerRb.position.y-1.0f,PlayerRb.position.z), _lookRotation * Vector3.forward*10.0f, Color.red);
        //Debug.DrawRay(new Vector3(PlayerRb.position.x, PlayerRb.position.y + 1.0f, PlayerRb.position.z), _lookRotation * Vector3.forward * 10.0f, Color.yellow);

        if (Physics.Raycast(PlayerRb.position, new Vector3(0.0f,-1.0f,0.0f), out hit))
        {
			if (hit.transform.tag == "Untagged" || hit.transform.tag == "Kill" || hit.transform.tag == "StompNPC"|| hit.transform.tag == "NPC_charge")
            {
                floorDist = hit.distance;
                surfaceAngle = Quaternion.FromToRotation(hit.normal, new Vector3(0.0f, -1.0f, 0.0f));
                if (hit.rigidbody && isGrounded == true)
                    BottomPlatVel = hit.rigidbody.velocity;
                else
                    BottomPlatVel = Vector3.zero;
            }

        }
        if (Physics.Raycast(new Vector3(PlayerRb.position.x, PlayerRb.position.y - 1.0f, PlayerRb.position.z), _lookRotation * Vector3.forward, out hit_2)
            && Physics.Raycast(new Vector3(PlayerRb.position.x, PlayerRb.position.y + 1.0f, PlayerRb.position.z), _lookRotation * Vector3.forward, out hit_3))
        {
            //Debug.Log(hit_2.transform.tag);
            if (hit_2.transform.tag == "Untagged" || hit_3.transform.tag == "Untagged"|| hit_2.transform.tag == "StompNPC"|| hit_3.transform.tag == "StompNPC")
            {
                if (hit_2.distance < hit_3.distance)
                {
                    forwardDist = hit_2.distance;
                }
                else if (hit_3.distance < hit_2.distance)
                {
                    forwardDist = hit_3.distance;
                }
                else
                {
                    forwardDist = hit_2.distance;
                }
            }
        }
        /*else
        if (Physics.Raycast(new Vector3(PlayerRb.position.x, PlayerRb.position.y + 1.0f, PlayerRb.position.z), _lookRotation * Vector3.forward, out hit_3))  {
            forwardDist = hit_3.distance;
        }*/
        //Debug.Log(forwardDist);
    }

    void IsGrounded() {
        //Debug.Log(floorDist);
        if (floorDist <= AcceptedFloorDist)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    void Animator() {
        Vector3 RelativPlayerMove = PlayerRb.velocity - BottomPlatVel;
        if ( RelativPlayerMove.magnitude >= 2.0f)
        {
            runner.Play();
        }
        else
        {
            runner.Stop();
        }
    }

    void GravityApplyer() {
        // if (currentfallSpeed <= terminalSpeed)
        //    
        //else
        //    currentfallSpeed = terminalSpeed;

        currentfallSpeed = initialAirSpeed + (setGrav * airTime);

        fallLenght = Vector3.down * currentfallSpeed;

        if (currentfallSpeed >= terminalSpeed) {
            fallLenght.y = -terminalSpeed;
        }

    }

	void OnCollisionEnter(Collision collision){
		//Debug.Log (collision.relativeVelocity);
		if (collision.gameObject) {
			touching = true;
		}	
	}

	void OnCollisionExit(Collision collision){
		if (collision.gameObject) {
			touching = false;
		}	
	}
}
