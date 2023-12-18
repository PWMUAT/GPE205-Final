using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    //play sound
    public void PlaySound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
