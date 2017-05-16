using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCoinScript : MonoBehaviour {
    public bool Collected;
    GameObject ThePlayer;
    GameObject RuneCoin;
    public float rotateSpeed;
    public float DisappearTime;
    float Counter;
	// Use this for initialization
	void Start () {
        RuneCoin = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Collected)
        {
            RuneCoin.GetComponent<MeshRenderer>().receiveShadows = false;
            RuneCoin.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            RuneCoin.transform.position = ThePlayer.transform.position + (Vector3.up*1.5f) + (Vector3.up*(Mathf.Pow(1000, Counter- 0.7f)));
            RuneCoin.transform.Rotate(new Vector3(0.0f, rotateSpeed * 10.0f, 0.0f) * Time.deltaTime);
            Counter += Time.deltaTime;
            Destroy(RuneCoin,DisappearTime);

        }
        else
        {
            RuneCoin.transform.Rotate(new Vector3(0.0f, rotateSpeed, 0.0f) * Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerMesh")
        {
            Collected = true;
            ThePlayer = other.gameObject;
        }
    }
}
