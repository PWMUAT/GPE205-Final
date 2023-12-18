using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WinTrigger : MonoBehaviour
{
    public Tile parentTile;

    public void OnTriggerEnter(Collider other)
    {
        //check if has pawn component
        if (other.GetComponent<RBPawn>() != null)
        {
            //get access to controller and add score
            RBPawn player = other.GetComponent<RBPawn>();
            player.controller.AddScore(1);

            //call spawn for next map
            parentTile.TileBeaten();
        }
    }

}
