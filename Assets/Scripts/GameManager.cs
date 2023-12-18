using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    #region variables
    /// <summary>
    /// The static instance of this class which can only exist once
    /// </summary>
    public static GameManager Instance;

    public PlayerController player;
    public PlayerSpawn playerSpawn;
    private GameObject mapSpawnerObject;
    public int highScore;
    public Camera gameCamera;
    public AudioSource menuMusic;
    public AudioSource gameMusic;
    public AudioSource SFXAudio;
    public bool hardMode;

    //prefabs
    public PlayerController playerControllerPrefab;
    public GameObject playerPawnPrefab;
    public GameObject MapGeneratorPrefab;
    #endregion

    //game States
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;
    public GameObject WinScreenStateObject;

    private void Awake()
    {
        //if we dont have an instance, make one
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //if another instance exists, delete this one
        else
        {
            Destroy(gameObject);
        }

        highScore = PlayerPrefs.GetInt("Highscore", 0);
    }

    private void Start()
    {
        DeactivateAllStates();
        ActivateTitleScreen();
    }
    public void StartGame()
    {
        if (player == null)
        {
            SpawnPlayer();
        }
        else
        {
            RespawnPlayer();
        }
    }
    public void SpawnPlayer()
    {
        //get player spawnpoint
        playerSpawn = FindFirstObjectByType<PlayerSpawn>();

        //spawn controller at world origin
        GameObject newPlayerObject = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity).gameObject;
        newPlayerObject.transform.parent = GameplayStateObject.transform;

        //spawn the pawn and connect it to controller
        GameObject newPawnObject = Instantiate(playerPawnPrefab, playerSpawn.transform.position, playerSpawn.transform.rotation);
        newPawnObject.transform.parent = mapSpawnerObject.transform;

        //get PlayerController and Pawn components
        Controller newController = newPlayerObject.GetComponent<Controller>();
        Pawn newPawn = newPawnObject.GetComponent<Pawn>();

        //hook components up
        newController.pawn = newPawn;
        newPawn.controller = newController;
    }
    public void RespawnPlayer()
    {
        //destroy player pawn
        Destroy(player.pawn.gameObject);

        //get player spawnpoint
        playerSpawn = FindFirstObjectByType<PlayerSpawn>();

        //spawn new pawn and connect it to controller
        GameObject newPawnObject = Instantiate(playerPawnPrefab, playerSpawn.transform.position, playerSpawn.transform.rotation);
        newPawnObject.transform.parent = mapSpawnerObject.transform;

        //get PlayerController and Pawn components
        Controller newController = player.GetComponent<Controller>();
        Pawn newPawn = newPawnObject.GetComponent<Pawn>();

        //hook components up
        newController.pawn = newPawn;
        newPawn.controller = newController;
    }


    private void DeactivateAllStates()
    {
        //Deactivate all states
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverScreenStateObject.SetActive(false);
        DestroyMap();
        if(player != null)
        {
            Destroy(player.gameObject);
        }
    }
    public void ActivateTitleScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only title screen
        TitleScreenStateObject.SetActive(true);
        //play music
        menuMusic.Play();
    }
    public void ActivateMainMenuScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only main menu
        MainMenuStateObject.SetActive(true);
    }
    public void ActivateOptionsMenuScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only options screen
        OptionsScreenStateObject.SetActive(true);
    }
    public void ActivateCreditsScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only credits screen
        CreditsScreenStateObject.SetActive(true);
    }
    public void ActivateGameplayState()
    {
        //stop and play music
        menuMusic.Stop();
        gameMusic.Play();
        //Deactivate states
        DeactivateAllStates();
        //Activate only gameplay
        GameplayStateObject.SetActive(true);
        //start the gameplay
        mapSpawnerObject = Instantiate(MapGeneratorPrefab, Vector3.zero, Quaternion.identity).gameObject;
        mapSpawnerObject.transform.parent = GameplayStateObject.transform;
    }
    public void ActivateGameOverScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only game over screen
        GameOverScreenStateObject.SetActive(true);
        //set player prefs to save score
        PlayerPrefs.SetInt("Highscore", highScore);
        PlayerPrefs.Save();
    }
    public void ActivateWinScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only win screen
        WinScreenStateObject.SetActive(true);
        //set player prefs to save score
        PlayerPrefs.SetInt("Highscore", highScore);
        PlayerPrefs.Save();
    }
    public void DestroyMap()
    {
        //destroy map spawner and all its children
        //...that sounds bad but its funny so im leavin it
        Destroy(mapSpawnerObject);
    }

    public void LoseGame()
    {
        //if current score is more than high score
        if(player.score > highScore)
        {
            //set highscore
            highScore = player.score;
        }

        //activate game over
        ActivateGameOverScreen();
    }
}
