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
    public float CurrentY_axisSpeed;

    public bool LegRoatation, CounterClock;

    public float HalfPeriodTime;
    public float YAnglePos;
    public float YAngleNeg;
    //public bool DoNotHop;

    Vector3 eulerAngleDir;

    float CurrentAngle, YAngP, YAngNeg, Y_Angle, YSpeedCalculated;
    public bool InZone, QuickChange;

    int OutZoneCount;
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
        YSpeedCalculated = (YAnglePos - YAngleNeg) / HalfPeriodTime;

        if (CounterClock)
            YSpeedCalculated = YSpeedCalculated * -1.0f;
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

       // YSpeedCalculated = (YAnglePos - YAngleNeg) / HalfPeriodTime;
       // Debug.Log(YSpeedCalculated);

        if (Y_Angle > YAngNeg || Y_Angle < YAngP)
            InZone = true;
        else
            InZone = false;

        Debug.Log(QuickChange);
        //its stuck on true for somme reason
        if (!InZone)
        {
            QuickChange = true;
            if (OutZoneCount > 0)
                QuickChange = false;
            OutZoneCount++;
        }
        else
            OutZoneCount = 0;

        if (QuickChange)
        {
            YSpeedCalculated = YSpeedCalculated * -1;
        }

        CurrentY_axisSpeed = YSpeedCalculated;//Mathf.Lerp(CurrentY_axisSpeed, YSpeedCalculated, Time.deltaTime);

        eulerAngleDir = new Vector3(X_axisSpeed, CurrentY_axisSpeed, Z_axisSpeed);
        DirRotT.angularVelocity = eulerAngleDir * Time.deltaTime;
        
    }
}
