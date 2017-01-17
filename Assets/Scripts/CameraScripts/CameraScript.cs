using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Transform lookAt; // the object the camera is looking at
    Transform TheCamera;
    Transform CamRb;

    public bool NotControlledByPlayer = false;

    public float OrigDistance = 10.0f;
    public float ZoomInDistance = 2.0f;
    public float ZoomOutDistance = 20.0f;
    public float CamXSpeed = 2.0f;
    public float CamYSpeed = 2.0f;
    public float MaxHeight = 3.0f;
    public float MinHeight = 0.0f;
    public float RecalibrationSpeed = 1.0f;

    private Vector3 velocity;
    //public float LookingSpeed = 10.0f;

    public float StartX;
    public float StartY;
    private float CurrentX;
    private float CurrentY;
    private float CurrentCamXSpeed;
    private float CurrentCamYSpeed;

    private float CurrentDistance;

    private Vector3 FinalCamPosition;

	void Start ()
	{
        CurrentX = StartX;
        CurrentY = StartY;
		TheCamera = null;//GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        CamRb = this.GetComponent<Transform>();
        lookAt = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        CamRb.position = lookAt.position + Quaternion.Euler(CurrentY, CurrentX, 0.0f) * new Vector3(0.0f, 0.0f, -OrigDistance);
        CurrentDistance = OrigDistance;
    }

	//Noah Squeeze for player assignment
	public void AssignPlayer(GameObject p){
        CurrentX = StartX;
        CurrentY = StartY;
        lookAt = p.transform;
        CamRb.position = lookAt.position + Quaternion.Euler(CurrentY, CurrentX, 0.0f) * new Vector3(0.0f, 0.0f, -OrigDistance);
        CurrentDistance = OrigDistance;
        //Debug.Log(lookAt.ToString());
    }

	void Update ()
	{
        //Debug.Log(CurrentY+","+CurrentX);

        CamXRotSpeed();
        CurrentX += CurrentCamXSpeed;
        CamYRotSpeed();
        CurrentY += CurrentCamYSpeed;
        CurrentY = Mathf.Clamp(CurrentY, MinHeight, MaxHeight);
        ZoomInOut();

        if (NotControlledByPlayer == false) {
            Vector3 dir = new Vector3(0.0f, 0.0f, -CurrentDistance);
            Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0.0f);
            FinalCamPosition = lookAt.position + rotation * dir;
        }
    }

    void LateUpdate() {
		if (TheCamera != null) {
	        if (NotControlledByPlayer == false)
	        {

				//Debug.Log (lookAt.position);
	            //preferred one===>//
				//CamRb.position = Vector3.MoveTowards(CamRb.position, FinalCamPosition, RecalibrationSpeed * Time.deltaTime);
	            //CamRb.position = Vector3.SmoothDamp(CamRb.position, FinalCamPosition, ref velocity,RecalibrationSpeed);
	            //CamRb.transform.Translate(FinalCamPosition * RecalibrationSpeed * Time.deltaTime);

	            CamRb.position = FinalCamPosition;


	            //CamRb.position = FinalCamPosition;
	            //TheCamera.position = Vector3.MoveTowards(TheCamera.position, CamRb.position, RecalibrationSpeed * Time.deltaTime);
	            TheCamera.position = CamRb.position;
	            TheCamera.rotation = CamRb.rotation;
	        }

			//CamRb.position = FinalCamPosition;
			if (CurrentDistance == ZoomInDistance) {
				CamRb.transform.LookAt (lookAt.position + new Vector3 (0.0f, 1.0f, 0.0f));
				TheCamera.position = CamRb.position;
				TheCamera.rotation = CamRb.rotation;

				// Smoothly rotate towards the target point.
				//CamRb.rotation = Quaternion.Slerp(CamRb.rotation, Quaternion.LookRotation(lookAt.position + new Vector3(0.0f, 1.0f, 0.0f) - CamRb.position), LookingSpeed * Time.deltaTime);
			} else {
				//CamRb.rotation = Quaternion.Slerp(CamRb.rotation, Quaternion.LookRotation(lookAt.position - CamRb.position), LookingSpeed * Time.deltaTime);
				CamRb.transform.LookAt (lookAt.position);
				TheCamera.position = CamRb.position;
				TheCamera.rotation = CamRb.rotation;
			}
		}

        

		Debug.DrawRay(CamRb.position, CamRb.rotation * Vector3.forward*10.0f, Color.yellow);

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
   
    void CamYRotSpeed(){
        if (Input.GetKey(KeyCode.U))
        {
                CurrentCamYSpeed = CamYSpeed;
        }
        else if (Input.GetKey(KeyCode.P))
        {
                CurrentCamYSpeed = -CamYSpeed;
        }
        else {
            CurrentCamYSpeed = 0.0f;
        }
    }

    void ZoomInOut() {
        //ZoomIn..
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (CurrentDistance == OrigDistance)
            {
                CurrentDistance = ZoomInDistance;
            }
            else if (CurrentDistance == ZoomOutDistance)
            {
                CurrentDistance = OrigDistance;
            }
        }
        //Zoom Out...
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (CurrentDistance == OrigDistance)
            {
                CurrentDistance = ZoomOutDistance;
            }
            else if (CurrentDistance == ZoomInDistance)
            {
                CurrentDistance = OrigDistance;
            }
        }
    }
}
