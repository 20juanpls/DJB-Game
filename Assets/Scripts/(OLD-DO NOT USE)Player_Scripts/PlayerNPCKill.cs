using UnityEngine;
using System.Collections;

public class PlayerNPCKill : MonoBehaviour {
    public bool InCollider;
    public Vector3 JumpVect;

	void Start(){
	}
	void Update(){
        if (InCollider == true)
        {
            JumpVect = Vector3.up * 100.0f;
        }
        else {
            JumpVect = Vector3.zero;
        }
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "JumpCollider"){
            //Debug.Log("InCollider");
            InCollider = true;
        }
	}
	void OnTriggerExit(Collider other){
		//if (other.tag == "JumpCollider"){
            //Debug.Log("OutOfCollider");
            InCollider = false;
        //}
	}
}
