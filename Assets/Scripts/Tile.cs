using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private MapGenerator mapController;

    // Start is called before the first frame update
    void Start()
    {
        if (FindFirstObjectByType<MapGenerator>() != null)
        {
            mapController = FindFirstObjectByType<MapGenerator>();
        }
    }

    public void TileBeaten()
    {
        //tell to spawn next map
        mapController.SpawnNextMap();
    }
}
