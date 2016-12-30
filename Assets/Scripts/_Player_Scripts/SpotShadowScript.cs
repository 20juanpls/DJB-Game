using UnityEngine;
using System.Collections;

public class SpotShadowScript : MonoBehaviour {
    PlayerMovement_Ver2 PlayerScr;
    Rigidbody PlayerPos;

    Transform SpotShadowT;
    Quaternion ShadowRot;

    float ShadowDist;
	
    // Use this for initialization
	void Start () {
        SpotShadowT = this.gameObject.transform;
        PlayerPos = GameObject.Find("Player").GetComponent<Rigidbody>();
        PlayerScr = GameObject.Find("Player").GetComponent<PlayerMovement_Ver2>();

        SpotShadowT.rotation = Quaternion.Euler(-90.0f,0.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
        SpotShadowT.position = new Vector3(PlayerPos.position.x, PlayerPos.position.y - PlayerScr.floorDist + 0.1f, PlayerPos.position.z);
	
	}
}
