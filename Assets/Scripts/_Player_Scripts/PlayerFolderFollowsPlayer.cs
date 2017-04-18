using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFolderFollowsPlayer : MonoBehaviour {
    Transform ThePlayer;
    //Quaternion originalRotation;

    public bool IsOnRotate;
	// Use this for initialization
	void Start () {
        ThePlayer = this.transform.GetChild(0).GetComponent<Transform>();
        //originalRotation = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = ThePlayer.transform.position;
        Debug.Log(this.transform.position);


        if (!IsOnRotate) {
            this.transform.rotation = Quaternion.identity;
        }
	}
}
