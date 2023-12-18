using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerController : Controller
{
    public KeyCode moveUpKey;
    public KeyCode moveDownKey;
    public KeyCode moveRightKey;
    public KeyCode moveLeftKey;
    public KeyCode sneakKey;

    public int winScore;
    public bool canWin;
    public SoundPlayer winSound;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        winSound = GetComponent<SoundPlayer>();

        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //set audio source from game manager reference
            winSound.audioSource = GameManager.Instance.SFXAudio;

            //check if player slot is empty
            if (GameManager.Instance.player == null)
            {
                //make self player
                GameManager.Instance.player = this;
                score = 0;
            }
        }
    }

    public void OnDestroy()
    {
        base.Start();

        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //check if player exists
            if (GameManager.Instance.player != null)
            {
                //remove self as player
                GameManager.Instance.player = null;
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        ProcessInputs();
    }

    public void ProcessInputs()
    {
        if (pawn != null)
        {
            if (Input.GetKey(moveUpKey))
            {
                pawn.MoveUp();
            }

            if (Input.GetKey(moveDownKey))
            {
                pawn.MoveDown();
            }

            if (Input.GetKey(moveRightKey))
            {
                pawn.MoveRight();
            }

            if (Input.GetKey(moveLeftKey))
            {
                pawn.MoveLeft();
            }

            if(Input.GetKeyDown(sneakKey))
            {
                pawn.Sneak();
            }
        }
    }
    public override void AddScore(int modifyScore)
    {
        //add score to 
        base.AddScore(modifyScore);
        score = score + modifyScore;

        //if score hits threshhold
        if(score >= winScore && canWin)
        {
            //check if we have a GameManager
            if (GameManager.Instance != null)
            {
                //win game
                GameManager.Instance.ActivateWinScreen();
                //play win sound
                winSound.PlaySound();
            }
        }
    }
}
