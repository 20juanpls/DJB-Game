using UnityEngine;
using System.Collections;

public class InRoomScript : MonoBehaviour {
    public GameObject CamForThisRoom;
    Transform Player;
    Vector3 CamPositionForThisRoom;
    Vector3 OriginalCamPos;
    CameraScript OrigCam;
    Transform OriginalCam;

    public bool Activator = false;

	// Use this for initialization
	void Start () {
        OrigCam = GameObject.FindGameObjectWithTag("MainCameraMovement").GetComponent<CameraScript>();
        OriginalCam = GameObject.FindGameObjectWithTag("MainCameraMovement").GetComponent<Transform>();
        Player = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        CamPositionForThisRoom = CamForThisRoom.transform.position;
    }

    public void AssignPlayer(GameObject p)
    {
        Player = p.transform;
        Debug.Log(Player.ToString());
    }

    // Update is called once per frame
    void Update () {

        if (Activator == true)
        {
            OrigCam.NotControlledByPlayer = true;
            OriginalCam.position = Vector3.MoveTowards(OriginalCam.position, CamPositionForThisRoom, OrigCam.RecalibrationSpeed * Time.deltaTime);
        }
        else {
            OrigCam.NotControlledByPlayer = false;
        }
	}
    void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerMesh") {
            Activator = true;
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.tag == "PlayerMesh")
        {
            Activator = false;
        }
    }
}
