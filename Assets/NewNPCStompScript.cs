using UnityEngine;
using System.Collections;

public class NewNPCStompScript : MonoBehaviour
{
    Rigidbody ThisStompRb;
    GameObject Player, BottomPlane;
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
    private float Distance, GroundDist;
    private Vector3 PlayerDist, BottomPlanePos;
    private float currentfallSpeed;
    public bool CanFall, Recover, Warned;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        PlayerPos = Player.GetComponent<Transform>();
        BottomPlane = transform.parent.FindChild("MovingPlat").transform.FindChild("ColliderForStompEnemy").transform.FindChild("BottomPlane").gameObject;
        Debug.Log(BottomPlane);
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
    void Update()
    {

        Distance = Vector3.Distance(new Vector3(PlayerPos.position.x, 0.0f, PlayerPos.position.z), new Vector3(OrigStompPos.x, 0.0f, OrigStompPos.z));

        UpdateCurrentTouchingGround();


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
            else
            {
                CurrentWarnTime = OrigWarnTime;
            }

            if (Recover == false && Warned == true)
            {
                CanFall = true;
                CurrentWarnTime = OrigWarnTime;
            }
        }

        if (CanFall == true)
        {
            currentfallSpeed = initialAirSpeed + (fallAccel * airTime);

            ThisStompRb.velocity = new Vector3(0.0f, currentfallSpeed, 0.0f);
            airTime += Time.deltaTime;
            CurrentTimeOnGround = OrigTimeOnGround;
            CurrentTimeOnAir = OrigTimeOnAir;
        }

        //Debug.Log(touching);
        if (touching == true)
        {
            Recover = true;
            CanFall = false;
            airTime = 0.0f;
        }
        if (Recover == true)
        {
            ThisStompRb.velocity = Vector3.zero;
            CurrentTimeOnGround -= Time.deltaTime;
            touching = false;
            if (CurrentTimeOnGround <= 0.0f)
            {
                //ThisStompRb.position = Vector3.MoveTowards(ThisStompRb.position, OrigStompPos, RecoverySpeed *Time.deltaTime);
                ThisStompRb.velocity = new Vector3(0.0f, RecoverySpeed, 0.0f);
                //Debug.Log(currentfallSpeed);
                if (ThisStompRb.position.y >= OrigStompPos.y)
                {
                    CurrentTimeOnAir -= Time.deltaTime;
                    ThisStompRb.velocity = Vector3.zero;
                    if (CurrentTimeOnAir <= 0.0f)
                    {
                        Recover = false;
                        Warned = false;
                    }
                }
            }
        }

    }

    void UpdateCurrentTouchingGround(){
        BottomPlanePos = BottomPlane.transform.position;

        RaycastHit hit;
        if (Physics.Raycast(BottomPlanePos, Vector3.down, out hit)) {
            if (hit.transform.tag != "PlayerMesh") {
                GroundDist = hit.distance;
            }
        }

        if (GroundDist <= 0.5f)
            touching = true;
        else
            touching = false;
    }
}

