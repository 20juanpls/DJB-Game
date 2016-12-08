using UnityEngine;
using System.Collections;

public class NPC_Death : MonoBehaviour {

	public float deathTime;
	public float deathInterval;
	public float angleInterval;
	public float sizeInterval;
	public NPC_Follow nf;
	public GameObject collider;
	int count;
	bool flag;

	void Start(){
		if (deathTime == 0.0f){
			deathTime = 3.0f;
			Debug.Log("Deathtime not initialized, auto-setting to 3 in :" + this.ToString());
		}

		nf = this.gameObject.GetComponent<NPC_Follow>();
		if(nf == null){
			Debug.Log("NPC_Follow was not found!:" + this.ToString());
		}

		collider = this.transform.GetChild(2).gameObject;
		if (collider.name != "JumpCollider"){
			Debug.Log("Jump Collider not initalized! :" + this.ToString());
		}

		count = -1;
		flag = true;
	}

	public void activateDeath(){
		nf.enabled = false;
		collider.SetActive(false);
		if (flag){
			StartCoroutine(deathCycle(deathTime));
		}
		flag = false;
	}

	public IEnumerator deathCycle(float deathTime){

		float f = deathTime/deathInterval;
		count = Mathf.RoundToInt(f);

		while(deathTime >= 0){
			Debug.Log("Toot: " + deathTime);
			yield return new WaitForSeconds(deathInterval);
			deathTime -= deathInterval;
			//rotate alon the y axis every interval "angleInterval"
			this.transform.Rotate(new Vector3(0.0f,angleInterval,0.0f));
			//reduce size by "sizeInterval" every interval
			if (this.transform.localScale.magnitude <= 0.01){
				Destroy(this.gameObject);
			}
			else{
				this.transform.localScale -= new Vector3(sizeInterval,sizeInterval,sizeInterval);
			}
		}
	}



}
