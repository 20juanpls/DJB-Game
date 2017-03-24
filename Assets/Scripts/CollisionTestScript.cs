using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTestScript : MonoBehaviour {
    public bool touching;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    /*void OnCollisionEnter(Collision collision)
    {
        //Debug.Log (collision.relativeVelocity);
        if (collision.gameObject)
        {
            touching = true;
            Debug.Log(collision.transform.tag);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject)
        {
            touching = false;
        }
    }*/
    void OnCollisionStay(Collision collision) {
        if (collision.gameObject) {
            touching = true;
            Debug.Log(collision.transform.tag);
        }

    }

}
