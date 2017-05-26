using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMovement : MonoBehaviour {
    Transform TheGem;
    Collider ThisCollider;

    Vector3 StartPosition;
    public float rotateSpeed, speedOfHop, IntensityofHop, LenghtOfHop, MinFloorDist, DownWardSpeed;
    public bool TinyHop, ColliderOff;

    float CurrTime;
    float FloorDist;
	// Use this for initialization
	void Start () {
        TheGem = this.transform;
        StartPosition = TheGem.position;
        ThisCollider = this.gameObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        TheGem.Rotate(new Vector3(0.0f, rotateSpeed, 0.0f) * Time.deltaTime);
        DownwardRaycast();

        if (ColliderOff)
            ThisCollider.enabled = false;
        else
            ThisCollider.enabled = true;

        if (TinyHop)
        {
            CurrTime += Time.deltaTime;
            TheGem.transform.position = new Vector3(0.0f, Mathf.Abs(Mathf.Sin(CurrTime * speedOfHop) * (1 / IntensityofHop) * LenghtOfHop), 0.0f) + StartPosition;
            if (CurrTime >= Mathf.PI)
            {
                TheGem.transform.position = StartPosition;
                CurrTime = 0.0f;
                TinyHop = false;
            }
        }
        else {
            Debug.Log(FloorDist);
            if (FloorDist >= MinFloorDist) {
                this.transform.Translate(Vector3.down * DownWardSpeed * Time.deltaTime);
            }
        }
    }

    void DownwardRaycast() {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, new Vector3(0.0f, -1.0f, 0.0f), out hit))
        {
            FloorDist = hit.distance;
        }
    }
}
