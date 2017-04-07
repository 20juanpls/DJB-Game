using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovingPlatform : MonoBehaviour {

	//I want to be able to have this specific game object move from a list of given checkpoints
	//I want to be able to set the time it travels between these points

	public float time;
	public List<GameObject> checkpoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (checkpoints.Capacity);
		Debug.Log ("Start: " + checkpoints [0]);
		Debug.Log ("Stop: " + checkpoints [1]);
		this.transform.position = Vector3.Lerp (checkpoints [0].transform.position, checkpoints [1].transform.position, time);
	}
}
