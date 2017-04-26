using UnityEngine;
using System.Collections;

public class PlayerNPCKill : MonoBehaviour {
    public bool InCollider;
    public Vector3 JumpVect;

    int InCollisionCount;
    int UpdateCount;

	void Start(){
	}
	void Update(){
        //Debug.Log(InCollisionCount);
        if (InCollider == true)
        {
            JumpVect = Vector3.up * 100.0f;
            UpdateCount++;
            if (UpdateCount > InCollisionCount) {
                UpdateCount = 0;
                InCollisionCount = 0;
                InCollider = false;
            }
        }
        else {
            JumpVect = Vector3.zero;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "JumpCollider")
        {
            //Debug.Log("InCollider");
            InCollider = true;
//            Debug.Log(other.gameObject.activeSelf);
            //if (other.gameObject.activeSelf)
            InCollisionCount++;
        }
    }

    void OnTriggerExit(Collider other){
		//if (other.tag == "JumpCollider"){
            //Debug.Log("OutOfCollider");
            InCollider = false;
            UpdateCount = 0;
            InCollisionCount = 0;
        //}
    }
}
