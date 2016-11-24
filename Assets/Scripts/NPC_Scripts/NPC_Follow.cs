using UnityEngine;
using System.Collections;

public class NPC_Follow : MonoBehaviour {

    ColliderIndicator ColIn;
    Rigidbody rb;
	GameObject prb;
	float distance;
	public float minDistance;
	public float atkDistance;
	//Around ~25 seems to be good
	public float atkForce;
    private float OldAtkForce;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
        prb = GameObject.Find ("Player");
        ColIn = GameObject.Find("NPC_Collider").GetComponent<ColliderIndicator>();
		distance = 100.0f;
		minDistance = 20.0f;
		atkDistance = 10.0f;
		atkForce = 5000.0f;
        OldAtkForce = atkForce;
	}
	
	// Update is called once per frame
	void Update () {
		
		distance = Vector3.Distance (rb.transform.position, prb.transform.position);

		//if distance between player and npc is less than mindistance...
		if (distance <= minDistance) {

			//npc looks at player 
			//rb.transform.LookAt (prb.transform.position);
            //npc rotation locked  to y only
            // rb.transform.rotation = Quaternion.Euler(0.0f, rb.transform.rotation.eulerAngles.y, 0.0f);
            rb.transform.rotation = Quaternion.Slerp(
                rb.transform.rotation,
                Quaternion.LookRotation(new Vector3(prb.transform.position.x,0.0f, prb.transform.position.z) - new Vector3(rb.transform.position.x, 0.0f, rb.transform.position.z)), 
                Time.deltaTime*7.0f);

			//if player is being watched and gets too close...
			if (distance <= atkDistance) {
				rb.AddForce (rb.transform.forward * atkForce);
			}
		}
        if (ColIn.IsHitting() == true) {
            StartCoroutine(WaitingForNextAttack(2.0f));
        }
	}
    IEnumerator WaitingForNextAttack( float waitTime) {
        this.atkForce = 0.0f;
        yield return new WaitForSeconds(waitTime);
        this.atkForce = OldAtkForce;
    }
}
