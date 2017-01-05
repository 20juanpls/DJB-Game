using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    PlayerMovement_Ver2 PlayerScript;
    Transform PlayerTrn;
    CapsuleCollider PlayerColl;
    PlayerKnockback KnockKnock;

	public int StartHealth = 3, Lives = 3;


    public float CrushSpeedMultiplier = 10.0f;
    public float TimeCrushed = 1.0f;
    public float CrushRealizationTime = 1.0f;
    public bool IsDead = false;
	public bool GameOver = false;

	public float currentHealth;
    bool Crushing = false;


	// Use this for initialization
	void Start () {
        PlayerScript = this.GetComponent<PlayerMovement_Ver2>();
        PlayerTrn = this.GetComponent<Transform>();
        PlayerColl = this.GetComponent<CapsuleCollider>();
        KnockKnock = this.GetComponent<PlayerKnockback>();
        currentHealth = StartHealth;

    }
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Deaths:"+ Deaths);
		//if (Deaths == Lives - 1) {
		//	GameOver = true;
		//	Debug.Log ("GG m8");
		//}
        currentHealth = StartHealth - KnockKnock.totalDamage;

        if (currentHealth == 0.0f) {
           if (KnockKnock.collided == false)
            {
                IsDead = true;
            }
            PlayerScript.DontMove = true;
        }

        ThisIsATest();
        GettingCrushed();

	}

    void ThisIsATest()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Crushing = true;
        }
    }

    void GettingCrushed() {
        if (Crushing == true) {
            TimeCrushed -= Time.deltaTime;
            PlayerTrn.localScale = new Vector3(PlayerTrn.localScale.x, PlayerTrn.localScale.y*TimeCrushed* CrushSpeedMultiplier, PlayerTrn.localScale.z);
            PlayerColl.radius = 0.01f;
            PlayerScript.AcceptedFloorDist = 0.1f;
            KnockKnock.Inactive = true;
            PlayerScript.DontMove = true;

            if (PlayerTrn.localScale.y <= 0.0f) {
                CrushRealizationTime -= Time.deltaTime;
                if (CrushRealizationTime <= 0.0f)
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
    public void PlayerHealthReset() {
        PlayerScript = this.GetComponent<PlayerMovement_Ver2>();
        PlayerTrn = this.GetComponent<Transform>();
        PlayerColl = this.GetComponent<CapsuleCollider>();
        KnockKnock = this.GetComponent<PlayerKnockback>();
        currentHealth = StartHealth;

        IsDead = false;
        PlayerScript.DontMove = false;
        KnockKnock.totalDamage = 0.0f;
    }
}
