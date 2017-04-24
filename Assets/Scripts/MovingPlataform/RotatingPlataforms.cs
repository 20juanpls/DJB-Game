using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlataforms : MonoBehaviour {


    //Transform Cam;
    Rigidbody DirRotT;
    //GameObject Platforms;
    public float X_axisSpeed;
    public float Y_axisSpeed;
    public float Z_axisSpeed;
    //public bool DoNotHop;

    Vector3 eulerAngleDir;

    float CurrentAngle;
    //public float speedOfHop = 1.0f;
    //public float IntensityofHop = 10.0f;
    //public float LenghtOfHop = 3.0f;

    Vector3 OrigPos;

    // Use this for initialization
    void Start()
    {
        DirRotT = this.GetComponent<Rigidbody>();
        OrigPos = this.gameObject.GetComponent<Transform>().position;
        eulerAngleDir = new Vector3(X_axisSpeed, Y_axisSpeed, Z_axisSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        DirRotT.angularVelocity = eulerAngleDir * Time.deltaTime;
    }
}
