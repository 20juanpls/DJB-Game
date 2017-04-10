using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManagementScript : MonoBehaviour {

    PlayerMovement_Ver2 MovementScript;
    PlayerKnockback PlayKnockScript;
    AudioSource PlayAudioSource;

    public AudioClip JumpSound, HitSound, DFallSound, HasFallSound;

    bool HitPlayClip, HPlayingClip , DFallPlayClip, DFallPlayingClip, GrndPlayClip, GrndPlayingClip;
	// Use this for initialization
	void Start () {
        MovementScript = this.GetComponent <PlayerMovement_Ver2 > ();
        PlayKnockScript = this.GetComponent<PlayerKnockback>();
        PlayAudioSource = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        //Sound when it jumps
        JumpSoundF();
        HitSoundF();
        DangerousFallSoundF();
        HasFallenSoundF();

    }

    void JumpSoundF() {
        if (MovementScript.JumpActiveButton)
        {
            PlayAudioSource.clip = JumpSound;
            PlayAudioSource.PlayOneShot(JumpSound);
        }
    }
    void HitSoundF() {
        if (PlayKnockScript.collided == true)
        {
            if (!HPlayingClip)
                HitPlayClip = true;
        }
        else
        {
            HPlayingClip = false;
        }
        if (HitPlayClip == true)
        {
            PlayAudioSource.clip = HitSound;
            PlayAudioSource.Play();
            HPlayingClip = true;
            HitPlayClip = false;
        }
    }

    void DangerousFallSoundF() {
        //Debug.Log(PlayKnockScript.DangerousFall);
        if (PlayKnockScript.DangerousFall == true)
        {
            if (!DFallPlayingClip)
                DFallPlayClip = true;
        }
        else
        {
            DFallPlayingClip = false;
        }
        if (DFallPlayClip == true)
        {
            PlayAudioSource.clip = DFallSound;
            PlayAudioSource.Play();
            DFallPlayingClip = true;
            DFallPlayClip = false;
        }
    }

    void HasFallenSoundF() {
        if (PlayKnockScript.HasFallen == true)
        {
            if (!GrndPlayingClip)
                GrndPlayClip = true;
        }
        else
        {
            GrndPlayingClip = false;
        }
        if (GrndPlayClip == true)
        {
            PlayAudioSource.clip = HasFallSound;
            PlayAudioSource.Play();
            GrndPlayingClip = true;
            GrndPlayClip = false;
        }
    }
}
