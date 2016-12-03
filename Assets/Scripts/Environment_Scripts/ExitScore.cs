using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitScore : MonoBehaviour {

	PlayerScore ps;
	Text txt;

	void Start(){
		ps = GameObject.Find("Player").GetComponent<PlayerScore>();
		if (ps == null){
			Debug.Log("PlayerScore is not initialized/found! :" + this.ToString());
		}

		txt = this.GetComponent<Text>();
		if (txt == null){
			Debug.Log("Text object is not initialized/found! :" + this.ToString());
		}
	}

	void Update(){
		txt.text = "Coins: " + ps.score + "/" + ps.coins.Length;
	}



}
