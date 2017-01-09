using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HeartContainer_Script : MonoBehaviour {
	PlayerHealth PlayHP;
	GameObject thisHeartCanvas;
	public GameObject heartContainer;
	public GameObject heart;
    public float OnScreenTimer;
    public float CurrentOnScreenTimer;
	int count, heartsOff;

    GameObject TheLivesTextG_obj;
    Text LivesText;

    public ArrayList Hrts;
    public ArrayList HrtsC;
    public ArrayList HrtsLeft;
    public ArrayList PositionList;
    
        // Use this for initialization
    void Start () {
		if (Hrts == null)
			Hrts = new ArrayList();
		if (HrtsC == null)
			HrtsC = new ArrayList ();
        if (HrtsLeft == null)
            HrtsLeft = new ArrayList();
        if (PositionList == null)
            PositionList = new ArrayList();

        thisHeartCanvas = this.gameObject;
		PlayHP = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<PlayerHealth> ();

		for (int i = 0; i < PlayHP.StartHealth; i++) {
            Vector3 _OrigHeartPos;

            GameObject _H = (GameObject)Instantiate(heartContainer);
            _H.transform.SetParent(thisHeartCanvas.transform);
            _H.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _H.GetComponent<RectTransform>().anchoredPosition = new Vector3(88.0f + (i * 100), -60.0f, 0.0f); ;
            HrtsC.Add(_H);

            GameObject _hC = (GameObject)Instantiate (heart);
			_hC.transform.SetParent (thisHeartCanvas.transform);
			_hC.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
            _hC.GetComponent<RectTransform>().anchoredPosition = new Vector3(88.0f + (i * 100), -60.0f, 0.0f); ;
			Hrts.Add (_hC);
            HrtsLeft.Add (_hC);

            _OrigHeartPos = _hC.transform.position;

            PositionList.Add(_OrigHeartPos);
		}


        TheLivesTextG_obj = GameObject.Find("LivesText");
        LivesText = TheLivesTextG_obj.GetComponent<Text>();
        if (LivesText == null)
        {
            Debug.Log("Text for Lives missing! : " + this.ToString());
        }
        LivesText.text = "Lives: 0";

        CurrentOnScreenTimer = OnScreenTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("current Health:"+PlayHP.currentHealth);
        //Debug.Log("Count in heart list: "+Hrts.Count);
        //Debug.Log("Hearts Left: "+ HrtsLeft.Count);

        if (PlayHP.currentHealth < HrtsLeft.Count)//Hrts.Count)
        {
            //Debug.Log("Ouch!");

            GameObject IsOff = null;
            IsOff = (GameObject)Hrts[PlayHP.currentHealth];
            IsOff.GetComponent<RawImage>().enabled = false;
            HrtsLeft.RemoveAt(PlayHP.currentHealth);

        } 
       
        if (PlayHP.currentHealth > HrtsLeft.Count)
        {
            //Debug.Log("Does this happen?!");
            GameObject IsOn = null;
            //Debug.Log(PlayHP.currentHealth);
            IsOn = (GameObject)Hrts[PlayHP.currentHealth-1];
            IsOn.GetComponent<RawImage>().enabled = true;
            HrtsLeft.Add(IsOn);

        }

        MovingHeartsInAndOut();

        LivesText.text = "Lives: " + (PlayHP.Lives - GameObject.Find("SpawnManager").GetComponent<PlayerLavaDeath>().Deaths);
        if (PlayHP.Lives - GameObject.Find("SpawnManager").GetComponent<PlayerLavaDeath>().Deaths == 0) {
            TheLivesTextG_obj.SetActive(false);
        }
    }

    void MovingHeartsInAndOut(){
        if (PlayHP.currentHealth == Hrts.Count)
        {
            CurrentOnScreenTimer -= Time.deltaTime;
            if (CurrentOnScreenTimer <= 0.0f)
            {
                for (int j = 0; j < HrtsC.Count; j++)
                {
                    GameObject MovingH = null;
                    GameObject MovingHC = null;
                    Vector3 HOrigPos = Vector3.zero;
                    MovingH = (GameObject)Hrts[j];
                    MovingHC = (GameObject)HrtsC[j];
                    HOrigPos = (Vector3)PositionList[j];
                    MovingH.transform.position = Vector3.MoveTowards(MovingH.transform.position, HOrigPos + Vector3.up * 100, 2.0f);
                    MovingHC.transform.position = Vector3.MoveTowards(MovingH.transform.position, HOrigPos + Vector3.up * 100, 2.0f);
                }
            }
        }
        else {
            for (int k = 0; k < HrtsC.Count; k++)
            {
                GameObject MovingH = null;
                GameObject MovingHC = null;
                Vector3 HOrigPos = Vector3.zero;
                MovingH = (GameObject)Hrts[k];
                MovingHC = (GameObject)HrtsC[k];
                HOrigPos = (Vector3)PositionList[k];
                MovingH.transform.position = Vector3.MoveTowards(MovingH.transform.position, HOrigPos, 2.0f);
                MovingHC.transform.position = Vector3.MoveTowards(MovingH.transform.position, HOrigPos, 2.0f);
            }
            CurrentOnScreenTimer = OnScreenTimer;
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
            HeartOn.GetComponent<RawImage>().enabled = true;
        }

    }
}
