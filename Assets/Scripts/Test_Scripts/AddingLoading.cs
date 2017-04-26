using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddingLoading : MonoBehaviour {

	void Start() {
		
	}
	public void newLoad()
	{
		GameObject newGO = new GameObject("LoadScreen");
		newGO.transform.SetParent(this.transform);
		Text myText = newGO.AddComponent<Text>();
		myText.text = "Loading...";
	}
	// Update is called once per frame
	void Update () {
		
	}
}
