using UnityEngine;
using System.Collections;

public class RelativGrav : MonoBehaviour {
    Transform Player;
    PlayerControls PlayerMesh;
	public float fallAccel = 2.0f;
    private float currentFallAccel;
	public float initialSpeed = 0.0f;
	public float currentRotSpeed = 2.0f;
	public float terminalSpeed = 2.0f;
	public float currentfallSpeed = 0.0f;
    public float minGroundDistance = 1.2f;
	private bool isGrounded;
	public float airTime;
	Quaternion surfaceAngle;
	Collider floor;
	Vector3 dwnL,UpL;
    public Vector3 posRun, fallLenght; //FallingDirection;
    //Vector3 posFloor;
    public float floorDist;
	float CeilDist;
	//Use this for initialization
	void Start () {
        currentFallAccel = fallAccel;
        //CenterG = GameObject.FindGameObjectWithTag ("GravP").GetComponent<Rigidbody>();
        PlayerMesh = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<PlayerControls> ();
        Player = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        airTime = 0.0f;
		dwnL = transform.TransformDirection (Vector3.down);
        UpL = transform.TransformDirection(Vector3.up);
        //FallingDirection = transform.TransformDirection(Vector3.zero);
		//isGrounded = false;
		//floor = GameObject.FindGameObjectWithTag ("Ground").GetComponent<Collider> ();
		//posFloor = floor.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		posRun = this.transform.position;

		FloorMeasure ();
		FallTowards();

		if (isGrounded == false) {
			airTime += Time.deltaTime;
		}
        if (currentFallAccel == 0.0f) {
            airTime = 0.0f;
        }
		if (floorDist <= minGroundDistance) {
			currentfallSpeed = 0.0f;
			isGrounded = true;
			airTime = 0.0f;
		} else if(floorDist > minGroundDistance)
        {
			isGrounded = false;
		}
        if (CeilDist <= minGroundDistance && CeilDist!=0.0f) {
            initialSpeed = 0.0f;
            currentfallSpeed = 0.0f;
        }
	}

    public void setInitialSpeed(float innitvalue, bool isPriority){
        if (isGrounded == true && isPriority == false)
        {
            initialSpeed = innitvalue;
        }
        if (isPriority == true) {
            initialSpeed = innitvalue;
        }
		//Debug.Log ("initial jumps speed = " + initialSpeed);
	}
    public void setFallAcceleration(float fallAcc) {
        currentFallAccel = fallAcc;
    }

    public void setCurrentFallSpeed(float innitvalue) {
        currentfallSpeed = innitvalue;
    }

    public bool IsItGrounded(){
		return isGrounded;
	}

    void FallTowards(){
            if (currentfallSpeed <= terminalSpeed)
                currentfallSpeed = initialSpeed + (currentFallAccel * airTime);
            else
                currentfallSpeed = terminalSpeed;
        /*this.GetComponent<Rigidbody> ().transform.rotation = Quaternion.Slerp (
			this.GetComponent<Rigidbody> ().transform.rotation, 
			Quaternion.LookRotation (CenterG.transform.position -this.GetComponent<Rigidbody> ().transform.position),
			currentRotSpeed * Time.deltaTime);*/

        //this.GetComponent<Rigidbody> ().transform.position += this.GetComponent<Rigidbody> ().transform.forward * currentfallSpeed * Time.deltaTime;
        //Debug.Log(fallLenght);
        fallLenght = Vector3.down * currentfallSpeed * Time.deltaTime;
        //FallingDirection = PlayerMesh.rotatedDirection* PlayerMesh.moveSpeed + fallLenght;

       // if (PlayerMesh.IWantToJump == true)
            //Player.GetComponent<Rigidbody>().transform.Translate(FallingDirection);

        if (transform != Player) {
            this.GetComponent<Rigidbody>().transform.Translate(Vector3.down * currentfallSpeed * Time.deltaTime);
        }
        //Debug.DrawRay(Vector3.zero, Vector3.down * currentfallSpeed * Time.deltaTime, Color.red);
        //Debug.Log(fallLenght);

    }

    public Quaternion currentSurfaceAngle(){
		return surfaceAngle;
	}

    void FloorMeasure(){
		RaycastHit hit;
        RaycastHit hit_2;

        if (Physics.Raycast(posRun, dwnL, out hit)){
			floorDist = hit.distance;
			surfaceAngle = Quaternion.FromToRotation(hit.normal,-dwnL);
		}

        if (Physics.Raycast(posRun,UpL,out hit_2)) {
            CeilDist = hit_2.distance;
        }
	}
}
