using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class SavefileManager : MonoBehaviour {

	public int status;
	String sceneName;

	// Use this for initialization
	void Start () {

		//Creating save file if doesn't exist, if not just debug out save file exist
		if (File.Exists ("DJB_SaveFile")) {
			Debug.Log ("Save file exists!");
		} else {
			var fileName = File.CreateText ("DJB_SaveFile");
			fileName.WriteLine ("DJB SAVE FILE ");
			fileName.WriteLine ("-------------------------");
			fileName.WriteLine ("SceneName 0/1 ");
			fileName.WriteLine ("LevelOneTutorialThing 0 ");
			fileName.WriteLine ("WallClimbingandJumping 0 ");
			fileName.WriteLine ("Stage_Babylon 0 ");
			fileName.WriteLine ("DJB_TestStg_02 0 "); //TESTING; REMOVE LATER
			fileName.Close();
			Debug.Log ("Created new save file!");
		}
			
		sceneName = SceneManager.GetActiveScene ().name;	//Get scene name
		//check if scene name matches the name in the file
		StreamReader reader = new StreamReader("DJB_SaveFile");
		String allText = reader.ReadToEnd();
		String currentLine;
		int finalChar = 0;
		String sub;
		//Debug.Log ("ORIGINAL TEXT\n"+allText);
		status = 0;
		for (int x = 0; x < 15; x++) {
			finalChar = allText.IndexOf (" ");
			//Debug.Log ("Final Char: " + finalChar);
			sub = allText.Substring (1, finalChar);
			sub = sub.Substring (0, sub.Length-1);
			//Debug.Log ("Sub: <" + sub + ">");
			if (sub.Equals (sceneName)) {
				//Debug.Log ("Found Scene!");
				allText = allText.Substring (sub.Length+2);
				allText = allText.Substring (0, 1);
				//Debug.Log ("Final Text: <" + allText + ">");
				status = int.Parse (allText);
				x = 9999;
			} else {
				allText = allText.Substring (finalChar + 1);
				//Debug.Log ("Modified Text\n" + allText);
			}
		}
		reader.Close ();

		//SaveFile (1);
		//Debug.Log (allText);

	}
		
	public void SaveFile(int newState){
		StreamReader reader = new StreamReader("DJB_SaveFile");
		String all = reader.ReadToEnd ();
		int loc = all.IndexOf (sceneName);
		//All the way to loc of scene name
		String front = all.Substring (0, loc + sceneName.Length);
		//Space, status, space
		//from 3 char after loc to end
		String back = all.Substring(loc+3+sceneName.Length);
		//Debug.Log (front + " " + newState + " " + back);
		File.Delete ("DJB_SaveFile");
		var fileWriter = File.CreateText ("DJB_SaveFile");
		fileWriter.Write (front + " " + newState + " " + back);

		reader.Close ();
		fileWriter.Close();
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
