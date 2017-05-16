using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMovementSpeed : MonoBehaviour {
    Rigidbody TheMainRigidBody;
    Vector3 distFromRigidBod;
    public Vector3 pointVel;

	// Use this for initialization
	void Start () {
        TheMainRigidBody = this.transform.parent.gameObject.GetComponent<speedcubetest>().followThis.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        distFromRigidBod = this.transform.position - TheMainRigidBody.position;
        //Debug.Log(TheMainRigidBody.angularVelocity.magnitude);
        Vector3 currRotVel = Vector3.Cross(distFromRigidBod, TheMainRigidBody.angularVelocity) * -1.0f;
        pointVel = TheMainRigidBody.velocity + currRotVel;
    }
}
