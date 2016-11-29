using UnityEngine;
using System.Collections;

public class ButtonActivator : MonoBehaviour {

	public GameObject activated;
	public bool oneUse;
	bool ded;

	void Start(){
		activated.SetActive (!activated.activeSelf);
		ded = false;
	}

	public void ButtonActivated(){
		if (ded == false) {
			Debug.Log ("fixin");
			activated.SetActive (!activated.activeSelf);
			if (oneUse) {
				ded = true;
				this.transform.parent.transform.GetChild (1).gameObject.SetActive (false);
			}
		}
	}

}
