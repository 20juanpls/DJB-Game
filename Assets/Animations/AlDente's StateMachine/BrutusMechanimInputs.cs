using UnityEngine;
using System.Collections;

public class BrutusMechanimInputs : MonoBehaviour 
{
    //A NOTE to Anyone using this code for reference: If you are using this code for reference
    //please consider that this is not by any means the best or the most efficient way to use mechanim
    //as the creator of this script, i implore you to do more research on mechanim or at least dedicate more
    //time to learning it... 
    //                                                                                  -Sinceraly Oliver Briquenkos (Omar Briceno) 
    public Animator thisAnimator;
    ThePause pauser;
    public GameObject ThePlayer;
	public Rigidbody PlayRb;
	public PlayerMovement_Ver2 Player;
	//public int beforeStart = 0;
    public float AnimSpeed;

    public bool Idle, Running, Climbing, Jumping, Falling, Sliding;

    bool upClimb, downClimb, idleClimb;

    int StopHash = Animator.StringToHash("Stops");
    int jumpHash = Animator.StringToHash("Jumping");
    int fallHash = Animator.StringToHash("Falling");
    int RunStateHash = Animator.StringToHash("Runs");
    int ClimbHash = Animator.StringToHash("Climbs");
    int SlideHash = Animator.StringToHash("Sliding");

	//public bool Climbing, Running, Sliding, Falling, Idling, Jumping, Falling;

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
        /*beforeStart++;
        if (Player == null || PlayRb == null) {
            Player = this.transform.GetComponentInParent<PlayerMovement_Ver2>();
            PlayRb = this.transform.GetComponentInParent<Rigidbody>();
            thisAnimator.speed = AnimSpeed;
        }
		if (beforeStart % 6 == 0)
		{
			RunningAnim ();
			beforeStart--;
		}*/
        OnGroundAnim();
        AnimPlayer();


        OnPause();
    }

    void OnGroundAnim() {
        Vector3 RelativPlayerMove = new Vector3(Player.VelRelativeToPlay.x, 0.0f, Player.VelRelativeToPlay.z);
        //Vector3 RelativVertMove = new Vector3(0.0f, Player.VelRelativeToPlay.y, 0.0f);
        Vector3 OverAllMove = new Vector3(Player.VelRelativeToPlay.x, Player.VelRelativeToPlay.y, Player.VelRelativeToPlay.z);
        if (Player.GroundSequence)
        {
            //thisAnimator.SetFloat("Runs", RelativPlayerMove.magnitude);

            if (RelativPlayerMove.magnitude > 2.0f)
            {
                Running = true;
                Idle = false;
                Climbing = false;
                Jumping = false;
                Falling = false;
                Sliding = false;
            }

            else
            {
                Idle = true;
                Running = false;
                Climbing = false;
                Jumping = false;
                Falling = false;
                Sliding = false;
            }

        }
        else if (Player.ClimbSequence)
        {
            Climbing = true;
            Idle = false;
            Running = false;
            Jumping = false;
            Falling = false;
            Sliding = false;
            if (OverAllMove.magnitude > 1.0f)
            {
                if (OverAllMove.y >= 0.0f)
                {
                    upClimb = true;
                    downClimb = false;
                    idleClimb = false;
                }
                else if (OverAllMove.y < 0.0f)
                {
                    upClimb = false;
                    downClimb = true;
                    idleClimb = false;
                }
            }
            else
            {
                upClimb = false;
                downClimb = false;
                idleClimb = true;
            }
        }

        if (Player.Sliding) {
            Sliding = true;
            Jumping = false;
            Falling = false;
            Climbing = false;
            Idle = false;
            Running = false;
        }

        if (!Player.ClimbSequence && !Player.GroundSequence && !Player.Sliding)
        {
            if (OverAllMove.y > 1.0f)
            {
                Jumping = true;
                Falling = false;
                Climbing = false;
                Idle = false;
                Running = false;
                Sliding = false;
            }
            else {
                Falling = true;
                Jumping = false;
                Climbing = false;
                Idle = false;
                Running = false;
                Sliding = false;
            }
        }
    }

    void AnimPlayer(){
        thisAnimator.SetTrigger(StopHash);
        thisAnimator.SetTrigger(RunStateHash);
        thisAnimator.SetTrigger(jumpHash);
        thisAnimator.SetTrigger(fallHash);
        thisAnimator.SetTrigger(ClimbHash);
        thisAnimator.SetTrigger(SlideHash);

        if (Running)
        {
            //thisAnimator.SetTrigger(RunStateHash);
            thisAnimator.SetBool(RunStateHash, true);
            thisAnimator.SetBool(StopHash, false);
            thisAnimator.SetBool(ClimbHash, false);
            thisAnimator.SetBool(fallHash, false);
            thisAnimator.SetBool(jumpHash, false);
            thisAnimator.SetBool(SlideHash, false);
        }
        else if (Idle)
        {
            //thisAnimator.SetTrigger(jumpHash);
            thisAnimator.SetBool(RunStateHash, false);
            thisAnimator.SetBool(StopHash, true);
            thisAnimator.SetBool(ClimbHash, false);
            thisAnimator.SetBool(fallHash, false);
            thisAnimator.SetBool(jumpHash, false);
            thisAnimator.SetBool(SlideHash, false);
        }
        else if (Climbing)
        {
            thisAnimator.SetBool(RunStateHash, false);
            thisAnimator.SetBool(StopHash, false);
            thisAnimator.SetBool(ClimbHash, true);
            thisAnimator.SetBool(fallHash, false);
            thisAnimator.SetBool(jumpHash, false);
            thisAnimator.SetBool(SlideHash, false);
            if (upClimb)
                thisAnimator.speed = 2.0f;
            else if (downClimb)
                thisAnimator.speed = -2.0f;
        }
        else if (Jumping)
        {
            thisAnimator.SetBool(fallHash, false);
            thisAnimator.SetBool(jumpHash, true);
            thisAnimator.SetBool(RunStateHash, false);
            thisAnimator.SetBool(StopHash, false);
            thisAnimator.SetBool(ClimbHash, false);
            thisAnimator.SetBool(SlideHash, false);

        }
        else if (Falling)
        {
            thisAnimator.SetBool(fallHash, true);
            thisAnimator.SetBool(jumpHash, false);
            thisAnimator.SetBool(RunStateHash, false);
            thisAnimator.SetBool(StopHash, false);
            thisAnimator.SetBool(ClimbHash, false);
            thisAnimator.SetBool(SlideHash, false);
        }
        else if (Sliding) {
            thisAnimator.SetBool(SlideHash, true);
            thisAnimator.SetBool(fallHash, false);
            thisAnimator.SetBool(jumpHash, false);
            thisAnimator.SetBool(RunStateHash, false);
            thisAnimator.SetBool(StopHash, false);
            thisAnimator.SetBool(ClimbHash, false);
        }

    }

    void OnPause() {
        if (pauser.Paused == true || ThePlayer.GetComponent<PlayerMovement_Ver2>().CinematicFreeze ||(idleClimb && Climbing))
        {
            thisAnimator.speed = 0.0f;
        }
        else {
            thisAnimator.speed = AnimSpeed;
        }
    }
}