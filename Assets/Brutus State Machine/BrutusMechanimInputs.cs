using UnityEngine;
using System.Collections;

public class BrutusMechanimInputs : MonoBehaviour 
{
    public Animator thisAnimator;

    void Start () 
    {
        thisAnimator = GetComponent<Animator>();
    }

    void Update () 
    {
        if(Input.GetButtonDown("Fire1"))
        {
            thisAnimator.SetTrigger("Runs");
        }

        if(Input.GetButtonDown("Fire2"))
        {
            thisAnimator.SetTrigger("Stops");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            thisAnimator.SetBool("Jumps", true);
        }
        else
        {
            thisAnimator.SetBool("Jumps", false);
        }
    }
}