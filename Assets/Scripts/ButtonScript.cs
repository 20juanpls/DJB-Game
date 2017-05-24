using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
    public Material IndicatorLightsM;
    GameObject Toggle;
    ToogleTouchScript ToggleScript;
	public GameObject MovingRigid;

	Rigidbody TheRigidOfMove;

    Vector3 OrigTogPos, TheOrg;

    public bool ButtonActive, ActivatorB, TimerB, ToggleB, PannelB;
    public float ButtonTime, LightsSpeed;

    bool instapress, ActivateTimer, IsMovingPlat;
    float CurrentTime, LightsGrayScale;

    int PannelCount;

	// Use this for initialization
	void Start () {
        Toggle = transform.FindChild("toggle").gameObject;
        try
        {
            //IndicatorLights = transform.FindChild("Lights").gameObject;//.GetComponent<Shader>();
            IndicatorLightsM = transform.FindChild("Lights").gameObject.GetComponent<Renderer>().material;
            
        }
        catch{
            IndicatorLightsM = null;
        }
		TheOrg = Toggle.transform.position;
        ToggleScript = Toggle.GetComponent<ToogleTouchScript>();
        CurrentTime = ButtonTime;
		if (MovingRigid != null)
			TheRigidOfMove = MovingRigid.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log (OrigTogPos + ", "+Toggle.transform.position);
        if (IndicatorLightsM != null) {
            IndicatorLightsM.SetColor("_TintColor", new Color(IndicatorLightsM.GetColor("_TintColor").r, IndicatorLightsM.GetColor("_TintColor").g, IndicatorLightsM.GetColor("_TintColor").b, LightsGrayScale * 0.1f));
            //if (LightsGrayScale * 0.1f >= 0.5f)
            //    Debug.Log("Enough");
            //Debug.Log(IndicatorLightsM.GetColor("_TintColor"));
            if (ButtonActive)
                LightsGrayScale = Mathf.Lerp(LightsGrayScale, 5.0f, LightsSpeed * Time.deltaTime);
            else
                LightsGrayScale = Mathf.Lerp(LightsGrayScale, 0.0f, LightsSpeed * Time.deltaTime);

        }

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
            Toggle.transform.position = Vector3.MoveTowards(Toggle.transform.position, (Vector3.up * -0.3f) + OrigTogPos, 20.0f * Time.deltaTime);
        }
        else {
            Toggle.transform.position = Vector3.MoveTowards(Toggle.transform.position, OrigTogPos, 20.0f * Time.deltaTime);
        }
    }
}
