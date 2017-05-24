using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class satansummon : MonoBehaviour {

	public AudioSource spookBG;
	public AudioSource whisps;
	public AudioSource dabber;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Alpha6) && Input.GetKey(KeyCode.F6) && Input.GetKey(KeyCode.Keypad6)){
			StopAudio ();
			spookBG.Play ();
			whisps.Play ();
		}
		if (Input.GetKey(KeyCode.Keypad0) && Input.GetKey(KeyCode.Keypad2) && Input.GetKey(KeyCode.Keypad4)){
			StopAudio ();
			dabber.Play ();
		}
	}

	void StopAudio(){
		var allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach( AudioSource audioS in allAudioSources) {
			audioS.Stop();
		}
	}
}
