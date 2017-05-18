using UnityEngine;
using System.Collections;

public class ColliderIndicator : MonoBehaviour {

    Rigidbody npcRB;

    public bool AmIHitting;
    public bool HitsGround, AmHittingGroundRunnin;
    // Use this for initialization
	void Start () {
        AmIHitting = false;
        npcRB = this.GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {    
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerMesh")
        {
            //Get NPC Rigidbody then reverse it's velocity;
            npcRB.velocity = -npcRB.velocity;//*0.8f;

            AmIHitting = true;
        }
    }

    /*void OnTriggerExit(Collider other) {
        if (other.tag == "PlayerMesh")
            AmIHitting = false;
    }*/
}
