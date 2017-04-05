using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIndicator : MonoBehaviour {
    public bool HitsPlayer;
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerMesh") {
            HitsPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerMesh")
        {
            HitsPlayer = false;
        }
    }
}
