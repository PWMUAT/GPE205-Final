using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Collider))]
public class LoseTrigger : MonoBehaviour
{
    public SoundPlayer failSound;

    private void Start()
    {
        failSound = GetComponent<SoundPlayer>();
        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //set audio source from game manager reference
            failSound.audioSource = GameManager.Instance.SFXAudio;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //check if has pawn component
        if (other.GetComponent<RBPawn>() != null && other != this)
        {
            //get access to controller
            RBPawn pawn = other.GetComponent<RBPawn>();
            //check if is player
            if(pawn.controller.GetType() == typeof(PlayerController))
            {
                //play sound
                failSound.PlaySound();
                //lose game
                //check if we have a GameManager
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.LoseGame();
                }
            }
        }
    }
}
