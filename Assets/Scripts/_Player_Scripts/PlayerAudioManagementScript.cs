using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManagementScript : MonoBehaviour {

    PlayerMovement_Ver2 MovementScript;
    PlayerKnockback PlayKnockScript;
    AudioSource PlayAudioSource;
    PlayerNPCKill PlayNPCK;

    public AudioClip JumpSound, DoubleJumpSound, HitSound, DFallSound, HasFallSound, JumpO_s1, JumpO_s2, JumpO_s3;

    public ArrayList JumpedOnSounds;

    AudioClip CurrJumpO;

    bool HitPlayClip, HPlayingClip , DFallPlayClip, DFallPlayingClip, GrndPlayClip, GrndPlayingClip, JPC, JPingC;

    int countJO;
	// Use this for initialization
	void Start () {
        MovementScript = this.GetComponent <PlayerMovement_Ver2 > ();
        PlayKnockScript = this.GetComponent<PlayerKnockback>();
        PlayAudioSource = this.GetComponent<AudioSource>();
        PlayNPCK = this.GetComponent<PlayerNPCKill>();

        if (JumpedOnSounds == null)
            JumpedOnSounds = new ArrayList();

        JumpedOnSounds.Add(JumpO_s1);
        JumpedOnSounds.Add(JumpO_s2);
        JumpedOnSounds.Add(JumpO_s3);
    }
	
	// Update is called once per frame
	void Update () {
        //Sound when it jumps
        JumpSoundF();
        DoubleJumpSoundF();
        HitSoundF();
        DangerousFallSoundF();
        HasFallenSoundF();
        JumpedOnSoundF();

    }

    void JumpSoundF() {
        if (MovementScript.JumpActiveButton)
        {
            PlayAudioSource.clip = JumpSound;
            PlayAudioSource.PlayOneShot(JumpSound);
        }
    }
    void DoubleJumpSoundF() {
        if (MovementScript.DJumpActive) {
            PlayAudioSource.clip = DoubleJumpSound;
            PlayAudioSource.PlayOneShot(DoubleJumpSound);
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
    void JumpedOnSoundF()
    {
        if (PlayNPCK.InCollider == true)
        {
            if (!JPingC)
                JPC = true;
        }
        else
        {
            JPingC = false;
        }
        if (JPC == true)
        {    
            AudioClip currentJOsound = (AudioClip)JumpedOnSounds[countJO];
            PlayAudioSource.clip = currentJOsound;
            PlayAudioSource.Play();

            countJO++;
            if (countJO > 2)
            {
                countJO = 0;
            }

            JPingC = true;
            JPC = false;
        }
    }
}
