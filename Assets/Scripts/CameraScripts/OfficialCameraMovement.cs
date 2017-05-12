using UnityEngine;
using System.Collections;

public class OfficialCameraMovement: MonoBehaviour
{
	public bool DoNotMove;

    RuneCoinManager RuneCoinManager;

    public Transform thisPos;
	public GameObject Target;
    public GameObject ThePlayer;
	public Vector3 CameraOffset = new Vector3(0.0f, 10.0f, -20.0f);
	public float CameraDistance;
	public float ZoomInDistance;
	public float ZoomOutDistance;
	public float OrigCameraSpeed = 10f;

	private float CurrentCamSpeed, OrigDistance;

	public float CamXSpeed;
	public float CamYSpeed;
	float MinHeight = -70.0f;
	float MaxHeight = 60.0f;


	public float joystickDeadzone = 0.1f;

	private float CurrentCamXSpeed;
	private float CurrentCamYSpeed;

	private Camera _camera;

	private bool zoomLockIn,zoomLockOut;
	public bool OnGround, NoControls;
	bool MajorCollectGet;

    Vector3 CamNxPos;

	void Start()
	{
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");
        Target = ThePlayer;
        try
        {
            RuneCoinManager = GameObject.Find("SpecialCoinManager").GetComponent<RuneCoinManager>();
        }
        catch
        {
			Debug.Log (RuneCoinManager);
            RuneCoinManager = null;
        }
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		thisPos = GetComponent<Transform>();
		thisPos.position = CameraOffset + Target.transform.position;
		_camera.transform.position = thisPos.position;
		_camera.transform.rotation = thisPos.rotation;
		CurrentCamSpeed = OrigCameraSpeed;
		OrigDistance = 1.0f;
		CameraDistance = OrigDistance;
		zoomLockIn = true;
		zoomLockOut = true;


	}

	public void AssignPlayer(GameObject p)
	{
        ThePlayer = p;
        Target = ThePlayer;
		thisPos.position = CameraOffset + Target.transform.position;
		_camera.transform.position = thisPos.position;
		_camera.transform.rotation = thisPos.rotation;
		CurrentCamXSpeed = 0.0f;
		CurrentCamYSpeed = 0.0f;
		CameraDistance = OrigDistance;
	}

	void FixedUpdate()
	{
		//Debug.Log(Input.GetAxis("XB1_RightLeft"));
		if (_camera != null && Target != null)
		{
			if (RuneCoinManager != null)
				MajorCollectGet = RuneCoinManager.MajorCollectableGet;
			else
				MajorCollectGet = false;


			if (MajorCollectGet)
				Target = RuneCoinManager.TheCollect;
            else
                Target = ThePlayer;
            //Call Interior Functions

            //Debug.Log (OnGround);
            if (!NoControls)
            {
                ZoomInOut();    //Detect Zoom button clicking
                CamXRotSpeed(); //Detect horizontal input, add/subtract x speed
                                //momentary if conditional
                                //if (!OnGround)
                CamYRotSpeed(); //Detect vertical input, add/subtract y speed
            }

			CurrentCamYSpeed = Mathf.Clamp(CurrentCamYSpeed, MinHeight, MaxHeight);	//Clamp the camera's Y angle

			Vector3 targetPos = Target.transform.position;
			Vector3 offset = CameraOffset;

			float cameraAngle = _camera.transform.eulerAngles.y;
            //Actually unused
            //float cameraAngle = _camera.transform.eulerAngles.y;

            offset = Quaternion.Euler(CurrentCamYSpeed, CurrentCamXSpeed, 0.0f) * offset * CameraDistance;

            //Important
            //if (RuneCoinManager.MajorCollectableGet)
            //{
            //    offset = Vector3.zero;
            //}

            CamNxPos = Vector3.Lerp(_camera.transform.position, targetPos + offset, CurrentCamSpeed * /*Time.deltaTime*/0.2f);
            thisPos.transform.position = CamNxPos;
			thisPos.transform.LookAt(targetPos);



            //Debug.Log(Vector3.Distance(thisPos.transform.position, targetPos + offset));
            Debug.DrawRay(_camera.transform.position, _camera.transform.rotation*Vector3.forward*10.0f, Color.yellow);
            /*if (Vector3.Distance(thisPos.transform.position, targetPos + offset) > 5.0f)
            {
                CurrentCamSpeed = OrigCameraSpeed * 0.2f;
            }
            else {
                CurrentCamSpeed = OrigCameraSpeed;
            }*/
			if (MajorCollectGet) {
				_camera.transform.LookAt (targetPos);
				NoControls = true;
			} else {
				NoControls = false;
			}

			if (DoNotMove == false && !MajorCollectGet)
			{
				_camera.transform.position = thisPos.transform.position;
				_camera.transform.rotation = thisPos.transform.rotation;
			}
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		//_camera.transform.position = new Vector3(_camera.transform.position.x,collision.transform.position.y,_camera.transform.position.z);
		foreach (ContactPoint contact in collision.contacts) {
			GameObject Other_GmObj = contact.otherCollider.gameObject;
			string Other_Tag = Other_GmObj.transform.tag;

			Debug.Log (Other_Tag);
			if (Other_Tag == "Untagged") {
				OnGround = true;
			}
		
		}
	}

	void CamXRotSpeed(){
		//Clockwise
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("XB1_RightLeft") > joystickDeadzone || Input.GetKey(KeyCode.I))
		{
			CurrentCamXSpeed += CamXSpeed;
		}
		//counterclockwise
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("XB1_RightLeft") < -joystickDeadzone || Input.GetKey(KeyCode.O))
		{
			CurrentCamXSpeed -= CamXSpeed;
		}
	}

	void CamYRotSpeed() {
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("XB1_UpDown") > joystickDeadzone || Input.GetKey(KeyCode.U))
		{
			CurrentCamYSpeed += CamYSpeed;
		}
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("XB1_UpDown") < -joystickDeadzone || Input.GetKey(KeyCode.P))
		{
			CurrentCamYSpeed -= CamYSpeed;
		}
	}

	void ZoomInOut()
	{	

		//ZoomIn..
		if (Input.GetKeyDown(KeyCode.J) || Input.GetAxis("XB1_Zoom") > 0.1f)
		{
			if (zoomLockIn)
			{
				if (CameraDistance == OrigDistance)
				{
					CameraDistance = ZoomInDistance;
				}
				else if (CameraDistance == ZoomOutDistance)
				{
					CameraDistance = OrigDistance;
				}
				zoomLockIn = false;
			}
		}
		else
		{
			if (Input.GetAxis("XB1_Zoom") <= 0.1f){
				zoomLockIn = true;
			}
		}
		//Zoom Out...
		if (Input.GetKeyDown(KeyCode.K) || Input.GetAxis("XB1_ZoomOut") > 0.1f)
		{
			if (zoomLockOut)
			{
				if (CameraDistance == OrigDistance)
				{
					CameraDistance = ZoomOutDistance;
				}
				else if (CameraDistance == ZoomInDistance)
				{
					CameraDistance = OrigDistance;
				}
				zoomLockOut = false;
			}
		}
		else
		{
			if (Input.GetAxis("XB1_ZoomOut") <= 0.1f){
				zoomLockOut = true;
			}
		}

	}
}
