using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedcubetest : MonoBehaviour {
    public GameObject followThis;
    GameObject BottomFace;
    Rigidbody CenterOfMass;

    public float Speed = 5.0f;
    public float SwitchTime = 1.0f;
    public float LerpSpeed = 1.0f;

    public bool MoveWithFollowThis, harshmovement;
    public bool UpDOwn, dontRotate;

    float TimeSwitch;
    Vector3 MoveToPos;
    Vector3 origPos;
    Vector3 Direction;


    Vector3 previous;
    Vector3 VeloZ;
    public float velocity;
	// Use this for initialization
	void Start () {
        if (this.GetComponent<Rigidbody>() != null)
            CenterOfMass = this.GetComponent<Rigidbody>();
        TimeSwitch = SwitchTime;
        origPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (MoveWithFollowThis)
        {
            this.transform.position = followThis.transform.position;
            if (!dontRotate)
                this.transform.rotation = followThis.transform.rotation;
            //this.transform.position = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>().position + Vector3.up*4.0f;
        }
        else
        {
            //Debug.Log(CenterOfMass.velocity.magnitude);

            //MoveToPos += Vector3.right * Speed * Time.deltaTime;
            //CenterOfMass.velocity = Vector3.right*Speed;
            if (UpDOwn)
                Direction = Vector3.up;
            else
                Direction = Vector3.right;

            if (harshmovement)
                CenterOfMass.velocity = Direction * Speed;//Vector3.Lerp(CenterOfMass.velocity, Direction*Speed, LerpSpeed * Time.deltaTime);
            else
                CenterOfMass.velocity = Vector3.Lerp(CenterOfMass.velocity, Direction*Speed, LerpSpeed * Time.deltaTime);

            if (TimeSwitch < 0.0f)
            {
                TimeSwitch = SwitchTime;
                Speed = Speed * -1;
            }

            TimeSwitch = TimeSwitch - Time.deltaTime;
        }
    }

}
