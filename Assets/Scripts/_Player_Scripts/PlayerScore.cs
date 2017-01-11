using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

	public int score;
	Text scoreText;
	public GameObject[] coins;

	// Use this for initialization
	void Start () {
		score = 0;
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();

		if (scoreText == null){
			Debug.Log("Text for score missing! : "+ this.ToString());
		}

		scoreText.text = "Score: 0";
		coins = GameObject.FindGameObjectsWithTag("Coin");
	}

	// ScoreUpdate is called whenever a coin is collected
	void ScoreUpdate () {
		score++;
		scoreText.text = "Score: " + score;

	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Coin") {
			ScoreUpdate ();
			Destroy (other.gameObject);
		}

        if (other.tag == "HeartHP")
        {
            Destroy(other.gameObject);
        }
    }

}
