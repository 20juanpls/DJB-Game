using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	Transform player;
    Rigidbody thisrb;

	private Vector3 offset;

	public float DistY = 6.0f;
	public float DistX = 10.0f;
    public float MinDist = 6.0f;
    public float MaxDist = 12.0f;
    public float MinHeight = -5.0f;
    public float MaxHeight = 2.0f;

	//private Vector3 relativePos;
	//private Quaternion rotset;
	public float SetCamRotSpeed = 2.3f;
    private float CamRotSpeed = 0.0f;
    public float ZoomSpeedY = 0.2f;
    private float ZoomPosY = 0.0f;
    private float XZDist = 0.0f;
    public float ZoomSpeedXZ = 0.2f;
    private float CurrentZoomSpeedDist;
    //private float yOrigOffset;
    float XDist;
    float ZDist;

    private Vector3 directionZoom;
	
	void Start ()
	{
        thisrb = this.GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<Transform>();
        offset = new Vector3(0.0f, DistY, DistX);
    }

	//Noah Squeeze for player assignment
	public void AssignPlayer(GameObject p){
		player = p.transform;
	}

	void LateUpdate ()
	{
        CamRotation ();
        SetZoomYSet();
        SetZoomXZSet();
        directionZoom = Quaternion.Euler(transform.rotation.x,0.0f,0.0f)* Vector3.forward;
        //Debug.DrawRay(transform.position,directionZoom,Color.green);
        offset = Quaternion.AngleAxis (CamRotSpeed, Vector3.up) * offset;
		transform.position = player.position + offset;
        //thisrb.AddForce(offset);

        CamSetter();
        transform.LookAt(player.position);


        //Camera following
        //Debug.Log(player.position);
		//CameraOrbits --V


	}
	void CamRotation(){

		//Debug.Log (Input.GetAxis ("XB1_RightLeft"));
		CamRotSpeed = Input.GetAxis("XB1_RightLeft") * SetCamRotSpeed;
		if (Input.GetAxis ("XB1_RightLeft") <= 0.1 && Input.GetAxis("XB1_RightLeft") >= -0.1) {
			CamRotSpeed = 0.0f;
		}

		//for keyboard input
		if (Input.GetKey(KeyCode.I)) {
			CamRotSpeed = SetCamRotSpeed;
		}
		else if (Input.GetKey (KeyCode.O)) {
			CamRotSpeed = -SetCamRotSpeed;
		} else {
			//CamRotSpeed = 0.0f;
		}
	}
    void SetZoomYSet() {
        if (Input.GetKey(KeyCode.U))
        {
          if (ZoomPosY >= MinHeight)  
            ZoomPosY = ZoomPosY - ZoomSpeedY;
        }
        else if (Input.GetKey(KeyCode.P))
        {
          if(ZoomPosY <= MaxHeight)
            ZoomPosY = ZoomPosY + ZoomSpeedY;
        }
    }
    void SetZoomXZSet() {
        //XDist = Mathf.Abs(transform.position.x - player.transform.position.x);
        //ZDist = Mathf.Abs(transform.position.z - player.transform.position.z);
       // Debug.Log(AbsXDist+","+AbsZDist);
        //XZDist = Mathf.Sqrt((Mathf.Pow(AbsXDist, 2)) + (Mathf.Pow(AbsZDist, 2)));
        XZDist = Vector3.Distance(new Vector3(transform.position.x,0.0f, transform.position.z), new Vector3(player.transform.position.x, 0.0f, player.transform.position.z));
        //Debug.Log(XZDist);


        if (Input.GetKey(KeyCode.U))
        {
            if (XZDist >= MinDist)
            {
                CurrentZoomSpeedDist = CurrentZoomSpeedDist + ZoomSpeedXZ;
                //transform.Translate(directionZoom * -ZoomSpeedXZ);
                //XDist = XDist - (ZoomSpeedXZ * (Mathf.Cos(45 * Mathf.PI) / 180));
                //ZDist = ZDist - (ZoomSpeedXZ * (Mathf.Sin(45 * Mathf.PI) / 180));
            }
        }
        else if (Input.GetKey(KeyCode.P))
        {
            if (XZDist <= MaxDist)
            {
                CurrentZoomSpeedDist = CurrentZoomSpeedDist - ZoomSpeedXZ;
                //transform.Translate(directionZoom * ZoomSpeedXZ);
                //XDist = XDist + (ZoomSpeedXZ * (Mathf.Cos(45 * Mathf.PI) / 180));
                //ZDist = ZDist + (ZoomSpeedXZ * (Mathf.Sin(45 * Mathf.PI) / 180));
            }
        }
        //else {
        //    CurrentZoomSpeedDist = 0.0f;
		//}
        //Debug.Log(XDist+","+ ZDist);
    }
    void CamSetter() {
        transform.Translate(directionZoom * CurrentZoomSpeedDist);
        transform.position = new Vector3(transform.position.x, transform.position.y + ZoomPosY, transform.position.z);
        //thisrb.AddForce(Vector3.up*ZoomPosY*1000);
    }
}
