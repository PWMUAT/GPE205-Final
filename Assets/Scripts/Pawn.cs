using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    /// <summary>
    /// Variable for movement speed
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// Variable for rotation speed
    /// </summary>
    public float turnSpeed;

    //controller variable
    public Controller controller;

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void MoveForwards();
    public abstract void MoveUp();
    public abstract void MoveDown();
    public abstract void MoveRight();
    public abstract void MoveLeft();
    public abstract void RotateClockwise();
    public abstract void RotateCounterClockwise();
    public abstract void RotateTowards(Vector3 targetPosition, float steerMultiplier);
    public abstract void Sneak();
}
