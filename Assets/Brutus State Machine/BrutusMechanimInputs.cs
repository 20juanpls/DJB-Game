using UnityEngine;
using System.Collections;

public class BrutusMechanimInputs : MonoBehaviour 
{
    public Animator thisAnimator;

	public Rigidbody PlayRb;
	public PlayerMovement_Ver2 Player;

    void Start () 
    {
        thisAnimator = GetComponent<Animator>();
		Player = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<PlayerMovement_Ver2> ();
		PlayRb = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<Rigidbody> ();
    }

    /*public void AssignPlayer(GameObject p) {
        Player = p.GetComponent<PlayerMovement_Ver2>();
        PlayRb = p.GetComponent<Rigidbody>();
    }*/

    void Update () 
    {
        if (Player == null || PlayRb == null) {
            Player = this.transform.GetComponentInParent<PlayerMovement_Ver2>();
            PlayRb = this.transform.GetComponentInParent<Rigidbody>();
        }
		/*if(Input.GetButton("Fire1"))
        {
			Debug.Log("HowMem");
            thisAnimator.SetBool("Jumps", true);
        }
		else
        {
            thisAnimator.SetBool("Jumps", false);
        }*/

		RunningAnim ();
    }

	void RunningAnim(){

		Vector3 RelativPlayerMove = new Vector3(Player.VelRelativeToPlay.x,0.0f,Player.VelRelativeToPlay.z);
		//Debug.Log (RelativPlayerMove.magnitude);
		if (Player.isGrounded == true) {
			thisAnimator.SetBool("Jumps", false);
			if (RelativPlayerMove.magnitude > 2.0f) {
				thisAnimator.SetTrigger ("Runs");
				thisAnimator.SetBool ("Stops", false);
			} else { 
				thisAnimator.SetTrigger ("Stops");
				thisAnimator.SetBool ("Runs", false);
			}
		} else {
			thisAnimator.SetBool ("Stops", false);
			thisAnimator.SetBool ("Runs", false);
			thisAnimator.SetTrigger ("Jumps");
		}
	}
}