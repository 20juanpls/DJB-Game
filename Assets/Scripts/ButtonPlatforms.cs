using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlatforms : MonoBehaviour {
    Transform ThisPlatform;
    public GameObject DesignatedButton, TGoalPos;
    public float PlatSpeed;
    public bool UsingB_Goal;
    bool ActivatePlat;

    Vector3 OrigPos, GoalPos;
	// Use this for initialization
	void Start () {
        if (TGoalPos != null)
        {
            OrigPos = this.transform.position;
            GoalPos = TGoalPos.transform.position;
            ThisPlatform = this.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (DesignatedButton != null)
        {
            if (UsingB_Goal)
                ActivatePlat = DesignatedButton.GetComponent<BoulderGoalScript>().B_GOAL;
            else
                ActivatePlat = DesignatedButton.GetComponent<ButtonScript>().ButtonActive;

            if (ActivatePlat)
            {
                ThisPlatform.position = Vector3.MoveTowards(ThisPlatform.position, GoalPos, PlatSpeed * Time.deltaTime);
            }
            else
            {
                ThisPlatform.position = Vector3.MoveTowards(ThisPlatform.position, OrigPos, PlatSpeed * Time.deltaTime);
            }
        }
        else
            Debug.Log("no button in:" +this.gameObject.name+"!");
	}
}
