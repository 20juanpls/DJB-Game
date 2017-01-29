using UnityEngine;
using System.Collections;

public class BrutusMechanimInputs : MonoBehaviour 
{
    public Animator thisAnimator;

    public GameObject ThePlayer;
	public Rigidbody PlayRb;
	public PlayerMovement_Ver2 Player;

    public float AnimSpeed;

    void Start () 
    {
        thisAnimator = GetComponent<Animator>();
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");
        Player = ThePlayer.GetComponent<PlayerMovement_Ver2> ();
		PlayRb = ThePlayer.GetComponent<Rigidbody> ();
        AnimSpeed = thisAnimator.speed;
    }



    void Update () 
    {
        if (Player == null || PlayRb == null) {
            Player = this.transform.GetComponentInParent<PlayerMovement_Ver2>();
            PlayRb = this.transform.GetComponentInParent<Rigidbody>();
            thisAnimator.speed = AnimSpeed;
        }
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

    public void OnPause( bool Paused) {
        //Debug.Log(thisAnimator.speed);
        //Debug.Log(AnimSpeed);
        if (Paused == true)
        {
            thisAnimator.speed = 0.0f;
        }
        else {
            thisAnimator.speed = AnimSpeed;
        }
    }
}