using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour {

    Rigidbody DirRotT;
    //GameObject Platforms;

    public bool VerticalMovement;
    public float DownAngleLimit;

    public float InitialX_axisSpeed;
    public float Y_axisSpeed;
    public float Z_axisSpeed;

    public float DownMultiplier;

    float CurrentXAxis_Speed, airTime;
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
        if (VerticalMovement)
        {
            airTime += Time.deltaTime;
            CurrentXAxis_Speed = InitialX_axisSpeed + (DownMultiplier * airTime);
            if (DirRotT.rotation.eulerAngles.x <= (DownAngleLimit+360) && CurrentXAxis_Speed < 0.0f)
            {
                VerticalMovement = false;
            }
        }
        else {
            airTime = 0.0f;
            CurrentXAxis_Speed = 0.0f;
        }


        //triggered inclusion
        //Debug.Log(DirRotT.rotation.eulerAngles.x);

        //CurrentXAxis_Speed = InitialX_axisSpeed + (DownMultiplier * airTime);


        eulerAngleDir = new Vector3(CurrentXAxis_Speed, Y_axisSpeed, Z_axisSpeed);
        DirRotT.angularVelocity = eulerAngleDir * Time.deltaTime;
    }
}
