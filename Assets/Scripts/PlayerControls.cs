using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Rigidbody Prb;
	Transform TransP;
	Transform Camera_Rot;
	RelativGrav JumpingC;
	private float angle_1;
	public float jumpSpeed = -10.0f;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 rotatedDirection;
	private Vector3 rtXZ;
	private Vector3 forwardDirection;
	private Quaternion _lookRotation;
	Quaternion surfaceAngle;
	public float moveSpeed = 0.5f;
	private bool canitjump;
	public float rotationSpeed = 1.0f;
	// Use this for initialization
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
		moveDirection = new Vector3(HorizMov, 0, VertMov);
		
		ControlOrientation ();

		//PlayerMeshOrientation (); //<Dont use this just yet...>

		if (Input.GetKeyDown("space")) {
			JumpingC.setInitialSpeed(jumpSpeed);
			}
		if (canitjump == false) {
			JumpingC.setInitialSpeed(0.0f);
		}
		canitjump = JumpingC.IsItGrounded();
	
	}
	void ControlOrientation(){
		surfaceAngle = JumpingC.currentSurfaceAngle();
		//finds angle of camera relative to world & angle of surface
		float cameraRot = Camera_Rot.rotation.eulerAngles.y;
		//Debug.Log (surfaceAngle.eulerAngles);
		float EulerX = -surfaceAngle.eulerAngles.x;
		float EulerZ = -surfaceAngle.eulerAngles.z;
		//Debug.Log (EulerX + "," + EulerZ);
		//converts floats to horizontal and vertical value depending on camera orientation
		Quaternion qx = Quaternion.AngleAxis(EulerX, Vector3.right);
		Quaternion qz = Quaternion.AngleAxis(EulerZ, Vector3.forward); 
		Quaternion qy = Quaternion.AngleAxis(cameraRot, Vector3.up);
		Quaternion q = qx * qz * qy;

		rotatedDirection = q * moveDirection;
		rtXZ = qy * moveDirection;
		forwardDirection = qz * moveDirection;
		//Debug.DrawLine (Vector3.zero, rotatedDirection, Color.green);
		//applies the direction to GamePbject Player rigidbody
		//_lookRotation =  new Quaternion(0.0f,Quaternion.LookRotation(rotatedDirection).y,0.0f,0.0f);
		//_lookRotation = Quaternion.LookRotation(rtXZ);
		//Debug.Log (_lookRotation);
		//TransP.transform.rotation = Quaternion.Slerp(this.transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
		//this.transform.rotation = Quaternion.LookRotation (rtXZ);

		Prb.transform.Translate (rotatedDirection*moveSpeed);

		//Debug.Log ("this rotation:"+this.transform.rotation);
	}
	void PlayerMeshOrientation(){
		_lookRotation =  new Quaternion(0.0f,Quaternion.LookRotation(rotatedDirection).y,0.0f,0.0f);
		this.transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
	}
}
