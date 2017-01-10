﻿using UnityEngine;
using System.Collections;

public class NPC_Follow : MonoBehaviour {

    ColliderIndicator ColIn;
	NPC_Death TheDeath;
    Rigidbody rb;
	GameObject prb;
    GameObject ChilDCollin;
    Transform SpikesChild;

    private float distance = 100.0f;
	public float minDistance = 20.0f;
	public float atkDistance = 10.0f;
    public float RotationSpeed = 2.0f;
	//Around ~25 seems to be good
	public float atkForce = 5000.0f;
    private float OldAtkForce, LedgeDist, OldRotationSpeed,spikeSpeed, CurrentSpikeSpeed;

    private bool IsitBehindMe, InHeightRange, playerFound, isActive, AreSpikesBehindMe;

    public bool Death;

    Vector3 thisForward, originalSpikePosition;

    public Vector3 OrigNPCPos, OrigNPCScale;
    public Quaternion OrigNPCrot;

    // Use this for initialization
    void Start () {
        
		rb = this.GetComponent<Rigidbody> ();
        prb = GameObject.Find ("Player");
		TheDeath = this.gameObject.GetComponent<NPC_Death> ();
        //ColIn = GameObject.Find("NPC_Collider").GetComponent<ColliderIndicator>();
        ColIn = this.gameObject.transform.GetChild(1).GetComponent<ColliderIndicator>();
        // ChilDCollin = GameObject.Find("NPC_Collider");
        ChilDCollin = this.gameObject.transform.GetChild(1).gameObject;

        SpikesChild = this.gameObject.transform.GetChild(0);

         OldAtkForce = atkForce;
        OldRotationSpeed = RotationSpeed;
        spikeSpeed = 0.3f;
        CurrentSpikeSpeed = spikeSpeed;

        OrigNPCPos = rb.transform.position;
        OrigNPCScale = rb.transform.localScale;
        OrigNPCrot = rb.transform.rotation;


    }

	//ANother Noah Squeeze
	public void AssignPlayer(GameObject p){
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        TheDeath.NPCIsDead = false;
        prb = p;
        rb.transform.position = OrigNPCPos;
        rb.transform.localScale = OrigNPCScale;
        rb.transform.rotation = OrigNPCrot;
    }

	// Update is called once per frame
	void Update () {

        WhereIsIt();
        RayForMeasure();
        //Debug.Log(LedgeDist);
        ActiveSpikeSetter();

        distance = Vector3.Distance (rb.transform.position, prb.transform.position);
        ChilDCollin.SetActive(isActive);

        //if distance between player and npc is less than mindistance...
        if (distance <= minDistance) {

            //npc looks at player 
			if (rb.velocity == Vector3.zero)
            {
                //ChilDCollin.SetActive(false);
                rb.transform.rotation = Quaternion.Slerp(
                    rb.transform.rotation,
                    Quaternion.LookRotation(new Vector3(prb.transform.position.x, 0.0f, prb.transform.position.z) - new Vector3(rb.transform.position.x, 0.0f, rb.transform.position.z)),
                    Time.deltaTime * RotationSpeed);
            }

			//if player is being watched and gets too close...
			if (distance <= atkDistance) {
				rb.AddForce (rb.transform.forward * atkForce);
			}
            //Debug.Log(atkForce);
            if (rb.velocity.magnitude > 1.0f)
            {
                isActive = true;
            }
            else if (rb.velocity.magnitude < 1.0f) {
                isActive = false;
            }
		}

        if (distance <= 4.0f && atkForce == 0.0f)
            {
                isActive = true;
            }
            else if (atkForce == 0.0f)
            {
                isActive = false;
            }
       
		if (TheDeath.NPCIsDead == true) {
			isActive = false;
		}

        if (ColIn.AmIHitting == true) {
                StartCoroutine(WaitingForNextAttack(2.0f));
            }

		//Debug.Log(ColIn.IsHitting());

		if (LedgeDist >= 2.15f && LedgeDist <= 2.25f)//(LedgeDist >= 1.95f && LedgeDist <= 2.05f)<-- FOR ORIGINAL CUBE
        {
            InHeightRange = true;
        }
        else {
            InHeightRange = false;
        }
        if (InHeightRange == false || IsitBehindMe == true)
        {
            //StartCoroutine(WaitingForNextAttack(0.1f));
            atkForce = 0.0f;
            //StartCoroutine(WaitingToTurn(0.1f));
        }
        else if (playerFound == true)
        {
               atkForce = OldAtkForce;
        }

        Physics.gravity = new Vector3(0.0f, -30.0f, 0.0f);
    }
    IEnumerator WaitingForNextAttack( float waitTime) {
		//if (Time.deltaTime>= 1.0f)
		//	Debug.Log (Time.deltaTime);
        atkForce = 0.0f;
        yield return new WaitForSeconds(waitTime);
        ColIn.AmIHitting = false;

        //atkForce = 0.0f;

    }
    IEnumerator WaitingToTurn(float waitTime) {
        RotationSpeed = 0.0f;
        yield return new WaitForSeconds(waitTime);
        RotationSpeed = OldRotationSpeed;
    }
    void WhereIsIt() {
        thisForward = rb.transform.rotation*Vector3.forward;
        Vector3 toPlayer = prb.transform.position - rb.transform.position;
        if (Vector3.Dot(thisForward, toPlayer) < 0)
        {
            IsitBehindMe = true;
        }
        else {
            IsitBehindMe = false;
        }

    }
    void RayForMeasure(){
        RaycastHit hit;
        RaycastHit hitdos;
		//Debug.Log (LedgeDist);
        Debug.DrawRay(new Vector3((thisForward.x*4.0f) + rb.transform.position.x, rb.transform.position.y + 1.0f, (thisForward.z*4.0f) + rb.transform.position.z), Vector3.down * 2.5f, Color.green);
        //Debug.DrawRay(rb.transform.position, rb.transform.rotation*Vector3.forward*minDistance, Color.red);
        if (Physics.Raycast(new Vector3 (rb.transform.position.x, rb.transform.position.y, rb.transform.position.z), rb.transform.rotation * Vector3.forward, out hitdos))
        {
            if (hitdos.transform.tag == "PlayerMesh")
            {
                //Debug.Log("Found Player!");
                playerFound = true;
            }
            else
            {
                playerFound = false;
            }
        }

        if (Physics.Raycast(new Vector3((thisForward.x * 4.0f) + rb.transform.position.x, rb.transform.position.y + 1.0f, (thisForward.z * 4.0f) + rb.transform.position.z), Vector3.down * 2.5f, out hit))
        {
            if (hit.transform.tag != "PlayerMesh") {
                LedgeDist = hit.distance;
            }
        }
    } 
    void ActiveSpikeSetter(){
        //thisForward = rb.transform.rotation * Vector3.forward;
        Vector3 toSpikes = SpikesChild.transform.position - rb.transform.position;
        if (Vector3.Dot(thisForward, toSpikes) > 0)
        {
            AreSpikesBehindMe = false;
        }
        else
        {
            AreSpikesBehindMe = true;
        }

        //Debug.Log(Vector3.Distance(SpikesChild.transform.position, rb.transform.position));
        if (isActive == true)
        {
            //new Vector3((thisForward.x * 5.0f) + rb.transform.position.x, rb.transform.position.y + 1.0f, (thisForward.z * 5.0f) + rb.transform.position.z)
            SpikesChild.Translate(Vector3.forward * CurrentSpikeSpeed, Space.Self);
            if (Vector3.Distance(SpikesChild.transform.position, rb.transform.position) >= 0.9f)
            {
                CurrentSpikeSpeed = 0.0f;
            }
            else {
                CurrentSpikeSpeed = spikeSpeed;
            }
        }
        else if (isActive == false)
        {
            SpikesChild.Translate(Vector3.forward * CurrentSpikeSpeed*-1, Space.Self);
            if (Vector3.Distance(SpikesChild.transform.position, prb.transform.position) >= 0.9f && AreSpikesBehindMe == false)
            {
                CurrentSpikeSpeed = spikeSpeed;
            }
            else
            {
                CurrentSpikeSpeed = 0.0f;
            }

        }
    }
}
