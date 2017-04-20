using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedcubetest : MonoBehaviour {
    public GameObject followThis;
    Rigidbody CenterOfMass;

    public float Speed = 5.0f;
    public float SwitchTime = 1.0f;
    public float LerpSpeed = 1.0f;

    public bool MoveWithFollowThis;
    public bool UpDOwn;

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

            CenterOfMass.velocity = Vector3.Lerp(CenterOfMass.velocity, Direction*Speed, LerpSpeed * Time.deltaTime);

            if (TimeSwitch < 0.0f)
            {
                TimeSwitch = SwitchTime;
                Speed = Speed * -1;
            }

            TimeSwitch = TimeSwitch - Time.deltaTime;
            //Debug.Log(MoveToPos);
            //this.transform.Translate(Vector3.right * Speed * Time.deltaTime);
            //this.transform.position = MoveToPos + origPos;
            //CenterOfMass.position = Vector3.Lerp(CenterOfMass.position, MoveToPos + origPos, LerpSpeed * Time.deltaTime);

            //Debug.Log(velocity);
            //Debug.Log((transform.position - previous)/Time.deltaTime);
            /*if (previous != Vector3.zero)
            {
                VeloZ = (transform.position - previous) / Time.deltaTime;
                velocity = VeloZ.magnitude;
            }
            previous = transform.position;*/

            //Debug.DrawRay(transform.position, VeloZ, Color.red);
            //this.transform.position = Vector3.Slerp(this.transform.position, )
        }
    }
}
