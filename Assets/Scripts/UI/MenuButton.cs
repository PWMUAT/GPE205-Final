using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Toggle toggleState;

    public void QuitGame()
    {

        #if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }

        #else 
        Application.Quit();
        #endif
    }

    public void ToggleHardMode()
    {
        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.hardMode = toggleState.isOn;
        }
    }
}
