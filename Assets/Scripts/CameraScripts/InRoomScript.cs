using UnityEngine;
using System.Collections;

public class InRoomScript : MonoBehaviour {
    public GameObject CamForThisRoom;
    Transform Player;
    Vector3 CamPositionForThisRoom;
    Vector3 OriginalCamPos;
    OfficialCameraMovement OrigCam;
    //Transform OriginalCam;
    Camera OriginalCam;

    public bool Activator = false;

	// Use this for initialization
	void Start () {
		OrigCam = GameObject.FindGameObjectWithTag("CamMovement_2.0").GetComponent<OfficialCameraMovement>();
        //OriginalCam = GameObject.FindGameObjectWithTag("CamMovement_2.0").GetComponent<Transform>(); 
        OriginalCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Player = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        CamPositionForThisRoom = CamForThisRoom.transform.position;
    }

    public void AssignPlayer(GameObject p)
    {
        Player = p.transform;
        //Debug.Log(Player.ToString());
    }

    // Update is called once per frame
    void Update () {

        if (Activator == true)
        {
            OrigCam.DoNotMove = true;
            OriginalCam.transform.position = Vector3.Lerp(OriginalCam.transform.position, CamPositionForThisRoom, OrigCam.OrigCameraSpeed*0.2f * Time.deltaTime);
            OriginalCam.transform.LookAt(Player.transform.position);

        }
        else {
            OrigCam.DoNotMove= false;
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
