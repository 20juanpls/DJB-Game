using UnityEngine;
using System.Collections;

public class CoinSpinner : MonoBehaviour {

	GameObject coin;
	public float rotateSpeed;

	// Use this for initialization
	void Start () {
		coin = this.gameObject;
		rotateSpeed = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {

		coin.transform.Rotate (new Vector3 (0.0f, rotateSpeed, 0.0f));

	}
}
