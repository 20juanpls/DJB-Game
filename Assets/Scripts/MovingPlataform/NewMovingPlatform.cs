using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovingPlatform : MonoBehaviour {

	//I want to be able to have this specific game object move from a list of given checkpoints
	//I want to be able to set the time it travels between these points

	public float time, buffer, deadzone;
	public List<GameObject> checkpoints;
	bool attatched;
	Collision other;
	int CDI, MPI;

	// Use this for initialization
	void Start () {
		MPI = 0;
		CDI = 1;
		this.transform.position = checkpoints [MPI].transform.position;
		//Debug.Log (checkpoints [CDI].transform.position + " - " + checkpoints [MPI].transform.position);
		Vector3 velo = new Vector3 (checkpoints [CDI].transform.position.x - checkpoints [MPI].transform.position.x, checkpoints [CDI].transform.position.y - checkpoints [MPI].transform.position.y, checkpoints [CDI].transform.position.z - checkpoints [MPI].transform.position.z);
		this.GetComponent<Rigidbody> ().velocity = (velo * buffer);
		//Debug.Log (this.GetComponent<Rigidbody> ().velocity);
		attatched = false;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (attatched) {
			float yVos = this.gameObject.GetComponent<Rigidbody> ().velocity.y;
			other.gameObject.GetComponent<Rigidbody> ().velocity += this.gameObject.GetComponent<Rigidbody> ().velocity;
			this.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3(this.gameObject.GetComponent<Rigidbody>().velocity.x,yVos,this.gameObject.GetComponent<Rigidbody>().velocity.z);
			Debug.Log ("Boosted " + other);
		}
		//Debug.Log ("DIST TO CDI: " + Vector3.Distance (this.transform.position, checkpoints [CDI].transform.position));
		if (Vector3.Distance (this.transform.position, checkpoints [CDI].transform.position) <= deadzone) {
			CDI++;
			MPI++;
			if (CDI >= checkpoints.Capacity) {
				CDI = 0;
			}
			if (MPI >= checkpoints.Capacity) {
				MPI = 0;
			}
		}
		Vector3 velo = new Vector3 (checkpoints [CDI].transform.position.x - checkpoints [MPI].transform.position.x, checkpoints [CDI].transform.position.y - checkpoints [MPI].transform.position.y, checkpoints [CDI].transform.position.z - checkpoints [MPI].transform.position.z);
		this.GetComponent<Rigidbody> ().velocity = (velo * buffer);


	}


	void OnCollisionEnter (Collision _other){
		Debug.Log ("Hello, " + _other.ToString());
		attatched = true;
		other = _other;
	}
	void OnCollisionExit(Collision _other){
		Debug.Log ("Goodbye, " + _other.ToString());
		attatched = false;
	}
}
