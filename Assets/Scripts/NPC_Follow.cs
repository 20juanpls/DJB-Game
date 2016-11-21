using UnityEngine;
using System.Collections;

public class NPC_Follow : MonoBehaviour {

	Rigidbody rb;
	GameObject prb;
	public int npcTurningSpeed;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		prb = GameObject.Find ("Player_mesh-er...");
		npcTurningSpeed = 1;
	}
	
	// Update is called once per frame
	void Update () {

		Quaternion.Slerp (rb.transform.rotation, prb.transform.rotation, npcTurningSpeed);
	}
}
