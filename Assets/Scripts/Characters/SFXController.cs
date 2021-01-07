using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{

    [SerializeField] private AudioClip runningSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;


    public void playRunningSound()
    {
        if (!AudioManager.Instance.isSFXPlaying()) AudioManager.Instance.PlaySFX(runningSound);
    }

    public void playAttackSound()
    {
        AudioManager.Instance.PlaySFX(attackSound);
    }

    public void playDeathSound()
    {
        AudioManager.Instance.PlaySFX(deathSound);
    }
}
