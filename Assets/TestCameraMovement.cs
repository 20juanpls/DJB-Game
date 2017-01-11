using UnityEngine;
using System.Collections;

public class TestCameraMovement : MonoBehaviour {
	Transform thisCamera;
	Transform theCamera;
	Rigidbody PlayerRb;
	public float CamXSpeed;

	private float CurrentCamXSpeed;

	// Use this for initialization
	void Start () {
		thisCamera = this.gameObject.GetComponent<Transform> ();
		theCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
		PlayerRb = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Rigidbody>();
		CurrentCamXSpeed = CamXSpeed;

	}

	public void AssignPlayer(GameObject p){

		PlayerRb = p.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		CamXRotSpeed();
		//Debug.Log (CurrentCamXSpeed);

	
	}
	void LateUpdate() {
		
		Vector3 Horiz = Quaternion.Euler(0.0f,thisCamera.rotation.y,0.0f) * Vector3.right * CurrentCamXSpeed*Time.deltaTime ;
		Vector3 HorizPlayVel = PlayerRb.rotation* new Vector3 (PlayerRb.velocity.x, 0.0f, PlayerRb.velocity.z) * Time.deltaTime;

		Vector3 FinalMovement = Horiz + HorizPlayVel;

		//This one transforms the camera!!!!!!
		thisCamera.transform.Translate (FinalMovement);
		//This one updates LOOKAT;
		thisCamera.LookAt (PlayerRb.transform.position);

		theCamera.position = thisCamera.position;
		theCamera.rotation = thisCamera.rotation;

		Debug.DrawRay (thisCamera.position, PlayerRb.rotation*PlayerRb.velocity, Color.green);
	}

	void CamXRotSpeed() {
		if (Input.GetKey(KeyCode.I))
		{
			CurrentCamXSpeed = CamXSpeed;
		}
		else if (Input.GetKey(KeyCode.O))
		{
			CurrentCamXSpeed = -CamXSpeed;
		}
		else
		{
			CurrentCamXSpeed = 0.0f;
		}
	}
}
