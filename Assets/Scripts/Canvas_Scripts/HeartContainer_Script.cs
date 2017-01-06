using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HeartContainer_Script : MonoBehaviour {
	PlayerHealth PlayHP;
	GameObject thisHeartCanvas;
	public GameObject heartContainer;
	public GameObject heart;
	int count;

    GameObject TheLivesTextG_obj;
    Text LivesText;

    public ArrayList Hrts;
    public ArrayList HrtsC;
    
        // Use this for initialization
    void Start () {
		if (Hrts == null)
			Hrts = new ArrayList();
		if (HrtsC == null)
			HrtsC = new ArrayList ();
			
		thisHeartCanvas = this.gameObject;
		PlayHP = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<PlayerHealth> ();

		for (int i = 0; i < PlayHP.StartHealth; i++) {
            GameObject _H = (GameObject)Instantiate(heartContainer);
            _H.transform.SetParent(thisHeartCanvas.transform);
            _H.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _H.GetComponent<RectTransform>().anchoredPosition = new Vector3(88.0f + (i * 100), -60.0f, 0.0f);
            HrtsC.Add(_H);

            GameObject _hC = (GameObject)Instantiate (heart);
			_hC.transform.SetParent (thisHeartCanvas.transform);
			_hC.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_hC.GetComponent<RectTransform>().anchoredPosition = new Vector3(88.0f +(i * 100),-60.0f,0.0f);
			Hrts.Add (_hC);
		}

        TheLivesTextG_obj = GameObject.Find("LivesText");
        LivesText = TheLivesTextG_obj.GetComponent<Text>();
        if (LivesText == null)
        {
            Debug.Log("Text for Lives missing! : " + this.ToString());
        }
        LivesText.text = "Lives: 0";


    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("Count in heart list: "+Hrts.Count);
        if (PlayHP.currentHealth < Hrts.Count)
        {
            //Debug.Log("Ouch!");

            GameObject IsOff = null;
            IsOff = (GameObject)Hrts[PlayHP.currentHealth];
            IsOff.SetActive(false);
            //(GameObject)Hrts[Hrts.Count-1].SetActive(false);
        }

        LivesText.text = "Lives: " + (PlayHP.Lives - GameObject.Find("SpawnManager").GetComponent<PlayerLavaDeath>().Deaths);
        if (PlayHP.Lives - GameObject.Find("SpawnManager").GetComponent<PlayerLavaDeath>().Deaths == 0) {
            TheLivesTextG_obj.SetActive(false);
        }
    }

    public void PlayerHeartIllustratorReset(GameObject p) {
        //if (Hrts == null)
        //     Hrts = new ArrayList();
        //if (HrtsC == null)
        //   HrtsC = new ArrayList();
        PlayHP = p.GetComponent<PlayerHealth>();

        for (int i = 0; i < PlayHP.currentHealth; i++)
        {
            GameObject HeartOn = null;
            HeartOn = (GameObject)Hrts[i];
            HeartOn.SetActive(true);
        }

    }
}
