using UnityEngine;
using System.Collections;

public class BowlderSpawner : MonoBehaviour {

    public GameObject BoulderPrefab;
    public Transform BoulderSpawn;

    GameObject Boulder;

    public bool dropBoulder = true;
	// Use this for initialization
	void Start () {
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
            Destroy(Boulder, 6.0f);
        }
        else {
            dropBoulder = true;
        }
    }
}
