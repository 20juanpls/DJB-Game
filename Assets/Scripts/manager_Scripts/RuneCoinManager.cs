using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCoinManager : MonoBehaviour {
    public GameObject EndCollectable, TheCollect;
    GameObject CManager, ThePlayer;
    public bool MovementPause, MajorCollectableGet;
    public int TotalRuneCoinCount;
    public int CoinsCollected;
    public float TimeLenght;

    int WinCount;

	// Use this for initialization
	void Start () {
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");
        CManager = this.gameObject;
        if (CManager.transform.childCount > 0)
        {
            for (int j = 0; j < CManager.transform.childCount; j++) {
                TotalRuneCoinCount++;
            }
        }
        else {
            Debug.Log("No Special Coins!!!");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (ThePlayer == null)
            ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");

        CoinsCollected = TotalRuneCoinCount - CManager.transform.childCount;
        if (CManager.transform.childCount <= 0 && WinCount < 1)
        {
            MajorCollectableGet = true;
            WinCount++;
            TheCollect = Instantiate(EndCollectable, ThePlayer.transform.position + (Vector3.up * 4.0f), ThePlayer.transform.rotation);
            TheCollect.GetComponent<GemMovement>().ColliderOff = true;
            TheCollect.GetComponent<GemMovement>().TinyHop = true;
        }

        if (MajorCollectableGet)
        {
            TimeLenght -= Time.deltaTime;
            if (TimeLenght <= 0.0f)
            {
                TheCollect.GetComponent<GemMovement>().ColliderOff = false;
                MajorCollectableGet = false;
            }
        }

        if (MajorCollectableGet)
            MovementPause = true;
        else
            MovementPause = false;

        /*if (Input.GetKeyDown(KeyCode.E))
        {
            if (!MovementPause)
                MovementPause = true;
            else
                MovementPause = false;
        }*/
           
    }
}
