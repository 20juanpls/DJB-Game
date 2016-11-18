using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Rigidbody Prb;
	Transform TransP;
	Transform Camera_Rot;
	RelativGrav JumpingC;
	public float jumpSpeed = -10.0f;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 lookDirection = Vector3.zero;
	private Vector3 rotatedDirection;
	private Vector3 rtY;
    private Vector3 TmD;
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
		Prb = this.GetComponent<Rigidbody> ();
		TransP = this.GetComponent<Transform> ();
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
		float VectMeasure = moveDirection.magnitude;
		Vector3 moveforward = new Vector3 (0.0f, 0.0f, VectMeasure);

		surfaceAngle = JumpingC.currentSurfaceAngle();
		//finds angle of camera relative to world & angle of surface
		float cameraRot = Camera_Rot.rotation.eulerAngles.y;
		//Debug.Log (surfaceAngle.eulerAngles);
		float EulerX = -surfaceAngle.eulerAngles.x;
		float EulerZ = -surfaceAngle.eulerAngles.z;

        /*float oldCameraRotation = cameraRot;
        if (rotatedDirection * moveSpeed == Vector3.zero)
        {
            cameraRot = oldCameraRotation;
        }*/
        //^-- Failed attempt at trying to get the player stay still when camera is rotating :'(
        //converts floats to horizontal and vertical value depending on camera orientation

        Quaternion qx = Quaternion.AngleAxis(EulerX, Vector3.right);
		Quaternion qz = Quaternion.AngleAxis(EulerZ, Vector3.forward); 
		Quaternion qy = Quaternion.AngleAxis(cameraRot, Vector3.up);
        Quaternion q = qx * qz * qy;

		Debug.Log (EulerX+", EulerZ:"+EulerZ);

		rtY = qy * lookDirection;

        TmD = q*moveDirection;

		_lookRotation = Quaternion.LookRotation (rtY);
		TransP.transform.rotation = Quaternion.Slerp (TransP.transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);

		if (canitjump == true) {
			rotatedDirection = new Vector3 (moveforward.x, TmD.y, moveforward.z);
			TransP.transform.Translate (rotatedDirection * moveSpeed);
		} else {
			rotatedDirection = new Vector3 (moveforward.x, 0.0f, moveforward.z);
			TransP.transform.Translate (rotatedDirection * moveSpeed);
		}
		Debug.DrawRay (Vector3.zero,rotatedDirection,Color.green);
	}
}
