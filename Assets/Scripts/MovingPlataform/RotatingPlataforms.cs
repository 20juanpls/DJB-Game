using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlataforms : MonoBehaviour {


    //Transform Cam;
    Transform TransRotT;
    Rigidbody DirRotT;
    //GameObject Platforms;
    public float X_axisSpeed;
    public float Y_axisSpeed;
    public float Z_axisSpeed;

    public bool LegRoatation;

    public float YAnglePos;
    public float YAngleNeg;
    //public bool DoNotHop;

    Vector3 eulerAngleDir;

    float CurrentAngle, YAngP, YAngNeg, Y_Angle;
    bool InZone, QuickChange;
    //public float speedOfHop = 1.0f;
    //public float IntensityofHop = 10.0f;
    //public float LenghtOfHop = 3.0f;

    Vector3 OrigPos;

    // Use this for initialization
    void Start()
    {
        TransRotT = this.GetComponent<Transform>();
        DirRotT = this.GetComponent<Rigidbody>();
        OrigPos = this.gameObject.GetComponent<Transform>().position;
        YAngP = YAnglePos;
        YAngNeg = YAngleNeg + 360;
    }

    // Update is called once per frame
    void Update()
    {
        if (LegRoatation)
        {
            TheLegRotation();
        }
        else
        {
            eulerAngleDir = new Vector3(X_axisSpeed, Y_axisSpeed, Z_axisSpeed);
            DirRotT.angularVelocity = eulerAngleDir * Time.deltaTime;
        }
    }

    void TheLegRotation() {

        Y_Angle = TransRotT.localEulerAngles.y;

        Debug.Log(Y_Angle);

        if (Y_Angle >= 300 || Y_Angle <= 30)
            InZone = true;
        else
            InZone = false;

        if (!InZone)
            QuickChange = true;

        if (QuickChange)
        {
            Y_axisSpeed = Y_axisSpeed * -1;
            QuickChange = false;
        }


        /*if (DirRotT.rotation.eulerAngles.y <= 320.0f && Y_axisSpeed < 0.0f)
            Y_axisSpeed = Y_axisSpeed * -1;
        else if (DirRotT.rotation.eulerAngles.y >= 30.0f && Y_axisSpeed > 0.0f)
            Y_axisSpeed = Y_axisSpeed * -1;*/
        //if (DirRotT.rotation.eulerAngles.y >= YAnglePos || DirRotT.rotation.eulerAngles.y <= YAngleNeg)
        //    Y_axisSpeed = Y_axisSpeed * -1;
        //else if (DirRotT.rotation.eulerAngles.y <= YAngleNeg)
        //    Y_axisSpeed = Y_axisSpeed * -1;





        eulerAngleDir = new Vector3(X_axisSpeed, Y_axisSpeed, Z_axisSpeed);
        DirRotT.angularVelocity = eulerAngleDir * Time.deltaTime;
    }
}
