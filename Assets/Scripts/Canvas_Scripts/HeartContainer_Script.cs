using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeartContainer_Script : MonoBehaviour {
	PlayerHealth PlayHP;
	GameObject thisHeartCanvas;
	public GameObject heartContainer;
	public GameObject heart;
	int count;
	List<GameObject> Hrts;
	List<GameObject> HrtsC;
	//public GameObject heartContainer;
	// Use this for initialization
	void Start () {
		if (Hrts == null)
			Hrts = new List<GameObject>();
		if (HrtsC == null)
			HrtsC = new List<GameObject> ();
			
		thisHeartCanvas = this.gameObject;
		PlayHP = GameObject.FindGameObjectWithTag ("PlayerMesh").GetComponent<PlayerHealth> ();

		for (int i = 0; i < PlayHP.StartHealth; i++) {
			GameObject _hC = (GameObject)Instantiate (heartContainer);
			_hC.transform.SetParent (thisHeartCanvas.transform);
			_hC.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_hC.GetComponent<RectTransform>().anchoredPosition = new Vector3(88.0f +(i * 100),-60.0f,0.0f);
			Hrts.Add (_hC);


			GameObject _H = (GameObject)Instantiate (heart);
			_H.transform.SetParent (thisHeartCanvas.transform);
			_H.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_H.GetComponent<RectTransform>().anchoredPosition = new Vector3(88.0f +(i * 100),-60.0f,0.0f);
			HrtsC.Add (_H);



		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (Hrts.Count);
		if (PlayHP.currentHealth < Hrts.Count) {
			Debug.Log ("Ouch!");
			//Hrts.GetRange(Hrts.Count)
			//Debug.Log(Hrts.GetRange(Hrts.Count-1,Hrts.Count).ToString);
			//(GameObject)Hrts[Hrts.Count-1].SetActive(false);

		}
	}
}
