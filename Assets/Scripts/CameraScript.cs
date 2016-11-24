using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	Transform player;

	private Vector3 offset;

	public float DistY = 6.0f;
	public float DistX = 10.0f;
    public float MinDist = 6.0f;
    public float MaxDist = 10.0f;
    public float MinHeight = -5.0f;
    public float MaxHeight = 6.0f;

	//private Vector3 relativePos;
	//private Quaternion rotset;
	public float SetCamRotSpeed = 1.0f;
    private float CamRotSpeed = 0.0f;
    public float ZoomSpeedY = 0.4f;
    private float ZoomPosY = 0.0f;
    private float XZDist = 0.0f;
    public float ZoomSpeedXZ = 0.4f;
    private float CurrentZoomSpeedDist;
    //private float yOrigOffset;
    float XDist;
    float ZDist;

    private Vector3 directionZoom;
	
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<Transform>();
        offset = new Vector3(player.position.x, player.position.y + DistY, player.position.z + DistX);
    }
	
	void LateUpdate ()
	{
        CamRotation ();
        SetZoomYSet();
        SetZoomXZSet();
        directionZoom = Quaternion.Euler(transform.rotation.x,0.0f,0.0f)* Vector3.forward;
        Debug.DrawRay(transform.position,directionZoom,Color.green);
        offset = Quaternion.AngleAxis (CamRotSpeed, Vector3.up) * offset;
		transform.position = player.position + offset;
        CamSetter();
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
    }
}
