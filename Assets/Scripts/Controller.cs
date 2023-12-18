using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    /// <summary>
    /// The pawn that the controller is in charge of.
    /// </summary>
    public Pawn pawn;
    public int score;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void AddScore(int modifyScore)
    {

    }
}
