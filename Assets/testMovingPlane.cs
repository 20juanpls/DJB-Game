using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMovingPlane : MonoBehaviour {

    Quaternion AngleOfRot;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        AngleOfRot = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<PlayerMovement_Ver2>().processedAngle;

        this.transform.rotation = AngleOfRot;
	}
}
