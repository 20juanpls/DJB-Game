using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPortalColorer : MonoBehaviour {

	ParticleSystem portal;
	SavefileManager sm;

	// Use this for initialization
	void Start () {
		portal = this.transform.parent.FindChild("PortalFX").gameObject.GetComponent<ParticleSystem> ();
		sm = new SavefileManager ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
