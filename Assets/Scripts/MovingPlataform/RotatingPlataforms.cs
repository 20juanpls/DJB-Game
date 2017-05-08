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
    }

    // Update is called once per frame
    void Update()
    {
        //triggered inclusion
        //Debug.Log(DirRotT.rotation.x);
        if (DirRotT.rotation.x * Mathf.Rad2Deg >= 20.0f) {
            //Debug.Log("agh");
            X_axisSpeed = X_axisSpeed * -1;

        }
        if (X_axisSpeed < 0.0f && DirRotT.rotation.x * Mathf.Rad2Deg <= -20.0f) {
            X_axisSpeed = X_axisSpeed * -1;
        }


        eulerAngleDir = new Vector3(X_axisSpeed, Y_axisSpeed, Z_axisSpeed);
        DirRotT.angularVelocity = eulerAngleDir * Time.deltaTime;
    }
}
