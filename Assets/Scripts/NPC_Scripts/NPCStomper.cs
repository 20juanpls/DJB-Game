using UnityEngine;
using System.Collections;

public class NPCStomper : MonoBehaviour {
    Rigidbody ThisStompRb;
    Transform PlayerPos;
    public Vector3 OrigStompPos;

    public bool touching;
    public float fallAccel;
    public float initialAirSpeed;
    public float MinPlayerDist;
    public float OrigTimeOnGround;
    public float OrigTimeOnAir;
    public float OrigWarnTime;
    public float CurrentTimeOnGround;
    public float CurrentTimeOnAir;
    public float CurrentWarnTime;
    public float RecoverySpeed;
    public float SpeedOfWarn;
    public float IntensityOfWarn;
    public float LenghtOfWarnShake;

    
    private float airTime;
    private float Distance;
    private Vector3 PlayerDist;
    private float currentfallSpeed;
    public bool CanFall, Recover, Warned;

	// Use this for initialization
	void Start () {
        PlayerPos = GameObject.Find("Player").GetComponent<Transform>();
        ThisStompRb = this.GetComponent<Rigidbody>();
        OrigStompPos = ThisStompRb.transform.position;
        CurrentTimeOnGround = OrigTimeOnGround;
        CurrentTimeOnAir = OrigTimeOnAir;
        CurrentWarnTime = OrigWarnTime;

        //ayy not yeet;
        //CanFall = true;

    }
    public void AssignPlayer(GameObject p)
    {
        PlayerPos = p.transform;
       // Debug.Log(Player.ToString());
    }

    // Update is called once per frame
    void Update() {

        Distance = Vector3.Distance(new Vector3(PlayerPos.position.x, 0.0f, PlayerPos.position.z), new Vector3(OrigStompPos.x, 0.0f, OrigStompPos.z));

        if (Distance <= MinPlayerDist)
        {
            if (Warned == false)
            {
                CurrentWarnTime -= Time.deltaTime;
                if (CurrentWarnTime >= 0.0f)
                {
                    Warned = false;
                    ThisStompRb.position = new Vector3(0.0f, Mathf.Sin(Time.time * SpeedOfWarn) * (1 / IntensityOfWarn) * LenghtOfWarnShake, 0.0f) + OrigStompPos;
                }
                else if (CurrentWarnTime <= 0.0f)
                {
                    Warned = true;
                    ThisStompRb.velocity = Vector3.MoveTowards(OrigStompPos, ThisStompRb.position, RecoverySpeed);
                }
            }
            else {
                CurrentWarnTime = OrigWarnTime;
            }

            if (Recover == false && Warned == true)
            {
                CanFall = true;
                CurrentWarnTime = OrigWarnTime;
            }
        }

        if (CanFall == true) {
            currentfallSpeed = initialAirSpeed + (fallAccel * airTime);

            ThisStompRb.velocity = new Vector3(0.0f, currentfallSpeed, 0.0f);
            airTime += Time.deltaTime;
            CurrentTimeOnGround = OrigTimeOnGround;
            CurrentTimeOnAir = OrigTimeOnAir;
        }

        if (touching == true){
            Recover = true;
            CanFall = false;
            airTime = 0.0f;
        }
        if (Recover == true)
        {
            ThisStompRb.velocity = Vector3.zero;
            CurrentTimeOnGround -= Time.deltaTime;
            if (CurrentTimeOnGround <= 0.0f)
            {
                ThisStompRb.velocity = Vector3.MoveTowards(OrigStompPos, ThisStompRb.position, RecoverySpeed);
                if (ThisStompRb.position.y >= OrigStompPos.y) {
                    CurrentTimeOnAir -= Time.deltaTime;
                    ThisStompRb.velocity = Vector3.zero;
                    if (CurrentTimeOnAir <= 0.0f) {
                        Recover = false;
                        Warned = false;
                    }
                }
            }
        }
	
	}

    void OnCollisionEnter(Collision collision){
        //Debug.Log (collision.relativeVelocity);
        if (collision.gameObject.name != "Player")
        {
            touching = true;
        }
    }

    void OnCollisionExit(Collision collision){
        if (collision.gameObject.name != "Player")
        {
            touching = false;
        }
    }
}
