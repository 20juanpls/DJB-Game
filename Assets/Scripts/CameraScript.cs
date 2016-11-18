using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	Transform player;

	private Vector3 offset;

	public float DistY = 8.0f;
	public float DistX = 7.0f;

	//private Vector3 relativePos;
	//private Quaternion rotset;
	public float SetCamRotSpeed = 1.0f;
	private float CamRotSpeed = 0.0f; 
	//private float yOrigOffset;
	
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<Transform>();
		offset = new Vector3(player.position.x, player.position.y + DistY, player.position.z + DistX);
	}
	
	void LateUpdate ()
	{
		CamRotation ();
		offset = Quaternion.AngleAxis (CamRotSpeed, Vector3.up) * offset;
		transform.position = player.position + offset; 
		transform.LookAt(player.position);



		//Camera following
		//this.transform.position = playerPos-thisPos;
		//this.transform.Translate (offset);
		//CameraOrbits --V


	}
	void CamRotation(){
		if (Input.GetKey(KeyCode.I)) {
			CamRotSpeed = SetCamRotSpeed;
		}
		else if (Input.GetKey (KeyCode.O)) {
			CamRotSpeed = -SetCamRotSpeed;
		} else {
			CamRotSpeed = 0.0f;
		}
	}
}
