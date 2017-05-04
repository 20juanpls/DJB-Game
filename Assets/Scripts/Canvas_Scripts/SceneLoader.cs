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

		yield return new WaitForSeconds (3);

		Time.timeScale = 1.0f;
		loadingCanvas.SetActive (false);
	}
}