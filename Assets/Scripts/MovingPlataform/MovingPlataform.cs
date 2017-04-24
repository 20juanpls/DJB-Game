using UnityEngine;
using System.Collections;

public class MovingPlataform : MonoBehaviour {
    Transform thisPlataform;
    Vector3 originalPos, thisForward, thisUp, HorizVel, VertVel, newPosition;
    public bool forwardOrBack, topOrBottom;
    public bool IsitBehind, IsitBelow;
    public float HorizMoveSpeed = 100.0f; 
    public float VertMoveSpeed = 100.0f;
    public float limDistance = 10.0f;
	// Use this for initialization
	void Start () {
        thisPlataform = this.GetComponent<Transform>();
        originalPos = thisPlataform.position;

        //HorizMoveSpeed = HorizMoveSpeed * (1/Time.deltaTime);
       // VertMoveSpeed = VertMoveSpeed * (1 / Time.deltaTime);
        //forwardOrBack = false;
        //topOrBottom = false;
        //false for back and bottom ... true for forward and top...
    }
	void Update () {
        //Debug.Log(Vector3.Distance(thisPlataform.position, originalPos));
        HorizwhereIsIt();
        VertwhereIsIt();

        ForwardToBack();
         TopToBottom();
        //thisPlataform.position = ((HorizVel + VertVel)*Time.deltaTime)+originalPos;
        newPosition = new Vector3(0.0f,(Mathf.Cos(Time.time * HorizMoveSpeed) * (1 / limDistance) * VertMoveSpeed), 0.0f) + originalPos;

        thisPlataform.position = Vector3.Lerp(thisPlataform.position, newPosition, 1.0f * Time.deltaTime);

    }

    void ForwardToBack() {
        if (forwardOrBack == false)
        {
            HorizVel = thisPlataform.rotation * Vector3.forward * -1 * HorizMoveSpeed;
            if (Vector3.Distance(thisPlataform.position, originalPos) >= limDistance && IsitBehind == false)
            {
                HorizVel = Vector3.forward * 0.0f;
                forwardOrBack = true;
            }
        }
        else if (forwardOrBack == true)
        {
            HorizVel = thisPlataform.rotation * Vector3.forward * HorizMoveSpeed;
            if (Vector3.Distance(thisPlataform.position, originalPos) >= limDistance && IsitBehind == true)
            {
                HorizVel = Vector3.forward * 0.0f;
                forwardOrBack = false;
            }
        }
    }

    void TopToBottom() {
        if (topOrBottom == false)
        {
            VertVel = thisPlataform.rotation * Vector3.up * -1 * VertMoveSpeed;
            if (Vector3.Distance(thisPlataform.position, originalPos) >= limDistance && IsitBelow == false)
            {
                VertVel = Vector3.up * 0.0f;
                topOrBottom = true;
            }
        }
        else if (topOrBottom == true)
        {
            VertVel = thisPlataform.rotation * Vector3.up * VertMoveSpeed;
            if (Vector3.Distance(thisPlataform.position, originalPos) >= limDistance && IsitBelow == true)
            {
                VertVel = Vector3.up * 0.0f;
                topOrBottom = false;
            }
        }
    }

    void HorizwhereIsIt() {
        thisForward = thisPlataform.rotation * Vector3.forward;
        Vector3 toPlata2 = originalPos - thisPlataform.transform.position;
        if (Vector3.Dot(thisForward, toPlata2) < 0)
        {
            IsitBehind = true;
        }
        else if (Vector3.Dot(thisForward, toPlata2) > 0)
        {
            IsitBehind = false;
        }
    }

    void VertwhereIsIt() {
        thisUp = thisPlataform.rotation * Vector3.up;
        Vector3 toPlata = originalPos - thisPlataform.transform.position;
        if (Vector3.Dot(thisUp, toPlata) < 0)
        {
            IsitBelow = true;
        }
        else if (Vector3.Dot(thisUp, toPlata) > 0)
        {
            IsitBelow = false;
        }
    }
}
