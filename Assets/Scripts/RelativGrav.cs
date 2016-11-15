using UnityEngine;
using System.Collections;

public class RelativGrav : MonoBehaviour {
	//Transform PlayerMesh;
	public float fallAccel = 2.0f;
	public float initialSpeed = 0.0f;
	public float currentRotSpeed = 2.0f;
	public float terminalSpeed = 2.0f;
	public float currentfallSpeed = 0.0f;
	private bool isGrounded;
	private float airTime;
	float surfaceAngleZ;
	Collider floor;
	Vector3 dwnL;
	Vector3 posRun;
	//Vector3 posFloor;
	float floorDist;
	//Use this for initialization
	void Start () {
		//CenterG = GameObject.FindGameObjectWithTag ("GravP").GetComponent<Rigidbody>();
		//PlayerMesh = GameObject.Find ("PlayerMesh").GetComponent<Transform> ();
		airTime = 0.0f;
		dwnL = transform.TransformDirection (Vector3.down);
		//isGrounded = false;
		//floor = GameObject.FindGameObjectWithTag ("Ground").GetComponent<Collider> ();
		//posFloor = floor.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		posRun = this.transform.position;

		FloorMeasure ();
		Debug.Log (isGrounded);
		FallTowards();

		if (isGrounded == false) {
			airTime += Time.deltaTime;
		}
		if (floorDist <= 1.2f) {
			currentfallSpeed = 0.0f;
			isGrounded = true;
			airTime = 0.0f;
			//initialSpeed = 0.0f;
		} else if(floorDist > 1.2f){
			isGrounded = false;
		}
	}
	public void setInitialSpeed(float innitspeed){
		if (isGrounded == true)
			initialSpeed = innitspeed;
		Debug.Log ("initial jumps speed = " + initialSpeed);
	}
	public bool IsItGrounded(){
		return isGrounded;
	}
	void FallTowards(){
		if (currentfallSpeed <= terminalSpeed) 
			currentfallSpeed = initialSpeed + (fallAccel * airTime);
		else
			currentfallSpeed = terminalSpeed;
		/*this.GetComponent<Rigidbody> ().transform.rotation = Quaternion.Slerp (
			this.GetComponent<Rigidbody> ().transform.rotation, 
			Quaternion.LookRotation (CenterG.transform.position -this.GetComponent<Rigidbody> ().transform.position),
			currentRotSpeed * Time.deltaTime);*/
		
		//this.GetComponent<Rigidbody> ().transform.position += this.GetComponent<Rigidbody> ().transform.forward * currentfallSpeed * Time.deltaTime;
		this.GetComponent<Rigidbody> ().transform.Translate (Vector3.down*currentfallSpeed*Time.deltaTime);
	}
	public float currentSurfaceAngle_z(){
		return surfaceAngleZ;
	}
	void FloorMeasure(){
		RaycastHit hit;
		if (Physics.Raycast(posRun, dwnL, out hit)){
			floorDist = hit.distance;
			surfaceAngleZ = Vector3.Angle(hit.normal,-dwnL);

		}
	}
}
