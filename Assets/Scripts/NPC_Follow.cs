using UnityEngine;
using System.Collections;

public class NPC_Follow : MonoBehaviour {

	Rigidbody rb;
	GameObject prb;
	float distance;
	public float minDistance;
	public float atkDistance;
	//Around ~25 seems to be good
	public float atkSpeed;
	Camera main;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		prb = GameObject.Find ("Player");
		distance = 100.0f;
		minDistance = 20.0f;
		atkDistance = 10.0f;
		atkSpeed = 25.0f;
		main = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		
		distance = Vector3.Distance (rb.transform.position, prb.transform.position);

		//if distance between player and npc is less than mindistance...
		if (distance <= minDistance) {


			//npc looks at player 
			rb.transform.LookAt (prb.transform.position);
			//Player looks at npc: BROKEN, character movement becomes relative to the NPC, but camera is not moivng. --Noah
			//main.transform.LookAt(rb.transform.position);
			//npc rotation locked  to y only
			rb.transform.rotation = new Quaternion (0.0f, rb.transform.rotation.y, 0.0f, rb.transform.rotation.w);

			//if player is being watched and gets too close...
			if (distance <= atkDistance) {
				rb.AddForce (rb.transform.forward * atkSpeed);
			}
		}
	}
}
