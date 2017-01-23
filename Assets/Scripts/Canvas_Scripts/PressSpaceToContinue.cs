using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PressSpaceToContinue : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
            //SceneManager.LoadScene("Bugfixer_0_0_1");//, LoadSceneMode.Additive);
            SceneManager.LoadScene("DJB_0_0_1_StartScene");
            Destroy (this.gameObject);
		}

	}

	public void Restart(){
        SceneManager.LoadScene("DJB_0_0_1_StartScene");
    }

}
