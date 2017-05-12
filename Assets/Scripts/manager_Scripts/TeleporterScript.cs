using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour {
    GameObject Player, Loader;
    Transform  T_Top, T_Bottom, T_Left, T_Right;

    Vector3 TelePos, Gpos_Top, Gpos_L, Gpos_R, OposTop,OposL,OposR, TargetT, TargetL,TargetR;

    public float PiecesSpeed, MinDist;
    float PlayerDist;

    public bool IsOn, Activated, TeleporterOpen;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("PlayerMesh").gameObject;

        T_Top = transform.FindChild("top");
        T_Bottom = transform.FindChild("bottom");
        T_Left = transform.FindChild("L_side");
        T_Right = transform.FindChild("R_side");
        Loader = transform.FindChild("Loader").gameObject;

        TelePos = this.transform.position;

        Gpos_Top = new Vector3(0.0f, 8.0f, 0.0f) + TelePos;
        Gpos_L = new Vector3(-5.0f, 4.6f, 0.0f) + TelePos;
        Gpos_R = new Vector3(5.0f, 4.6f, 0.0f) + TelePos;

        OposTop = new Vector3(0.0f,1.2f,0.0f) + TelePos;
        OposL = new Vector3(-2.8f, 0.75f, 0.0f) + TelePos;
        OposR = new Vector3(2.8f, 0.75f, 0.0f) + TelePos;

        T_Top.transform.position = OposTop;
        T_Left.transform.position = OposL;
        T_Right.transform.position = OposR;

        Loader.SetActive(false);
    }

    public void AssignPlayer(GameObject p)
    {
        Player = p;
    }

    // Update is called once per frame
    void Update () {
        DistanceMeasurer();
        FancyMovement();
        if (IsOn)
        {
            if (PlayerDist < MinDist)
                Activated = true;
            else
                Activated = false;
        }
        else {
            Activated = false;
        }
        LoaderLoading();	
	}

    void DistanceMeasurer() {
        PlayerDist = (Player.transform.position - TelePos).magnitude;
    }
    void FancyMovement() {
        if (Activated)
        {
            TargetT = Gpos_Top;
            TargetL = Gpos_L;
            TargetR = Gpos_R;

			this.transform.FindChild ("PortalFX").gameObject.SetActive (true);



        }
        else {
            TargetT = OposTop;
            TargetL = OposL;
            TargetR = OposR;
			this.transform.FindChild ("PortalFX").gameObject.SetActive (false);
        }

        T_Top.transform.position = Vector3.MoveTowards(T_Top.transform.position, TargetT, PiecesSpeed * Time.deltaTime);
        T_Left.transform.position = Vector3.MoveTowards(T_Left.transform.position, TargetL, PiecesSpeed * Time.deltaTime);
        T_Right.transform.position = Vector3.MoveTowards(T_Right.transform.position, TargetR, PiecesSpeed * Time.deltaTime);

        if (T_Top.transform.position == Gpos_Top && T_Left.transform.position == Gpos_L && T_Right.transform.position == Gpos_R)
            TeleporterOpen = true;
        else
            TeleporterOpen = false;

    }
    void LoaderLoading() {
        if (TeleporterOpen)
            Loader.SetActive(true);
        else
            Loader.SetActive(false);
    }
}
