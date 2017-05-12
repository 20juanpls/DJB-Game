using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMovement : MonoBehaviour {
    Transform TheGem;
    Collider ThisCollider;

    Vector3 StartPosition;
    public float rotateSpeed, speedOfHop, IntensityofHop, LenghtOfHop;
    public bool TinyHop, ColliderOff;

    float CurrTime;
	// Use this for initialization
	void Start () {
        TheGem = this.transform;
        StartPosition = TheGem.position;
        ThisCollider = this.gameObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        TheGem.Rotate(new Vector3(0.0f, rotateSpeed, 0.0f) * Time.deltaTime);

        if (ColliderOff)
            ThisCollider.enabled = false;
        else
            ThisCollider.enabled = true;

        if (TinyHop) {
            CurrTime += Time.deltaTime;
            TheGem.transform.position = new Vector3(0.0f, Mathf.Abs(Mathf.Sin(CurrTime * speedOfHop) * (1 / IntensityofHop) * LenghtOfHop), 0.0f) + StartPosition;
            if (CurrTime >= Mathf.PI) {
                TheGem.transform.position = StartPosition;
                CurrTime = 0.0f;
                TinyHop = false;
            }
        }
    }
}
