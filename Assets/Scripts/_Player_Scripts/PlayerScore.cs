using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

	public int score;
    public float YSpeed;
	Text scoreText;
    Text DebugYSpeedText, DebugEngineVelTest, DebugAirTime;
	public GameObject[] coins;
	public PlayerHealth playerHealth;
    PlayerMovement_Ver2 playMove;

	// Use this for initialization
	void Start () {
		playerHealth = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<PlayerHealth> ();

        //for debug
        playMove = this.GetComponent<PlayerMovement_Ver2>();
        //for debug.

		score = 0;
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();

        //for debug
        try{
            DebugYSpeedText = GameObject.Find("YSpeedText").GetComponent<Text>();
            DebugEngineVelTest = GameObject.Find("DebugEngineVelTest").GetComponent<Text>();
            DebugAirTime = GameObject.Find("DebugAirTime").GetComponent<Text>();
        }
        catch {
            DebugYSpeedText = null;
            DebugEngineVelTest = null;
            DebugAirTime = null;
        }
        //for debug.

        if (scoreText == null){
			Debug.Log("Text for score missing! : "+ this.ToString());
		}

        //for debug
        if (DebugYSpeedText == null || DebugEngineVelTest == null||DebugAirTime == null)
            Debug.Log("Still Debugging YSpeed, but not found: " + this.ToString());
        //for debug.

        GameObject.Find ("SceneSaver").GetComponent<SavefileManager> ().UpdateScore ();
		coins = GameObject.FindGameObjectsWithTag("Coin");
		if (score == null) {
			score = 0;
		}
		scoreText.text = "Score: " + score;
	}

    void Update()
    {
        if (DebugAirTime != null && DebugYSpeedText != null && DebugYSpeedText != null)
        {
            DebugYSpeedText.text = "FinalVel Y_Value: " + playMove.FinalVel.y;
            DebugEngineVelTest.text = "P_Rigidbody Y_Value: " + this.GetComponent<Rigidbody>().velocity.y;
            DebugAirTime.text = "AirTimeValue: " + playMove.airTime;

            if (this.GetComponent<PlayerKnockback>().DangerousFall)
            {
                DebugYSpeedText.color = Color.red;
                DebugEngineVelTest.color = Color.red;
            }
            else
            {
                DebugYSpeedText.color = Color.black;
                DebugEngineVelTest.color = Color.black;
            }

            if (playMove.ClimbSequence)
            {
                DebugAirTime.color = Color.green;
            }
            else
            {
                DebugAirTime.color = Color.black;
            }
        }

        ///Apparently Airtime freezes to zero at respawn by taking fall damage after getting hit by a fly npc from
        ///a certain height...
        ///----pls fix 5/15/17
    }

    // ScoreUpdate is called whenever a coin is collected
    void ScoreUpdate () {
		score++;
		scoreText.text = "Score: " + score;
		if(score >= 100)
		{
			score -= 100;
			playerHealth.Lives = playerHealth.Lives + 1;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Coin" && other.gameObject.GetComponent<MeshRenderer>().enabled == true) {
			ScoreUpdate ();
			other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Collider>().enabled = false;
		}

        if (other.tag == "HeartHP" && other.gameObject.GetComponent<MeshRenderer>().enabled == true)
        {
			other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (other.tag == "GoalCollectable") {
			//SavefileManager sm = GameObject.Find ("SceneSaver").GetComponent<SavefileManager> ();
			//sm.SaveFile (1);
            GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<PlayerLavaDeath>().PlayerWinState = true;
        }
    }

}
