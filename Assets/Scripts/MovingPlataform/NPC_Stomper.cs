using UnityEngine;
using System.Collections;

public class NPC_Stomper : MonoBehaviour {
    RelativGrav thisGrav;
    Rigidbody rb;
    GameObject prb;

    public float ThisMaxHeight = 1.0f;
    public float CloseDistToSmash = 7.0f;
    public float UpSpeed = 0.3f;
    private float CloseDist, CurrentUpSpeed;
    private Vector3 thisPos;
    private bool DistReader,translateUpwards, IsUpThere;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
        prb = GameObject.Find("Player");
        thisGrav = this.GetComponent<RelativGrav>();
        DistReader = true;
        CurrentUpSpeed = UpSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        thisPos = rb.transform.position;
        CloseDist = Vector3.Distance(new Vector3(thisPos.x, 0.0f, thisPos.z), new Vector3(prb.transform.position.x, 0.0f, prb.transform.position.z));
        //Debug.Log(CloseDist);

        if (thisGrav.floorDist >= ThisMaxHeight)
        {
            IsUpThere = true;
        }
        else {
            IsUpThere = false;
        }

        if (CloseDist <= CloseDistToSmash && DistReader == true)
        {
                thisGrav.setFallAcceleration(thisGrav.fallAccel);
                if (thisGrav.IsItGrounded() == true)
                {
                    DistReader = false;
                }
         }
         else {
                thisGrav.setFallAcceleration(0.0f);
         }
        Debug.Log(translateUpwards);
        if (DistReader == false) {
            StartCoroutine(WaitingForNextAttack(3.0f));
        }
        if (translateUpwards == true) {
            if (IsUpThere == true)
            {
                CurrentUpSpeed = 0.0f;
                DistReader = true;
                translateUpwards = false;
            }
            else {
                CurrentUpSpeed = UpSpeed;

            }
            rb.transform.Translate(Vector3.up * CurrentUpSpeed);
        }
	
	}

    IEnumerator WaitingForNextAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        translateUpwards = true;
    }
}
