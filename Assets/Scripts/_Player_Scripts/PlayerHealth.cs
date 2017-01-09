using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    PlayerMovement_Ver2 PlayerScript;
    Transform PlayerTrn;
    CapsuleCollider PlayerColl;
    PlayerKnockback KnockKnock;

    public int StartHealth = 3, Lives = 3;//, BlinkFrameBuffer = 5;

    //int BlinkFrameCount;


    public float CrushSpeedMultiplier = 10.0f;
    public float InvincibiltyFramesTime = 1.0f;
    public float TimeCrushed = 1.0f;
    public float CrushRealizationTime = 1.0f;
    public bool IsDead = false;
	public bool GameOver = false;

    public bool IsInvincible = false;

	public bool isInvincible = false;

	public int currentHealth;
    public bool Crushing = false;
    bool Blinking = false;
    //bool BlinkOff = false;

    private float currentInviTime;
    private float CurrentTimeCrushed;
    private float CurrentCrushRealizationTime;
    //I have to make these below public ;-;
    public float OrigAcceptedFloorDist;
    public float OrigCollRad;
    private float UpDist;
    public Vector3 OrigScale;

	// Use this for initialization
	void Start () {
        PlayerScript = this.GetComponent<PlayerMovement_Ver2>();
        PlayerTrn = this.GetComponent<Transform>();
        PlayerColl = this.GetComponent<CapsuleCollider>();
        KnockKnock = this.GetComponent<PlayerKnockback>();
        currentHealth = StartHealth;

        currentInviTime = InvincibiltyFramesTime;
        CurrentTimeCrushed = TimeCrushed;
        CurrentCrushRealizationTime = CrushRealizationTime;
        OrigAcceptedFloorDist = PlayerScript.AcceptedFloorDist;
        OrigCollRad = PlayerColl.radius;
        OrigScale = PlayerTrn.localScale;
        //Debug.Log("OrigScale:" + OrigScale);

    }
	
	// Update is called once per frame
	void Update () {
        UpRayShooter();
        //Debug.Log("total damage "+ KnockKnock.totalDamage);
        if (IsDead == true)
        {
            //Debug.Log("GO AWAY!!!");
            //takenaway mb later
            PlayerTrn.GetChild(1).gameObject.SetActive(true);

        }
        else if (IsDead == false)
        {
			if (isInvincible == false) {
				currentHealth = StartHealth - KnockKnock.totalDamage;
			}

			if (KnockKnock.collided == true) {
				isInvincible = true;
			} else {
				isInvincible = false;
			}

            if (KnockKnock.collided == true)
            {
                IsInvincible = true;
            }

            if (IsInvincible == true)
            {
                InvinsibilityFrames();
            }
            else {
                currentInviTime = InvincibiltyFramesTime;
            }

            if (currentHealth == 0.0f)
            {
                if (KnockKnock.collided == false)
                {
                    IsDead = true;
                }
                PlayerScript.DontMove = true;
            }

            GettingCrushed();
        }

	}

    void GettingCrushed() {
        if (Crushing == true) {
            CurrentTimeCrushed -= Time.deltaTime;
            PlayerTrn.localScale = new Vector3(PlayerTrn.localScale.x, PlayerTrn.localScale.y*TimeCrushed* CrushSpeedMultiplier, PlayerTrn.localScale.z);
            PlayerColl.radius = 0.01f;
            PlayerScript.AcceptedFloorDist = 0.1f;
            KnockKnock.Inactive = true;
            PlayerScript.DontMove = true;

            if (PlayerTrn.localScale.y <= 0.01f) {
                CurrentCrushRealizationTime -= Time.deltaTime;
                if (CurrentCrushRealizationTime <= 0.0f)
                {
                    Crushing = false;
                    IsDead = true;
                }
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        //on trigger collision with tagged "kill"
        if (other.tag == "Kill")
        {
            //loseScreen.gameObject.SetActive(true);
            IsDead = true;
        }

    }

    void UpRayShooter() {
        RaycastHit uphit;
        if (Physics.Raycast(PlayerTrn.position, new Vector3(0.0f, 1.0f, 0.0f), out uphit)) {

            if (uphit.transform.tag == "StompNPC") {
                UpDist = uphit.distance;
                //Debug.Log(UpDist);
                if (UpDist <= 1.5f && PlayerScript.isGrounded == true) {
                    Crushing = true;
                }
            }
        }

    }

    void InvinsibilityFrames() {
        KnockKnock.cantTakeDamage = true;

        if (KnockKnock.collided == false && Blinking == false) { Blinking = true; }
        Blinker();

        currentInviTime -= Time.deltaTime;
        if (currentInviTime <= 0.0f) {
            KnockKnock.cantTakeDamage = false;
            IsInvincible = false;
            Blinking = false;
            //This Is just a placeholder for momentary blinking
            PlayerTrn.GetChild(1).gameObject.SetActive(true);
            //BlinkFrameCount = 0;
            //This Is just a placeholder for momentary blinking
            

        }
    }

    void Blinker() {
        if (Blinking == true)
        {
                PlayerTrn.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void PlayerHealthReset() {
        PlayerScript = this.GetComponent<PlayerMovement_Ver2>();
        PlayerTrn = this.GetComponent<Transform>();
        PlayerColl = this.GetComponent<CapsuleCollider>();
        KnockKnock = this.GetComponent<PlayerKnockback>();
        currentHealth = StartHealth;

        IsInvincible = false;
        KnockKnock.cantTakeDamage = false;

        IsDead = false;
        PlayerScript.DontMove = false;
        KnockKnock.totalDamage = 0;

        CurrentTimeCrushed = TimeCrushed;
        CurrentCrushRealizationTime = CrushRealizationTime;

        Crushing = false;
        PlayerScript.AcceptedFloorDist = OrigAcceptedFloorDist;
        PlayerColl.radius = OrigCollRad;
        //Debug.Log("OrigScale:" + OrigScale);
        PlayerTrn.localScale = OrigScale;
    }
}
