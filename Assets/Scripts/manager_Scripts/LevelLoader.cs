using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public string sceneName;
	AddingLoading lScreen; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerMesh") {
            SceneManager.LoadScene(sceneName);
			lScreen.newLoad();
		}

    }
}
