using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PressSpaceToContinue : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space) ||Input.GetKeyDown("joystick button 0")) {
            //SceneManager.LoadScene("Bugfixer_0_0_1");//, LoadSceneMode.Additive);
            SceneManager.LoadScene("Stage_Hub");
            Destroy (this.gameObject);
		}

	}

	public void Restart(){
        SceneManager.LoadScene("Stage_Hub");
    }

}
