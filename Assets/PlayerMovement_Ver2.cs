using UnityEngine;
using System.Collections;

public class PlayerMovement_Ver2 : MonoBehaviour {
    Rigidbody PlayerRb;
    Transform Camera_Rot;

    private float HorizLook, VertLook, /*floorDist,*/ ActualSpeed;

    private bool isMove, isGrounded, lastYSpeed, hasJumped, touching;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;

    private Vector3 rotatedDirection, FinalDirection, rtY, TmD, ForwardRotatedDirection, fallLenght;

    private Quaternion _lookRotation, surfaceAngle;

    public float rotationSpeed = 20.0f;
    public float MoveSpeed = 10.0f;
    public float setGrav = 10.0f;
    public float JumpSpeed = 10.0f;
    public float currentfallSpeed;
    public float terminalSpeed = 10.0f;
	public float InitialMidAirJumpCount = 1.0f;

    float currentFallAccel;
	private float initialAirSpeed = 0.0f, forwardDist;
	float CurrentMidAirJumpCount;
    public float airTime;

    public float floorDist;

    public GameObject theRunningGuy;
    Animation runner;


    // Use this for initialization
    void Start () {

        PlayerRb = this.GetComponent<Rigidbody>();
        Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        runner = theRunningGuy.GetComponent<Animation>();
        ActualSpeed = MoveSpeed;
		hasJumped = false;
		CurrentMidAirJumpCount = InitialMidAirJumpCount;
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

        //Debug.Log(TmD.y);

        //PlayerRb.velocity = vel;
        if (isGrounded == true)
        {
            airTime = 0.0f;

            if (hasJumped == true)
            {
                initialAirSpeed = 0.0f;
                hasJumped = false;
            }
			CurrentMidAirJumpCount = InitialMidAirJumpCount;
        }
        else {
            airTime += Time.deltaTime;
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

        _lookRotation = Quaternion.LookRotation(rtY);


        PlayerRb.transform.rotation = Quaternion.Slerp(PlayerRb.transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        // Check this
        //

        //ForwardRotatedDirection = _lookRotation * Vector3.forward * 2;
    }

    void ApplyingDirection()
    {


		Debug.DrawRay (PlayerRb.position, PlayerRb.velocity, Color.green);
		//Debug.DrawRay (PlayerRb.position, _lookRotation *rotatedDirection * ActualSpeed, Color.red);

		Vector3 vel = PlayerRb.velocity;

		Vector3 finalDirection = new Vector3(rotatedDirection.x, TmD.y+fallLenght.y, rotatedDirection.z);
		finalDirection = _lookRotation * finalDirection;

       // Debug.Log (touching);

        //Debug.Log ("TmD.y = " + TmD.y);
        Debug.Log(surfaceAngle.eulerAngles.x + "," + surfaceAngle.eulerAngles.z);
        //PlayerRb.AddRelativeForce(finalDirection *ActualSpeed);
        if (PlayerRb.velocity.magnitude <= 0.1f && airTime > 0.1f) {
            airTime = 0.0f;
            initialAirSpeed = 0.0f;
        }

        vel = finalDirection * ActualSpeed;
        if (touching == true || forwardDist <= 1.0f)
        {
            if (isGrounded == false)
            {
                //Debug.Log ("WallKick?");
                vel = new Vector3(0.0f, finalDirection.y, 0.0f) * ActualSpeed;
                if (forwardDist >= 1.2f)
                {
                    vel = finalDirection * ActualSpeed;
                }

            }
        }
		PlayerRb.velocity = vel;

    }

    void JumpNow() {
        if (Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 0"))
        {
            initialAirSpeed = JumpSpeed;
        }
		if (airTime >= 0.01f) {
			hasJumped = true;
		}
			
		if ((Input.GetKeyDown ("space") || Input.GetKeyDown ("joystick button 11")) && isGrounded == false && CurrentMidAirJumpCount > 0) {
			initialAirSpeed = JumpSpeed;
			airTime = 0.0f;
			CurrentMidAirJumpCount--;
		}
    }

    void FloorMeasure()
    {
        RaycastHit hit;
        RaycastHit hit_2;

        //Debug.DrawRay(PlayerRb.position, PlayerRb.velocity, Color.green);
        //Debug.DrawRay(PlayerRb.position, _lookRotation * Vector3.forward*10.0f, Color.red);

        if (Physics.Raycast(PlayerRb.position, new Vector3(0.0f,-1.0f,0.0f), out hit))
        {
            floorDist = hit.distance;
            surfaceAngle = Quaternion.FromToRotation(hit.normal, new Vector3(0.0f, -1.0f, 0.0f));
        }
        if (Physics.Raycast(PlayerRb.position, _lookRotation * Vector3.forward, out hit_2)) {
            forwardDist = hit_2.distance;
        }
    }

    void IsGrounded() {
        //Debug.Log(floorDist);
        if (floorDist <= 1.7f)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    void Animator() {
        if (PlayerRb.velocity.magnitude >= 2.0f)
        {
            runner.Play();
        }
        else
        {
            runner.Stop();
        }
    }

    void GravityApplyer() {
        if (currentfallSpeed <= terminalSpeed)
            currentfallSpeed = initialAirSpeed + (setGrav * airTime);
        else
            currentfallSpeed = terminalSpeed;

		fallLenght = Vector3.down * currentfallSpeed;

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
