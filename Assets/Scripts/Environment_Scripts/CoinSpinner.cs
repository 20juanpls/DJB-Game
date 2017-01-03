using UnityEngine;
using System.Collections;

public class CoinSpinner : MonoBehaviour {

    //Transform Cam;
	GameObject coin;
	public float rotateSpeed = 3.0f;
    public float speedOfHop = 1.0f;
    public float IntensityofHop = 10.0f;
    public float LenghtOfHop = 3.0f;

    Vector3 OrigPos;

	// Use this for initialization
	void Start () {
		coin = this.gameObject;

        OrigPos = this.gameObject.GetComponent<Transform>().position;
        //rotateSpeed = 3.0f;
        //Cam = GameObject.Find("Main Camera").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

        coin.transform.Rotate (new Vector3 (0.0f, rotateSpeed, 0.0f));
        coin.transform.position = new Vector3(0.0f, Mathf.Abs(Mathf.Cos(Time.time * speedOfHop) * (1 / IntensityofHop)*LenghtOfHop),0.0f) + OrigPos;
        //coin.transform.LookAt(Cam.position);

    }
}
