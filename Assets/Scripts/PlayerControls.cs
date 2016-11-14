using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	Rigidbody Prb;
	Transform Camera_Rot;
	RelativGrav JumpingC;
	private float angle_1;
	public float jumpSpeed = -10.0f;
	private Vector3 moveDirection = Vector3.zero;
	public float moveSpeed = 0.5f;
	private bool canitjump;
	// Use this for initialization
	void Start () {
		Prb = this.GetComponent<Rigidbody> ();
		//JumpingC = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<RelativGrav> ();
		Camera_Rot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
		JumpingC = this.GetComponent<RelativGrav>();
		canitjump = false;

	}
	
	// Update is called once per frame
	void Update () {
		float HorizMov = Input.GetAxis ("Horizontal");
		float VertMov = Input.GetAxis ("Vertical");
		//finds angle of camera relative to world
		float cameraRot = Camera_Rot.rotation.eulerAngles.y;
		//converts floats to horizontal and vertical value depending on camera orientation
		moveDirection = new Vector3(HorizMov, 0, VertMov);
		Vector3 rotatedDirection = Quaternion.AngleAxis (cameraRot, Vector3.up) * moveDirection;
		//applies the direction to GamePbject Player rigidbody
		Prb.transform.Translate (rotatedDirection*moveSpeed);


		if (Input.GetKeyDown ("space") && canitjump == true) {
			Debug.Log ("is this touched");
			JumpingC.setInitialSpeed(jumpSpeed);
		}
		canitjump = JumpingC.IsItGrounded();
	}
}
