using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Rigidbody Prb;
	Transform Camera_Rot;
	RelativGrav JumpingC;
	private float angle_1;
	public float jumpSpeed = -10.0f;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 moveAngle = Vector3.zero;
	float surfaceAngle_z;
	public float moveSpeed = 0.5f;
	private bool canitjump;
	// Use this for initialization
	void Start () {
		Prb = this.GetComponent<Rigidbody> ();
		Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
		JumpingC = this.GetComponent<RelativGrav>();
		canitjump = false;

	}
	
	// Update is called once per frame
	void Update () {
		ControlOrientation ();
		if (Input.GetKeyDown ("space") && canitjump == true) {
			Debug.Log ("is this touched");
			JumpingC.setInitialSpeed(jumpSpeed);
		}
		canitjump = JumpingC.IsItGrounded();
	}
	void ControlOrientation(){
		float HorizMov = Input.GetAxis ("Horizontal");
		float VertMov = Input.GetAxis ("Vertical");
		surfaceAngle_z = JumpingC.currentSurfaceAngle_z();
		//finds angle of camera relative to world & angle of surface
		float cameraRot = Camera_Rot.rotation.eulerAngles.y;
		Debug.Log (surfaceAngle_z);
		//float EulerX = surfaceAngle.eulerAngles.x;
		//float EulerZ = surfaceAngle.eulerAngles.z;
		//converts floats to horizontal and vertical value depending on camera orientation
		moveDirection = new Vector3(HorizMov, 0, VertMov);
		//Quaternion qx = Quaternion.AngleAxis(EulerX, Vector3.right);
		//Quaternion qz = Quaternion.AngleAxis(EulerZ, Vector3.forward); 
		//Quaternion q1 = qx * qz;
		//Vector3 rotatedAngle = q1 * moveDirection;
		//moveAngle = new Vector3 (EulerX, 0, EulerZ);

		Quaternion qy =  Quaternion.AngleAxis(cameraRot, Vector3.up);

		Vector3 rotatedDirection = qy * moveDirection;
		Debug.DrawLine (Vector3.zero, moveDirection, Color.green);
		//applies the direction to GamePbject Player rigidbody
		Prb.transform.Translate (rotatedDirection*moveSpeed);
	}
}
