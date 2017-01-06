using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Transform lookAt; // the object the camera is looking at
    Transform TheCamera;
    Transform CamTransform;

    public bool NotControlledByPlayer = false;

    public float OrigDistance = 10.0f;
    public float ZoomInDistance = 2.0f;
    public float ZoomOutDistance = 20.0f;
    public float CamXSpeed = 2.0f;
    public float CamYSpeed = 2.0f;
    public float MaxHeight = 3.0f;
    public float MinHeight = 0.0f;
    public float RecalibrationSpeed = 1.0f;
    //public float LookingSpeed = 10.0f;

    private float CurrentX;
    private float CurrentY;
    private float CurrentCamXSpeed;
    private float CurrentCamYSpeed;

    private float CurrentDistance;

    private Vector3 FinalCamPosition;

	void Start ()
	{
        TheCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        CamTransform = transform;
        lookAt = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        CamTransform.position = lookAt.position + Quaternion.Euler(CurrentY, CurrentX, 0.0f) * new Vector3(0.0f, 0.0f, -OrigDistance);
        CurrentDistance = OrigDistance;
    }

	//Noah Squeeze for player assignment
	public void AssignPlayer(GameObject p){
		lookAt = p.transform;
        CamTransform.position = lookAt.position + Quaternion.Euler(CurrentY, CurrentX, 0.0f) * new Vector3(0.0f, 0.0f, -OrigDistance);
        CurrentDistance = OrigDistance;
        Debug.Log(lookAt.ToString());
    }

	void Update ()
	{
        //Debug.Log(CurrentY);
        CamXRotSpeed();
        CurrentX += CurrentCamXSpeed;
        CamYRotSpeed();
        CurrentY += CurrentCamYSpeed;
        CurrentY = Mathf.Clamp(CurrentY, MinHeight, MaxHeight);
        ZoomInOut();
    }

    void LateUpdate() {
        if (NotControlledByPlayer == false)
        {
            Vector3 dir = new Vector3(0.0f, 0.0f, -CurrentDistance);
            Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0.0f);
            FinalCamPosition = lookAt.position + rotation * dir;
            CamTransform.position = Vector3.MoveTowards(CamTransform.position, FinalCamPosition, RecalibrationSpeed * Time.deltaTime);
            TheCamera.position = CamTransform.position;
            TheCamera.rotation = CamTransform.rotation;
        }

        //CamTransform.position = FinalCamPosition;
        if (CurrentDistance == ZoomInDistance)
        {
            CamTransform.LookAt(lookAt.position + new Vector3(0.0f, 1.0f, 0.0f));
            TheCamera.position = CamTransform.position;
            TheCamera.rotation = CamTransform.rotation;

            // Smoothly rotate towards the target point.
            //CamTransform.rotation = Quaternion.Slerp(CamTransform.rotation, Quaternion.LookRotation(lookAt.position + new Vector3(0.0f, 1.0f, 0.0f) - CamTransform.position), LookingSpeed * Time.deltaTime);
        }
        else
        {
            //CamTransform.rotation = Quaternion.Slerp(CamTransform.rotation, Quaternion.LookRotation(lookAt.position - CamTransform.position), LookingSpeed * Time.deltaTime);
            CamTransform.LookAt(lookAt.position);
            TheCamera.position = CamTransform.position;
            TheCamera.rotation = CamTransform.rotation;
        }

        Debug.DrawRay(CamTransform.position, CamTransform.rotation * Vector3.forward*10.0f, Color.yellow);

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
