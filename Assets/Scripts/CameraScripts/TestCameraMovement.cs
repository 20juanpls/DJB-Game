using UnityEngine;
using System.Collections;

public class TestCameraMovement : MonoBehaviour
{
    public bool DoNotMove;

    public Transform thisPos;
    public GameObject Target;
    public Vector3 CameraOffset = new Vector3(0.0f, 10.0f, -20.0f);
    public float OrigCameraSpeed = 10f;
    private float CurrentCamSpeed;

    public float CamXSpeed;
    public float CamYSpeed;
    public float MinHeight = 0.0f;
    public float MaxHeight = 10.0f;

    private float CurrentCamXSpeed;
    private float CurrentCamYSpeed;

    private Camera _camera;

    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("PlayerMesh");
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        thisPos = GetComponent<Transform>();
        thisPos.position = CameraOffset + Target.transform.position;
        _camera.transform.position = thisPos.position;
        _camera.transform.rotation = thisPos.rotation;
        CurrentCamSpeed = OrigCameraSpeed;

    }

    public void AssignPlayer(GameObject p)
    {
        Target = p;
        thisPos.position = CameraOffset + Target.transform.position;
        _camera.transform.position = thisPos.position;
        _camera.transform.rotation = thisPos.rotation;
        CurrentCamXSpeed = 0.0f;
        CurrentCamYSpeed = 0.0f;
    }

    void FixedUpdate()
    {
        if (_camera != null && Target != null)
        {
            CamXRotSpeed();
            CamYRotSpeed();

            CurrentCamYSpeed = Mathf.Clamp(CurrentCamYSpeed, MinHeight, MaxHeight);

            Vector3 targetPos = Target.transform.position;
            Vector3 offset = CameraOffset;

            float cameraAngle = _camera.transform.eulerAngles.y;

            offset = Quaternion.Euler(CurrentCamYSpeed, CurrentCamXSpeed, 0.0f) * offset;

            thisPos.transform.position = Vector3.Lerp(_camera.transform.position, targetPos + offset, CurrentCamSpeed * Time.deltaTime);
            thisPos.transform.LookAt(targetPos);

            //Debug.Log(Vector3.Distance(thisPos.transform.position, targetPos + offset));

                if (Vector3.Distance(thisPos.transform.position, targetPos + offset) > 5.0f)
                {
                    CurrentCamSpeed = OrigCameraSpeed * 0.2f;
                }
                else {
                    CurrentCamSpeed = OrigCameraSpeed;
                }

            if (DoNotMove == false)
            {
                _camera.transform.position = thisPos.transform.position;
                _camera.transform.rotation = thisPos.transform.rotation;
            }
        }
    }

    void CamXRotSpeed(){
        if (Input.GetKey(KeyCode.I))
        {
            CurrentCamXSpeed += CamXSpeed;
        }
        else if (Input.GetKey(KeyCode.O))
        {
            CurrentCamXSpeed -= CamXSpeed;
        }
    }

    void CamYRotSpeed() {
        if (Input.GetKey(KeyCode.U))
        {
            CurrentCamYSpeed += CamYSpeed;
        }
        else if (Input.GetKey(KeyCode.P))
        {
            CurrentCamYSpeed -= CamYSpeed;
        }
    }
}
