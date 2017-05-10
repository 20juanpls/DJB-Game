using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

	public int score;
	Text scoreText;
	public GameObject[] coins;
	public PlayerHealth playerHealth;

	// Use this for initialization
	void Start () {
		playerHealth = GameObject.Find ("Player").GetComponent<PlayerHealth> ();
		score = 0;
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();

		if (scoreText == null){
			Debug.Log("Text for score missing! : "+ this.ToString());
		}

		GameObject.Find ("SceneSaver").GetComponent<SavefileManager> ().UpdateScore ();
		coins = GameObject.FindGameObjectsWithTag("Coin");
		if (score == null) {
			score = 0;
		}
		scoreText.text = "Score: " + score;
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
			SavefileManager sm = GameObject.Find ("SceneSaver").GetComponent<SavefileManager> ();
			sm.SaveFile (1);
            GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<PlayerLavaDeath>().PlayerWinState = true;
        }
    }

}
