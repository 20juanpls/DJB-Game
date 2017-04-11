using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyNPC_SoundManagement : MonoBehaviour {
    FlyingNPC_HeadMovement NPCFlyS;
    AudioSource NPCAudioSource;

    public AudioClip DeathSound, IntrudedSound;

    bool  DeathPlayClip, DeathPlayingClip, AtPlayClip, AtPlayingClip, ExPlayClip, ExPlayingClip;
    // Use this for initialization
    void Start()
    {
        NPCFlyS = this.GetComponent<FlyingNPC_HeadMovement>();
        NPCAudioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NPCFlyS.IsDead != true)
        {
            IntrudedSpaceF();
            ExitIntrudedSpece();
        }

        FlyNPCDeathF();
    }

    void FlyNPCDeathF()
    {
        if (NPCFlyS.IsDead == true)
        {
            if (!DeathPlayingClip)
                DeathPlayClip = true;
        }
        else
        {
            DeathPlayingClip = false;
        }

        if (DeathPlayClip == true)
        {
            NPCAudioSource.clip = DeathSound;
            NPCAudioSource.Play();
            DeathPlayingClip = true;
            DeathPlayClip = false;
        }
    }
    void IntrudedSpaceF() {
        if (NPCFlyS.PlayerHasIntruded/*Attack*/ == true)
        {
            if (!AtPlayingClip)
                AtPlayClip = true;
        }
        else
        {
            AtPlayingClip = false;
        }

        if (AtPlayClip == true)
        {
            NPCAudioSource.clip = IntrudedSound;
            NPCAudioSource.Play();
            AtPlayingClip = true;
            AtPlayClip = false;
        }
    }
    void ExitIntrudedSpece() {
        if (NPCFlyS.PlayerHasIntruded/*Attack*/ == false)
        {
            if (!ExPlayingClip)
                ExPlayClip = true;
        }
        else
        {
            ExPlayingClip = false;
        }

        if (ExPlayClip == true)
        {
            //NPCAudioSource.clip = DeathSound;
            NPCAudioSource.Stop();
            ExPlayingClip = true;
            ExPlayClip = false;
        }
    }
}
