using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LooseCanvasScript : MonoBehaviour {
    PlayerLavaDeath SpawnManager;
    GameObject LoseText, GameOverText;
    public bool FillUp;
    public float LooseScreenTime,FadeSpeed, GrayScale, WordGrayScale, GameOverGrayScale;
    Image LoseImage;
    public float CurrentLooseScreenTime;

    public bool AllFilled;
    // Use this for initialization
    void Start () {
        SpawnManager = GameObject.Find("SpawnManager").GetComponent<PlayerLavaDeath>();
        LoseImage = transform.FindChild("LoseImage").GetComponent<Image>();
        LoseText = transform.FindChild("LoseText").gameObject;
        GameOverText = transform.FindChild("GameOverText").gameObject;
        GameOverText = transform.FindChild("GameOverText").gameObject;
        CurrentLooseScreenTime = LooseScreenTime;
        GrayScale = 0.0f;
        WordGrayScale = 0.0f;
        GameOverGrayScale = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (SpawnManager.LoseScreenActive || SpawnManager.gameOverScreenActive)
            FillUp = true;
        // else
        //     FillUp = false;

        if (FillUp)
        {
            GrayScale = Mathf.Lerp(GrayScale, 1.0f, FadeSpeed * Time.deltaTime);
            if (GrayScale >= 0.9f)
                GrayScale = 1.0f;
        }
        else
        {
            GrayScale = Mathf.Lerp(GrayScale, 0.0f, FadeSpeed * Time.deltaTime);
            if (GrayScale <= 0.01f)
                GrayScale = 0.0f;
        }

        if (GrayScale >= 0.9f && FillUp == true)
        {
            if (SpawnManager.LoseScreenActive)
                DuringLooseScreen();
            else if (SpawnManager.gameOverScreenActive)
                DuringGameOverScreen();
        }

        LoseImage.color = new Color(LoseImage.color.r, LoseImage.color.g, LoseImage.color.b, GrayScale);
        LoseText.GetComponent<Text>().color = new Color(LoseText.GetComponent<Text>().color.r
            , LoseText.GetComponent<Text>().color.g, 
            LoseText.GetComponent<Text>().color.b, 
            WordGrayScale);
        GameOverText.GetComponent<Text>().color = new Color(GameOverText.GetComponent<Text>().color.r
            , GameOverText.GetComponent<Text>().color.g,
            GameOverText.GetComponent<Text>().color.b,
            GameOverGrayScale);
    }

    void DuringLooseScreen() {
        CurrentLooseScreenTime -= Time.deltaTime;
        if (CurrentLooseScreenTime >= 0.0f) 
        {
            WordGrayScale = Mathf.Lerp(WordGrayScale, 1.0f, FadeSpeed * Time.deltaTime);
            if (WordGrayScale >= 0.9f)
            {
                WordGrayScale = 1.0f;
                AllFilled = true;
            }
        }
        else if (CurrentLooseScreenTime <= 0.0f) {
            WordGrayScale = Mathf.Lerp(WordGrayScale, 0.0f, FadeSpeed * Time.deltaTime);
            if (WordGrayScale <= 0.01f)
            {
                WordGrayScale = 0.0f;
                AllFilled = false;
            }

            if (!AllFilled)
            {
                FillUp = false;
                CurrentLooseScreenTime = LooseScreenTime;
                SpawnManager.Restart();
            }
        }
    }
    void DuringGameOverScreen() {
        CurrentLooseScreenTime -= Time.deltaTime;
        if (CurrentLooseScreenTime >= 0.0f)
        {
            GameOverGrayScale = Mathf.Lerp(GameOverGrayScale, 1.0f, FadeSpeed * Time.deltaTime);
            if (GameOverGrayScale >= 0.9f)
            {
                GameOverGrayScale = 1.0f;
                AllFilled = true;
            }
        }
        else if (CurrentLooseScreenTime <= 0.0f)
        {
            GameOverGrayScale = Mathf.Lerp(GameOverGrayScale, 0.0f, FadeSpeed * Time.deltaTime);
            if (GameOverGrayScale <= 0.01f)
            {
                GameOverGrayScale = 0.0f;
                AllFilled = false;
            }
            if (!AllFilled)
                SpawnManager.RestartFromBeginning();
        }
    }
}
