using UnityEngine;
using System.Collections;

public class TestCameraMovement : MonoBehaviour
{
	public bool DoNotMove;

	public Transform thisPos;
	public GameObject Target;
	public Vector3 CameraOffset = new Vector3(0.0f, 10.0f, -20.0f);
	public float CameraDistance;
	public float ZoomInDistance;
	public float ZoomOutDistance;
	public float OrigCameraSpeed = 10f;

	private float CurrentCamSpeed, OrigDistance;

	public float CamXSpeed;
	public float CamYSpeed;
	public float MinHeight = 0.0f;
	public float MaxHeight = 10.0f;

	public float joystickDeadzone = 0.1f;

	private float CurrentCamXSpeed;
	private float CurrentCamYSpeed;

	private Camera _camera;

	private bool zoomLockIn,zoomLockOut;

	void Start()
	{
		Target = GameObject.FindGameObjectWithTag("PlayerMesh");
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
		Target = p;
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
			ZoomInOut();
			CamXRotSpeed();
			CamYRotSpeed();

			CurrentCamYSpeed = Mathf.Clamp(CurrentCamYSpeed, MinHeight, MaxHeight);

			Vector3 targetPos = Target.transform.position;
			Vector3 offset = CameraOffset;

			float cameraAngle = _camera.transform.eulerAngles.y;

			offset = Quaternion.Euler(CurrentCamYSpeed, CurrentCamXSpeed, 0.0f) * offset * CameraDistance;

			//Important
			thisPos.transform.position = Vector3.Lerp(_camera.transform.position, targetPos + offset, CurrentCamSpeed * Time.deltaTime);
			//Important
			/*thisPos.transform.position = Vector3.Lerp(new Vector3(_camera.transform.position.x,0.0f,_camera.transform.position.z),
                new Vector3(targetPos.x,0.0f,targetPos.z) + new Vector3(offset.x,0.0f,offset.z), CurrentCamSpeed * Time.deltaTime)
                + Vector3.Lerp(new Vector3(0.0f,_camera.transform.position.y,0.0f), 
                new Vector3(0.0f, targetPos.y,0.0f) + new Vector3(0.0f,offset.y,0.0f),CurrentCamSpeed*0.1f*Time.deltaTime);
            */

			thisPos.transform.LookAt(targetPos);


			//Debug.Log(Vector3.Distance(thisPos.transform.position, targetPos + offset));

			/*if (Vector3.Distance(thisPos.transform.position, targetPos + offset) > 5.0f)
            {
                CurrentCamSpeed = OrigCameraSpeed * 0.2f;
            }
            else {
                CurrentCamSpeed = OrigCameraSpeed;
            }*/

			if (DoNotMove == false)
			{
				_camera.transform.position = thisPos.transform.position;
				_camera.transform.rotation = thisPos.transform.rotation;
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
