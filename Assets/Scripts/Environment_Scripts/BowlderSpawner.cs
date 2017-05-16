using UnityEngine;
using System.Collections;

public class BowlderSpawner : MonoBehaviour {

    public GameObject BoulderPrefab;
    public Transform BoulderSpawn;

    GameObject ThePlayer;

    public float allotedTime = 6.0f;

    public bool DontUseTimer, DestroyBoulderWhenDeath;

    GameObject Boulder;

    public bool dropBoulder = true;
	// Use this for initialization
	void Start () {
        ThePlayer = GameObject.FindGameObjectWithTag("PlayerMesh");
	}
    void FindThePlayer() {
        ThePlayer  = GameObject.FindGameObjectWithTag("PlayerMesh");
    }

    // Update is called once per frame
    void Update () {
        //BoulderDrop();

        if (dropBoulder == true && Boulder == null)
        {
            Boulder = (GameObject)Instantiate(
                BoulderPrefab,
                BoulderSpawn.position,
                BoulderSpawn.rotation);
            dropBoulder = false;

        }

        if (Boulder != null)
        {
            if (!DontUseTimer)
                Destroy(Boulder, allotedTime);
            else {
                if (Boulder.GetComponent<TriggeredBoulder>().boulderDead) {
                    Destroy(Boulder);
                }
            }

            if (ThePlayer == null && DestroyBoulderWhenDeath)
                    Destroy(Boulder);
        }
        else {
            dropBoulder = true;
        }

        if (ThePlayer == null)
            FindThePlayer();
    }
}
