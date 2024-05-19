using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to trigger when voice instructions are played by animator
public class InstructionsController : MonoBehaviour
{
    [SerializeField] Animator breathingAnimator;

    [SerializeField] AudioSource instructionsAudio;
    [SerializeField] AudioClip introductionClip;
    [SerializeField] AudioClip stage1InstructionsClip;
    [SerializeField] AudioClip breatheOutInstructionsClip;
    [SerializeField] AudioClip stage1CompleteClip;
    [SerializeField] AudioClip startBreatheOutClip;
    [SerializeField] AudioClip breatheOutClip;
    [SerializeField] AudioClip breatheInRound1Clip;
    [SerializeField] AudioClip breatheInRound2Clip;
    [SerializeField] AudioClip breatheInRound3Clip;
    [SerializeField] AudioClip breatheInRound4Clip;
    [SerializeField] AudioClip breatheFailedClip;
    [SerializeField] AudioClip holdBreathClip;

    public void playIntroductionInstructions()
    {
        instructionsAudio.PlayOneShot(introductionClip);
    }

    public void playStage1Instructions()
    {
        instructionsAudio.PlayOneShot(stage1InstructionsClip);
    }

    public void playbreatheOutInstructions()
    {
        instructionsAudio.PlayOneShot(breatheOutInstructionsClip);
    }

    public void playStageCompleteInstructions()
    {
        instructionsAudio.PlayOneShot(stage1CompleteClip);
    }

    public void playBreatheOut()
    {
        instructionsAudio.PlayOneShot(breatheOutClip);
    }

    public void playStartBreatheOut()
    {
        instructionsAudio.PlayOneShot(startBreatheOutClip);
    }

    public void playBreatheIn()
    {
        switch (breathingAnimator.GetInteger("BreathNumber"))
        {
            case 0:
                instructionsAudio.PlayOneShot(breatheInRound1Clip);
                break;
            case 1:
                instructionsAudio.PlayOneShot(breatheInRound2Clip);
                break;
            case 2:
                instructionsAudio.PlayOneShot(breatheInRound3Clip);
                break;
            case 3:
                instructionsAudio.PlayOneShot(breatheInRound4Clip);
                break;
        }
    }

    public void playHoldBreath()
    {
        instructionsAudio.PlayOneShot(holdBreathClip);
    }

    public void playBreatheFailed()
    {
        instructionsAudio.Stop();
        instructionsAudio.PlayOneShot(breatheFailedClip);
    }
}
