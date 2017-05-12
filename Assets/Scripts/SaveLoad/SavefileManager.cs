using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.SceneManagement;

<<<<<<< HEAD
public class SavefileManager : MonoBehaviour {

	public int status;
	String sceneName;

	String defaultSave;


	// Use this for initialization
	void Start () {


		defaultSave = "DJB SAVE FILE \n------------------------- \nLevelOneTutorialThing 0 0 \nStage_1_3 0 0 \nWallClimbingandJumping 0 0 \nStage_Babylon 0 0 \nDJB_TestStg_02 0 0 \nEND";


		//Creating save file if doesn't exist, if not just debug out save file exist
		if (File.Exists ("DJB_SaveFile")) {
			Debug.Log ("Save file exists!");
		} else {
			var fileName = File.CreateText ("DJB_SaveFile");

			fileName.Write(defaultSave);
			/*fileName.WriteLine ("DJB SAVE FILE ");
			fileName.WriteLine ("------------------------- ");

			fileName.WriteLine ("DJB SAVE FILE ");
			fileName.WriteLine ("-------------------------");
			fileName.WriteLine ("SceneName 0/1 ");

=======
public class SavefileManager : MonoBehaviour
{

    public int status;
    String sceneName;
    String defaultSave;

    // Use this for initialization
    void Start()
    {

        defaultSave = "DJB SAVE FILE \n------------------------- \nLevelOneTutorialThing 0 0 \nStage_1_3 0 0 \nWallClimbingandJumping 0 0 \nStage_Babylon 0 0 \nDJB_TestStg_02 0 0 \nEND";

        //Creating save file if doesn't exist, if not just debug out save file exist
        if (File.Exists("DJB_SaveFile"))
        {
            Debug.Log("Save file exists!");
        }
        else
        {
            var fileName = File.CreateText("DJB_SaveFile");
            fileName.Write(defaultSave);
            /*fileName.WriteLine ("DJB SAVE FILE ");
			fileName.WriteLine ("------------------------- ");
			fileName.WriteLine ("DJB SAVE FILE ");
			fileName.WriteLine ("-------------------------");
			fileName.WriteLine ("SceneName 0/1 ");
>>>>>>> 83494f8a1eb18ac1b230dc56dc96f67f4e238c16
			fileName.WriteLine ("LevelOneTutorialThing 0 ");
			fileName.WriteLine ("WallClimbingandJumping 0 ");
			fileName.WriteLine ("Stage_Babylon 0 ");
			fileName.WriteLine ("DJB_TestStg_02 0 "); //TESTING; REMOVE LATER

			*/
<<<<<<< HEAD

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

			if (finalChar < 0) {
				Debug.Log ("ERR");
				break;
			}
			//Debug.Log ("Final Char: " + finalChar);
			sub = allText.Substring (1, finalChar);
			sub = sub.Substring (0, sub.Length-1);
			if (sub.Equals ("END")) {
				Debug.Log ("Level not found! (are you in the hub?)");
				break;
			}

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

		UpdateScore ();
	}

	public void UpdateScore(){
		StreamReader reader = new StreamReader("DJB_SaveFile");
		String all = reader.ReadToEnd ();

		if (sceneName == null) {
			Debug.Log ("NOT FOUND");
		} else {
			if (all.Contains (sceneName)) {
				int loc = all.IndexOf (sceneName);
				all = all.Substring (loc+sceneName.Length+3);
				loc = all.IndexOf (" ");
				all = all.Substring (0, loc);
				//All the way to the loc of the screen name + 3
				//Debug.Log(all.Length + " vs " + (loc+sceneName.Length+3));
				//Debug.Log("Score is " + all);
				//GameObject.Find("Player").GetComponent<PlayerScore>().score = int.Parse(all);

			}
		}
		reader.Close ();
	}




		

	public void SaveFile(int newState){
		StreamReader reader = new StreamReader("DJB_SaveFile");
		String all = reader.ReadToEnd ();
		int loc = all.IndexOf (sceneName);
		//All the way to loc of scene name
		String front = all.Substring (0, loc + sceneName.Length);
		//Space, status, space
		//from 3 char after loc to end

		String back = all.Substring(loc+4+sceneName.Length);
		//Debug.Log (front + " " + newState + " " + back);
		
		//Remove old file, replace with new one (that has an open filewriter)
		File.Delete ("DJB_SaveFile");
		var fileWriter = File.CreateText ("DJB_SaveFile");
		fileWriter.Write (front + " " + newState + " " + GameObject.Find("Player").GetComponent<PlayerScore>().score + back);


		reader.Close ();
		fileWriter.Close();


	}

	public int GetStatus(String _sceneName){

		Debug.Log ("WOO");

		StreamReader reader = new StreamReader("DJB_SaveFile");
		String allText = reader.ReadToEnd();
		String currentLine;
		int finalChar = 0;
		String sub;
		//Debug.Log ("ORIGINAL TEXT\n"+allText);
		status = 0;
		for (int x = 0; x < allText.Length; x++) {
			finalChar = allText.IndexOf (" ");
			if (finalChar < 0) {
				Debug.Log ("ERR");
				break;
			}
			//Debug.Log ("Final Char: " + finalChar);
			sub = allText.Substring (1, finalChar);
			sub = sub.Substring (0, sub.Length-1);
			if (sub.Equals ("END")) {
				Debug.Log ("Level not found! (are you in the hub?)");
				break;
			}
			//Debug.Log ("Sub: <" + sub + ">");
			if (sub.Equals (_sceneName)) {
				//Debug.Log ("Found Scene!");
				allText = allText.Substring (sub.Length+2);
				allText = allText.Substring (0, 1);
				//Debug.Log ("Final Text: <" + allText + ">");
				//Debug.Log("FOUND: " + allText);
				status = int.Parse (allText);
				x = 9999;
			} else {
				allText = allText.Substring (finalChar + 1);
				//Debug.Log ("Modified Text\n" + allText);
			}
		}
		reader.Close ();

		return status;



	}


	
	// Update is called once per frame
	void Update () {
		
	}
=======
            fileName.Close();
            Debug.Log("Created new save file!");
        }

        sceneName = SceneManager.GetActiveScene().name; //Get scene name
                                                        //check if scene name matches the name in the file
        StreamReader reader = new StreamReader("DJB_SaveFile");
        String allText = reader.ReadToEnd();
        String currentLine;
        int finalChar = 0;
        String sub;
        //Debug.Log ("ORIGINAL TEXT\n"+allText);
        status = 0;
        for (int x = 0; x < 15; x++)
        {
            finalChar = allText.IndexOf(" ");
            if (finalChar < 0)
            {
                Debug.Log("ERR");
                break;
            }
            //Debug.Log ("Final Char: " + finalChar);
            sub = allText.Substring(1, finalChar);
            sub = sub.Substring(0, sub.Length - 1);
            if (sub.Equals("END"))
            {
                Debug.Log("Level not found! (are you in the hub?)");
                break;
            }

            //Debug.Log ("Final Char: " + finalChar);
            sub = allText.Substring(1, finalChar);
            sub = sub.Substring(0, sub.Length - 1);
            //Debug.Log ("Sub: <" + sub + ">");
            if (sub.Equals(sceneName))
            {
                //Debug.Log ("Found Scene!");
                allText = allText.Substring(sub.Length + 2);
                allText = allText.Substring(0, 1);
                //Debug.Log ("Final Text: <" + allText + ">");
                status = int.Parse(allText);
                x = 9999;
            }
            else
            {
                allText = allText.Substring(finalChar + 1);
                //Debug.Log ("Modified Text\n" + allText);
            }
        }
        reader.Close();

        //SaveFile (1);
        //Debug.Log (allText);
        UpdateScore();
    }

    public void UpdateScore()
    {
        StreamReader reader = new StreamReader("DJB_SaveFile");
        String all = reader.ReadToEnd();

        if (sceneName == null)
        {
            Debug.Log("NOT FOUND");
        }
        else
        {
            if (all.Contains(sceneName))
            {
                int loc = all.IndexOf(sceneName);
                all = all.Substring(loc + sceneName.Length + 3);
                loc = all.IndexOf(" ");
                all = all.Substring(0, loc);
                //All the way to the loc of the screen name + 3
                //Debug.Log(all.Length + " vs " + (loc+sceneName.Length+3));
                //Debug.Log("Score is " + all);
                //GameObject.Find("Player").GetComponent<PlayerScore>().score = int.Parse(all);

            }
        }
        reader.Close();
    }

    public void SaveFile(int newState)
    {
        StreamReader reader = new StreamReader("DJB_SaveFile");
        String all = reader.ReadToEnd();
        int loc = all.IndexOf(sceneName);
        //All the way to loc of scene name
        String front = all.Substring(0, loc + sceneName.Length);
        //Space, status, space
        //from 3 char after loc to end
        String back = all.Substring(loc + 4 + sceneName.Length);
        //Debug.Log (front + " " + newState + " " + back);

        //Remove old file, replace with new one (that has an open filewriter)
        reader.Close();
        while (File.Exists("DJB_SaveFile"))
        {
            File.Delete("DJB_SaveFile");
        }
        //
        var fileWriter = File.CreateText("DJB_SaveFile");
        fileWriter.Write(front + " " + newState + " " + GameObject.Find("Player").GetComponent<PlayerScore>().score + back);
        //while (File.Exists("DJB_SaveFile"))
        //{
        //    File.Delete("DJB_SaveFile");
        //}
        //var fileWriter 

        fileWriter.Close();


    }

    public int GetStatus(String _sceneName)
    {

        Debug.Log("WOO");

        StreamReader reader = new StreamReader("DJB_SaveFile");
        String allText = reader.ReadToEnd();
        String currentLine;
        int finalChar = 0;
        String sub;
        //Debug.Log ("ORIGINAL TEXT\n"+allText);
        status = 0;
        for (int x = 0; x < allText.Length; x++)
        {
            finalChar = allText.IndexOf(" ");
            if (finalChar < 0)
            {
                Debug.Log("ERR");
                break;
            }
            //Debug.Log ("Final Char: " + finalChar);
            sub = allText.Substring(1, finalChar);
            sub = sub.Substring(0, sub.Length - 1);
            if (sub.Equals("END"))
            {
                Debug.Log("Level not found! (are you in the hub?)");
                break;
            }
            //Debug.Log ("Sub: <" + sub + ">");
            if (sub.Equals(_sceneName))
            {
                //Debug.Log ("Found Scene!");
                allText = allText.Substring(sub.Length + 2);
                allText = allText.Substring(0, 1);
                //Debug.Log ("Final Text: <" + allText + ">");
                //Debug.Log("FOUND: " + allText);
                status = int.Parse(allText);
                x = 9999;
            }
            else
            {
                allText = allText.Substring(finalChar + 1);
                //Debug.Log ("Modified Text\n" + allText);
            }
        }
        reader.Close();

        return status;
    }



// Update is called once per frame
    void Update()
    {

    }
>>>>>>> 83494f8a1eb18ac1b230dc56dc96f67f4e238c16
}

