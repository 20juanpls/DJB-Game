using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidActivatorButton : MonoBehaviour {
	Transform ThisPlatform;
	public GameObject DesignatedButton;
	GameObject TheRigid;
	public bool UsingB_Goal;
	public bool ActivatePlat;

	// Use this for initialization
	void Start () {
		ThisPlatform = this.transform;
		TheRigid = ThisPlatform.FindChild ("TheRigidBody").gameObject;
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
				TheRigid.SetActive(true);
			else
				TheRigid.gameObject.SetActive(false);

		}
		else
			Debug.Log("no button in:" +this.gameObject.name+"!");
	}
}
