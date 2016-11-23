using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Rigidbody TransP;
	Transform Camera_Rot;
	RelativGrav JumpingC;

	public float jumpSpeed = -10.0f;
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 1.0f;
    public int InitialmidAirJumpCount = 1;
    private float HorizLook, VertLook, forwardDist;

    private Vector3 moveDirection = Vector3.zero;
	private Vector3 lookDirection = Vector3.zero;
	private Vector3 rotatedDirection, rtY, TmD, FinalDirection,ForwardRotatedDirection;

    private Quaternion _lookRotation, surfaceAngle, templookRotation;

    private bool canitjump, isMove, CanMove;
    private int CurrentMidAirJumpCount = 0;
	// Use this for initialization

	void Start () {
		TransP = this.GetComponent<Rigidbody> ();
		Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
		JumpingC = this.GetComponent<RelativGrav>();
        canitjump = false;
        isMove = false;
        CanMove = true;
	}
	
	// Update is called once per frame
	void Update () {
        float HorizMov = Input.GetAxis ("Horizontal");
		float VertMov = Input.GetAxis ("Vertical");

        if (HorizMov != 0.00f || VertMov != 0.00f){
            HorizLook = HorizMov;
            VertLook = VertMov;
            isMove = false;
        }else{
            isMove = true;
        }

        moveDirection = new Vector3(HorizMov, 0, VertMov);
        lookDirection = new Vector3(HorizLook, 0, VertLook);

		ControlOrientation ();
        ForwardMeasure();

        if (forwardDist <= 0.6f){
                CanMove = false;
            }

        if (VertLook <= -0.01 || forwardDist > 0.6f){
                CanMove = true;
            }

        if (CanMove == false){
                rotatedDirection = Vector3.zero;
                FinalDirection = Vector3.zero;
            }

        ApplyingDirection();
        JumpNow();
	
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
        if (canitjump == true)
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
        Debug.DrawRay(TransP.transform.position, ForwardRotatedDirection,Color.green);
        //IMPORTANT: If I want to add a collider in front of player, I need to make sure the raycast ignores that collider...Tagging is key.
        if (Physics.Raycast(TransP.transform.position, ForwardRotatedDirection, out hit))
        {
            forwardDist = hit.distance;
            //surfaceAngle = Quaternion.FromToRotation(hit.normal, -dwnL);
        }
    }

    void JumpNow() {
        if (Input.GetKeyDown("space")){
            JumpingC.setInitialSpeed(jumpSpeed);
        }

        if (Input.GetKeyDown("space") && canitjump == false && CurrentMidAirJumpCount >= 0)
        {
            JumpingC.setCurrentFallSpeed(0.0f);
            JumpingC.setInitialSpeed(jumpSpeed);
            CurrentMidAirJumpCount--;
        }

        if (canitjump == false){
            JumpingC.setInitialSpeed(0.0f);
        }

        if (canitjump == true) {
            CurrentMidAirJumpCount = InitialmidAirJumpCount;
        }
        canitjump = JumpingC.IsItGrounded();
    }
}
