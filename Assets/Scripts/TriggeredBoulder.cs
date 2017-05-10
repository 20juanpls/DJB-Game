using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredBoulder : MonoBehaviour {
    public bool boulderDead;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BoulderDeath") {
            boulderDead = true;
        }
    }
}
