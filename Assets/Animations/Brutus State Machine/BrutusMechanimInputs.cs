using UnityEngine;
using System.Collections;

public class BrutusMechanimInputs : MonoBehaviour 
{
    public Animator thisAnimator;
    ThePause pauser;
    public GameObject ThePlayer;
	public Rigidbody PlayRb;
	public PlayerMovement_Ver2 Player;
	public int beforeStart = 0;
    public float AnimSpeed;

    void Start () 
    {
        thisAnimator = GetComponent<Animator>();
        ThePlayer = transform.parent.gameObject;//GameObject.FindGameObjectWithTag("PlayerMesh");
        pauser = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<ThePause>();
        Player = ThePlayer.GetComponent<PlayerMovement_Ver2> ();
		PlayRb = ThePlayer.GetComponent<Rigidbody> ();
        AnimSpeed = thisAnimator.speed;

    }



    void Update () 
    {
		beforeStart++;
        if (Player == null || PlayRb == null) {
            Player = this.transform.GetComponentInParent<PlayerMovement_Ver2>();
            PlayRb = this.transform.GetComponentInParent<Rigidbody>();
            thisAnimator.speed = AnimSpeed;
        }
		if (beforeStart % 6 == 0)
		{
			RunningAnim ();
			beforeStart--;
		}
        OnPause();
    }
	void RunningAnim(){

		Vector3 RelativPlayerMove = new Vector3(Player.VelRelativeToPlay.x,0.0f,Player.VelRelativeToPlay.z);
		//Debug.Log (RelativPlayerMove.magnitude);
		if (Player.isGrounded == true) {
            thisAnimator.SetBool("Jumps", false);
			if (RelativPlayerMove.magnitude > 2.0f) {
                //thisAnimator.SetTrigger ("Runs");
                thisAnimator.SetBool("Runs", true);
				thisAnimator.SetBool ("Stops", false);
			} else {
                //thisAnimator.SetTrigger ("Stops");
                thisAnimator.SetBool("Stops", true);
				thisAnimator.SetBool ("Runs", false);
			}
		} else {
			thisAnimator.SetBool ("Stops", false);
			thisAnimator.SetBool ("Runs", false);
            thisAnimator.SetBool("Jumps", true);
            //thisAnimator.SetTrigger ("Jumps");
            //thisAnimator.GetComponent<Animation>()["Jumps"].time = 1.0f;
		}
	}

    void OnPause() {
        if (pauser.Paused == true)
        {
            thisAnimator.speed = 0.0f;
        }
        else {
            thisAnimator.speed = AnimSpeed;
        }
    }
}