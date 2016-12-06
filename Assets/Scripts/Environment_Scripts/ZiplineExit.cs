using UnityEngine;
using System.Collections;

public class ZiplineExit : MonoBehaviour {

  GameObject player;
  public float setDistance;
  GameObject exit;
  GameObject winCan;

  void Start(){
    player = GameObject.Find("Player");
    if (player == null){
      Debug.Log("!!player unassigned!!: " + this.ToString());
    }

    exit = GameObject.Find("ExitCanvas");
    if(exit == null){
      Debug.Log("the exit canvas failed to initialize/assign :" + this.ToString());
    }
    exit.SetActive(false);

    if (setDistance == 0.0f){
      Debug.Log("setDistance undefined; autoset to 10.0f");
      setDistance = 10.0f;
    }

    winCan = GameObject.Find("WinCanvas");
    if (winCan == null){
      Debug.Log("win canvas faield to initailize/assign :" + this.ToString());
    }
    winCan.SetActive(false);
  }

  void Update(){
    if (Vector3.Distance(player.transform.position, this.transform.position)
    <= setDistance){
      //Debug.Log("Player is within range of zipline");
      exit.SetActive(true);
    }
    else{
      exit.SetActive(false);
    }
  }

  public void EndGame(){
    winCan.SetActive(true);
  }

    public void AssignPlayer(GameObject p)
    {
        player = p;
    }





  //public void Bump(){
    //player.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,1000.0f,0.0f));
  //}


}
