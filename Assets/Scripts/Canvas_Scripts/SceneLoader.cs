using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class SceneLoader : MonoBehaviour {

	public GameObject loadingCanvas, LoadingText, BlackB;
    Image BlackBakground;
    public float FadeOutSpeed, GrayScale;
    public bool BeginClear;

	void Start() {

		loadingCanvas = GameObject.Find ("LoadingScreen_Canvas");
        LoadingText = this.transform.FindChild("LoadingText").gameObject;
        BlackB = this.transform.FindChild("BlackBackground").gameObject;
        BlackBakground = BlackB.GetComponent<Image>();

        GrayScale = BlackBakground.color.a;

        StartCoroutine (LoadingScreen ());

    }
	void Update() {
        if (BeginClear) {
            BlackBakground.color = new Color(BlackBakground.color.r, BlackBakground.color.g, BlackBakground.color.b, GrayScale);
            GrayScale = Mathf.Lerp(GrayScale, 0.0f, FadeOutSpeed * Time.deltaTime);
        }

        if (GrayScale <= 0.01f)
            loadingCanvas.SetActive(false);

	}

	IEnumerator LoadingScreen()
	{
		Time.timeScale = 0.0f;
        LoadingText.SetActive(true);

        yield return new WaitForSeconds (1);

		Time.timeScale = 1.0f;
        LoadingText.SetActive(false);
        BeginClear = true;
    }
}