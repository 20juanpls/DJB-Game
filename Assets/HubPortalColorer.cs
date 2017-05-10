using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPortalColorer : MonoBehaviour {

	//Please write out all maps
	ParticleSystem portalStage_1_3;	//manual input :(
	int totalMaps;	//Manual input :(
	SavefileManager sm;

	// Use this for initialization
	void Start () {
		sm = new SavefileManager ();
		totalMaps = 1;	//MANUAL INPUT


		//PORTAL NAME
		portalStage_1_3 = this.transform.parent.gameObject.GetComponent<ParticleSystem> ();
		Debug.Log(sm.GetStatus ("Stage_1_3"));
		var main = portalStage_1_3.main;
		if (sm.GetStatus ("Stage_1_3") == 0) {	//IF UNCOMPLETED, RED BUTTHOLE
			//Debug.Log ("cool kat ag");
			//portalStage_1_3.main.startColor = new Color (255f, 255f, 255f);
			main.startColor = new Color (254f, 0f, 00f, 81f);
		} else {		//IF IT IS COMPLETED, BLUE BUTTHOLE
			main.startColor = new Color (.318f, .604f, 1f, .5f);
			//main.startColor = new Color32(81f,154f,255f,81f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
