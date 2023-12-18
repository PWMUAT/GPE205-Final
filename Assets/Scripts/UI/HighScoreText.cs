using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class HighScoreText : MonoBehaviour
{
    public TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        int tmpScore = 0;
        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            tmpScore = GameManager.Instance.highScore;
        }

        //get TMP text
        scoreText = GetComponent<TMP_Text>();

        scoreText.text = "High Score: " + tmpScore;
    }
}
