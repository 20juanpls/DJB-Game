using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderGoalScript : MonoBehaviour {
    GameObject ThePlayer;
    public bool B_GOAL;
    bool BoulderIn;
	// Use this for initialization
	void Start () {
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");
    }
    void FindThePlayer()
    {
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");
    }

    // Update is called once per frame
    void Update () {
        if (BoulderIn)
            B_GOAL = true;

        if (ThePlayer == null)
        {
            B_GOAL = false;
            BoulderIn = false;
            FindThePlayer();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boulder")
            BoulderIn = true;
    }
}
