using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleTouchScript : MonoBehaviour {

    GameObject PlayerMesh;
    public bool PlayerOnButton, PonTrigger;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (PonTrigger == true)
                PlayerOnButton = true;
        else if (PlayerMesh != null){
            //Debug.Log((transform.position - PlayerMesh.transform.position).magnitude);
            if ((transform.position-PlayerMesh.transform.position).magnitude > 2.5f)
                PlayerOnButton = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerMesh") {
            PonTrigger = true;
            PlayerMesh = other.gameObject;
            //PlayerOnButton = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PonTrigger = false;
        //PlayerOnButton = false;
    }


    /*private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts) {
            if (contact.otherCollider.gameObject.transform.tag == "PlayerMesh") {
                PlayerOnButton = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        PlayerOnButton = false;
    }*/
}
