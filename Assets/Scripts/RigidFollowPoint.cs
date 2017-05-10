using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidFollowPoint : MonoBehaviour {
    Rigidbody Centre;
    public GameObject FollowWith;
    Vector3 thePointVel;
    // Use this for initialization
    void Start () {
        this.transform.position = FollowWith.transform.position;
        if (this.GetComponent<Rigidbody>() != null)
            Centre = this.GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        if (FollowWith != null) {
            thePointVel = FollowWith.GetComponent<PointMovementSpeed>().pointVel;


            Centre.velocity = thePointVel;
        }
	}
}
