﻿using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	Transform player;
	private float  offsetY;
	private Vector3 relativePos;
	private Quaternion rotset;
	public float SetCamRotSpeed = 100.0f;
	private float CamRotSpeed = 0.0f; 
	
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<Transform>();
		//offsetY = this.transform.position.y - player.transform.position.y;
	}
	
	void LateUpdate ()
	{
		//Camera following
		//Debug.Log (offsetY);
		//this.transform.position.y = player.transform.position.y + offsetY;

		//CameraOrbits --V
		if (Input.GetKey(KeyCode.I)) {
			CamRotSpeed = SetCamRotSpeed;
		}
		else if (Input.GetKey (KeyCode.O)) {
			CamRotSpeed = -SetCamRotSpeed;
		} else {
			CamRotSpeed = 0.0f;
		}
		//Vector3 HorizVect = new Vector3 (0.0f, HorizonCount, 0.0f);
		this.transform.RotateAround(player.position, Vector3.up, CamRotSpeed * Time.deltaTime);
		//this.transform.Translate(Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
		//to look at player--V
		relativePos = player.position - this.transform.position;
		rotset = Quaternion.LookRotation (relativePos);
		this.transform.rotation = rotset;

	}
}
