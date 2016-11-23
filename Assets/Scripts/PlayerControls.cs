using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Rigidbody TransP;
	Transform Camera_Rot;
	RelativGrav JumpingC;
	public float jumpSpeed = -10.0f;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 lookDirection = Vector3.zero;
	private Vector3 rotatedDirection;
	private Vector3 rtY;
    private Vector3 TmD;
	private Vector3 FinalDirection;
	private Quaternion _lookRotation;
	Quaternion surfaceAngle;
	public float moveSpeed = 0.5f;
	private bool canitjump;
	public float rotationSpeed = 1.0f;
	private float HorizLook;
	private float VertLook;
	// Use this for initialization
    //11/17/16 - Make sure that if this game object is NOT grounded, rotatedDirection is switched to moveDirection on Air
    //so that the tiny air boosts don't happen... OK()
	void Start () {
		TransP = this.GetComponent<Rigidbody> ();
		Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
		JumpingC = this.GetComponent<RelativGrav>();
		canitjump = false;
	}
	
	// Update is called once per frame
	void Update () {
		float HorizMov = Input.GetAxis ("Horizontal");
		float VertMov = Input.GetAxis ("Vertical");
		if (HorizMov!=0.00f||VertMov!=0.00f) {
			HorizLook = HorizMov;
			VertLook = VertMov;
		}
		moveDirection = new Vector3(HorizMov, 0, VertMov);
		lookDirection = new Vector3 (HorizLook, 0, VertLook);

		ControlOrientation ();


		if (Input.GetKeyDown("space")) {
			JumpingC.setInitialSpeed(jumpSpeed);
			}
		if (canitjump == false) {
			JumpingC.setInitialSpeed(0.0f);
		}
		canitjump = JumpingC.IsItGrounded();
	
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

		rtY = qy * lookDirection;

        TmD = q*moveDirection;

<<<<<<< HEAD
		float TmDyzAngle;
		if (TmD.y != 0 || TmD.z != 0) {
			TmDyzAngle = Mathf.Rad2Deg * Mathf.Atan (TmD.y / TmD.z);
		} else {
			TmDyzAngle = 0;
		}

		//Debug.Log (TmDyzAngle);
		Quaternion fialel = Quaternion.AngleAxis (TmDyzAngle,Vector3.forward);

		_lookRotation = Quaternion.LookRotation (rtY);
<<<<<<< Updated upstream

        _lookRotation = Quaternion.LookRotation (rtY);

=======
=======
        _lookRotation = Quaternion.LookRotation (rtY);
>>>>>>> 57810ad4f7974b89f2a28dbff858ce661acf8657
>>>>>>> Stashed changes
		TransP.transform.rotation = Quaternion.Slerp (TransP.transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);

		rotatedDirection = new Vector3 (moveforward.x, 0.0f, moveforward.z);
		if (canitjump == true) {
			//DON'T MESS WITH THIS, THIS IS JUST TO SPABELIZE THE CHARACTER WHEN ITS GOING DOWNHILL!!!!
			if (TmD.y < 0) {
				TmD.y = TmD.y - 0.08f;
			}
			FinalDirection = new Vector3(rotatedDirection.x,TmD.y,rotatedDirection.z);

			TransP.transform.Translate (FinalDirection * moveSpeed);
		} else {
			TransP.transform.Translate (rotatedDirection * moveSpeed);
		}
		Debug.DrawRay (Vector3.zero,FinalDirection,Color.green);
	}
}
