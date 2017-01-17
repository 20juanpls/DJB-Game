using UnityEngine;
using System.Collections;

public class SpotShadowScript : MonoBehaviour {
    PlayerMovement_Ver2 PlayerScr;
    Transform PlayerPos;

    Transform SpotShadowT;
    Quaternion ShadowRot;

    float ShadowDist;
	
    // Use this for initialization
	void Start () {
        SpotShadowT = this.gameObject.transform;
        PlayerPos = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        PlayerScr = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<PlayerMovement_Ver2>();

        SpotShadowT.rotation = Quaternion.Euler(-90.0f,0.0f,0.0f);
	}

    // Update is called once per frame
    void Update () {
        Debug.Log(PlayerPos);
        SpotShadowT.position = new Vector3(PlayerPos.position.x, PlayerPos.position.y - PlayerScr.floorDist + 0.1f, PlayerPos.position.z);
	
	}

    public void ResetShadow( GameObject _p)
    {
        SpotShadowT = this.gameObject.transform;
        PlayerPos = _p.GetComponent<Transform>();
        Debug.Log(PlayerPos);
        PlayerScr = _p.GetComponent<PlayerMovement_Ver2>();

        SpotShadowT.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
    }
}
