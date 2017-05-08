using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class SceneLoader : MonoBehaviour {

	public GameObject loadingCanvas;

	void Start() {

		loadingCanvas = GameObject.Find ("LoadingScreen_Canvas");

		StartCoroutine (LoadingScreen ());

	}
	void Update() {

	}

	IEnumerator LoadingScreen()
	{
		Time.timeScale = 0.0f;
		loadingCanvas.SetActive (true);

<<<<<<< HEAD
		yield return new WaitForSeconds (2);
=======
		yield return new WaitForSeconds (1);
>>>>>>> 03ddb73f3a5811be97fcdc9eea6b8c1e7e2a6cf3

		Time.timeScale = 1.0f;
		loadingCanvas.SetActive (false);
	}
}