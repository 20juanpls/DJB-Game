using UnityEngine;
using System.Collections;

public class MovingPlataform : MonoBehaviour {
    Rigidbody thisPlataform;
    Vector3 originalPos, thisForward;
    bool forwardOrBack, IsitBehind;
    public float MoveForce = 100.0f;
    public float limDistance = 10.0f;
	// Use this for initialization
	void Start () {
        thisPlataform = this.GetComponent<Rigidbody>();
        originalPos = thisPlataform.position;
        forwardOrBack = false;
        //false for back ... true for forward...
	}
	void Update () {
        //Debug.Log(Vector3.Distance(thisPlataform.position, originalPos));
        whereIsIt();

        Debug.Log(forwardOrBack);

        if (forwardOrBack == false) {
            thisPlataform.AddRelativeForce(Vector3.forward *-1*MoveForce);
            if (Vector3.Distance(thisPlataform.position, originalPos) >= limDistance && IsitBehind == false ) {
                thisPlataform.AddRelativeForce(Vector3.forward * 0.0f);
                forwardOrBack = true;
            }
        }
        else if (forwardOrBack == true)
            {
                thisPlataform.AddRelativeForce(Vector3.forward * MoveForce);
            if (Vector3.Distance(thisPlataform.position, originalPos) >= limDistance && IsitBehind == true){
                    thisPlataform.AddRelativeForce(Vector3.forward * 0.0f);
                forwardOrBack = false;
                }
            }

    }
    void whereIsIt() {
        thisForward = thisPlataform.rotation * Vector3.forward;
        Vector3 toPlata = originalPos - thisPlataform.transform.position;
        if (Vector3.Dot(thisForward, toPlata) < 0)
        {
            IsitBehind = true;
        }
        else
        {
            IsitBehind = false;
        }
    }
}
