using UnityEngine;
using System.Collections;

public class Hopping : MonoBehaviour {
	bool canIjump;
	public float gravypull = 0.2f;
	private float oldgravypull;
	public float jumpSpeed = 0.25f;
	public float jumpAccel = 0.1f;
	private float oldjumpSpeed;
	float yAccel;
	GameObject floor;
	Vector3 posRun;
	Vector3 posFloor;
	float floorDist = 0;

	// Use this for initialization
	void Start () {
		canIjump = false;
		floor = GameObject.FindGameObjectWithTag ("plane");
		posRun = this.transform.position;
		posFloor = floor.transform.position;
		oldgravypull = gravypull;
		oldjumpSpeed = jumpSpeed;
		yAccel = 0.0f;
	}
	// Update is called once per frame
	void Update () {
		if (canIjump == true) {
			yAccel += jumpSpeed;
		}
		yAccel -= gravypull;
		this.transform.Translate(new Vector3 (0.0f, yAccel, 0.0f) );

		if (floorDist == 0) {
			yAccel = 0.0f;
			canIjump = true;
		}
	}
	void FloorMeasure()
	{
		
		RaycastHit hit;
		
		if (Physics.Linecast(posRun, posFloor, out hit))
		{
			floorDist = hit.distance;
		}
	}
}
