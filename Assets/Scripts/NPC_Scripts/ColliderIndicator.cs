using UnityEngine;
using System.Collections;

public class ColliderIndicator : MonoBehaviour {

    private bool AmIHitting;
    // Use this for initialization
	void Start () {
        AmIHitting = false;
	}
	
	// Update is called once per frame
	void Update () {    
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerMesh")
            AmIHitting = true;
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "PlayerMesh")
            AmIHitting = false;
    }
    public bool IsHitting() {
        return AmIHitting;
    }
}
