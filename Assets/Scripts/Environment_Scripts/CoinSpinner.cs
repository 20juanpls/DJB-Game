using UnityEngine;
using System.Collections;

public class CoinSpinner : MonoBehaviour {

    //Transform Cam;
	GameObject coin;
    public bool X_axis;
    public bool Y_axis;
    public bool Z_axis;
    public bool DoNotHop;

	public float rotateSpeed = 3.0f;
    public float speedOfHop = 1.0f;
    public float IntensityofHop = 10.0f;
    public float LenghtOfHop = 3.0f;

    Vector3 OrigPos;

	// Use this for initialization
	void Start () {
		coin = this.gameObject;

        OrigPos = this.gameObject.GetComponent<Transform>().position;

        rotateSpeed = rotateSpeed * 80.0f;
        
        //Cam = GameObject.Find("Main Camera").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (X_axis == true)
        {
            coin.transform.Rotate(new Vector3(rotateSpeed, 0.0f, 0.0f)* Time.deltaTime);
        }
        else if (Z_axis == true)
        {
            coin.transform.Rotate(new Vector3(0.0f, 0.0f, rotateSpeed)* Time.deltaTime);
        }
        else if (Y_axis == true)
        {
            coin.transform.Rotate(new Vector3(0.0f, rotateSpeed, 0.0f)* Time.deltaTime);
        }
        else {
            coin.transform.Rotate(new Vector3(0.0f, rotateSpeed, 0.0f)* Time.deltaTime);
        }

        if (DoNotHop != true)
        {
            coin.transform.position = new Vector3(0.0f, Mathf.Abs(Mathf.Cos(Time.time * speedOfHop) * (1 / IntensityofHop) * LenghtOfHop), 0.0f) + OrigPos;
        }
        //coin.transform.LookAt(Cam.position);

    }
}
