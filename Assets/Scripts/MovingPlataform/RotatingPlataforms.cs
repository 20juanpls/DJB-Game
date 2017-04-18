using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlataforms : MonoBehaviour {


    //Transform Cam;
    Transform DirRotT;
    //GameObject Platforms;
    public bool X_axis;
    public bool Y_axis;
    public bool Z_axis;
    //public bool DoNotHop;

    Quaternion RotationTo;
    public float rotateSpeed = 3.0f;

    float CurrentAngle;
    //public float speedOfHop = 1.0f;
    //public float IntensityofHop = 10.0f;
    //public float LenghtOfHop = 3.0f;

    Vector3 OrigPos;

    // Use this for initialization
    void Start()
    {
        //Platforms = this.gameObject;

        DirRotT = this.transform;

        OrigPos = this.gameObject.GetComponent<Transform>().position;

        //Cam = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedCounter();
        if (X_axis == true)
        {
            //DirRotT.Rotate(new Vector3(rotateSpeed, 0.0f, 0.0f) * Time.deltaTime);
            RotationTo = Quaternion.AngleAxis(CurrentAngle, Vector3.right);
        }
        else if (Z_axis == true)
        {
            //DirRotT.Rotate(new Vector3(0.0f, 0.0f, rotateSpeed) * Time.deltaTime);
            RotationTo = Quaternion.AngleAxis(CurrentAngle, Vector3.forward);
        }
        else if (Y_axis == true)
        {
            //DirRotT.Rotate(new Vector3(0.0f, rotateSpeed, 0.0f) * Time.deltaTime);
            RotationTo = Quaternion.AngleAxis(CurrentAngle,Vector3.up);
        }
        else
        {
            //DirRotT.Rotate(new Vector3(0.0f, rotateSpeed, 0.0f) * Time.deltaTime);
            RotationTo = Quaternion.AngleAxis(CurrentAngle, Vector3.up);
        }

        DirRotT.rotation = Quaternion.Slerp(DirRotT.rotation, RotationTo, 1.0f * Time.deltaTime);
    }

    void SpeedCounter() {
        //Debug.Log(CurrentAngle);
        CurrentAngle = CurrentAngle + (Time.deltaTime*rotateSpeed);
        if (CurrentAngle > 360)
            CurrentAngle = 0.0f;

    }
}
