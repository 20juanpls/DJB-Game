using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {
	
	public int score;
	Text scoreText;

	// Use this for initialization
	void Start () {
		score = 0;
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
		scoreText.text = "Score: 0";
		Debug.Log (scoreText.ToString ());
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
	}
}
