using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
    GameObject Toggle;
    ToogleTouchScript ToggleScript;
	public GameObject MovingRigid;

	Rigidbody TheRigidOfMove;

    Vector3 OrigTogPos, TheOrg;

    public bool ButtonActive, ActivatorB, TimerB, ToggleB, PannelB;
    public float ButtonTime;

    bool instapress, ActivateTimer, IsMovingPlat;
    float CurrentTime;

    int PannelCount;

	// Use this for initialization
	void Start () {
        Toggle = transform.FindChild("toggle").gameObject;
		TheOrg = Toggle.transform.position;
        ToggleScript = Toggle.GetComponent<ToogleTouchScript>();
        CurrentTime = ButtonTime;
		if (MovingRigid != null)
			TheRigidOfMove = MovingRigid.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (OrigTogPos + ", "+Toggle.transform.position);

		if (MovingRigid != null) {
			OrigTogPos = TheRigidOfMove.position + TheOrg;
		} else {
			OrigTogPos = TheOrg;
		}

        ActivatorButtonOpt();
        TimerButtonOpt();
        ToggleButtonOpt();
        PannelButtonOpt();


        ToggleMover();
	}
    void ActivatorButtonOpt() {
        if (ActivatorB) {
            if (ToggleScript.PlayerOnButton)
                ButtonActive = true;
        }
    }
    void TimerButtonOpt() {
        if (TimerB) {
            if (ToggleScript.PlayerOnButton)
                ActivateTimer = true;

            if (ActivateTimer == true) {
                CurrentTime -= Time.deltaTime;
                ButtonActive = true;
                if (CurrentTime <= 0.0f) {
                    ButtonActive = false;
                    CurrentTime = ButtonTime;
                    ActivateTimer = false;
                }
            }
        }
    }
    void ToggleButtonOpt() {
        if (ToggleB) {
            if (ToggleScript.PlayerOnButton)
                ButtonActive = true;

            if (!ToggleScript.PlayerOnButton) 
                ButtonActive = false;
        }
    }
    void PannelButtonOpt() {
        if (PannelB) {
            if (ToggleScript.PlayerOnButton)
            {
                instapress = true;
                if (PannelCount > 0)
                    instapress = false;
                PannelCount++;
            }
            else
                PannelCount = 0;

            if (instapress) {
                if (ButtonActive)
                    ButtonActive = false;
                else
                    ButtonActive = true;
            }
        }
    }
    void ToggleMover() {
        if (ButtonActive)
        {
            Toggle.transform.position = Vector3.MoveTowards(Toggle.transform.position, (Vector3.up * -0.5f) + OrigTogPos, 20.0f * Time.deltaTime);
        }
        else {
            Toggle.transform.position = Vector3.MoveTowards(Toggle.transform.position, OrigTogPos, 20.0f * Time.deltaTime);
        }
    }
}
