using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    private GameObject currentTile;
    public SoundPlayer levelSound;

    // Start is called before the first frame update
    void Start()
    {
        levelSound = GetComponent<SoundPlayer>();

        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //set audio source from game manager reference
            levelSound.audioSource = GameManager.Instance.SFXAudio;
        }

        //spawn the map
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateMap()
    {
        //play sound
        levelSound.PlaySound();

        //spawn random tile in array
        GameObject roomObj = Instantiate(GetRandomRoomPrefab(), Vector3.zero, Quaternion.identity);

        //set parent
        roomObj.transform.parent = this.transform;

        //make current tile
        currentTile = roomObj;

        //give readable name
        roomObj.name = "Gameplay Tile";

        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //tell to start game
            GameManager.Instance.StartGame();
        }
    }
    public GameObject GetRandomRoomPrefab()
    {
        return tilePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length)];
    }

    public void SpawnNextMap()
    {
        //delete current map
        Destroy(currentTile);
        //spawn next map
        GenerateMap();
    }
}
