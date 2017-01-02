using UnityEngine;
using System.Collections;

public class LeaveMovement : MonoBehaviour {
    public float speedOfShake;
    public float Intensity;
    Vector3 OrigPos;
    Transform ThisObjectTrans;
	// Use this for initialization
	void Start () {
        ThisObjectTrans = this.transform;
        OrigPos = ThisObjectTrans.position;
	}
	
	// Update is called once per frame
	void Update () {
        ThisObjectTrans.position = new Vector3(Mathf.Sin(Time.time * speedOfShake)*(1/Intensity), Mathf.Cos(Time.time * speedOfShake) * (1 / Intensity), Mathf.Cos(Time.time * speedOfShake)*(1 / Intensity)) + OrigPos;
    }
}
