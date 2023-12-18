using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NoiseMaker))]
public class RBPawn : Pawn
{
    //rigidbody for player
    private Rigidbody pawnRB;

    //sound component for making AI hear sound
    private NoiseMaker noiseComponent;
    public float stealthNoiseRange = 1;
    public float stealthSpeedMultiplier = 1;
    private float noiseRange = 3;
    private float walkMultiplier = 1;
    private bool isSneaking = false;
    private float defaultWalkMultiplier;
    private float defaultNoiseRange;

    public override void Start()
    {
        //getting rigidbody from self
        pawnRB = GetComponent<Rigidbody>();
        
        //getting noise component from self
        noiseComponent = GetComponent<NoiseMaker>();

        //set internal defaults
        defaultWalkMultiplier = walkMultiplier;
        defaultNoiseRange = noiseRange;
    }

    public override void Sneak()
    {
        //set to inverse of what it was
        isSneaking = !isSneaking;

        if (isSneaking)
        {
            //smaller range when sneaking
            noiseRange = stealthNoiseRange;
            //slower speed when sneaking
            walkMultiplier = stealthSpeedMultiplier;
        }
        else
        {
            //default range when not
            noiseRange = defaultNoiseRange;
            //default speed when not
            walkMultiplier = defaultWalkMultiplier;
        }
    }

    public override void MoveUp()
    {
        //Create vector for movement
        Vector3 upMovement = new Vector3(0, 0, 1) * moveSpeed * walkMultiplier / Time.deltaTime;
        //add force to rigidbody
        pawnRB.AddForce(upMovement);
        //make noise when moving
        noiseComponent.SetNoiseLevel(noiseRange, 0.01f);
    }

    public override void MoveDown()
    {
        //Create vector for movement
        Vector3 downMovement = new Vector3(0, 0, -1) * moveSpeed * walkMultiplier / Time.deltaTime;
        //add force to rigidbody
        pawnRB.AddForce(downMovement);
        //make noise when moving
        noiseComponent.SetNoiseLevel(noiseRange, 0.01f);
    }

    public override void MoveLeft()
    {
        //Create vector for movement
        Vector3 leftMovement = new Vector3(-1, 0, 0) * moveSpeed * walkMultiplier / Time.deltaTime;
        //add force to rigidbody
        pawnRB.AddForce(leftMovement);
        //make noise when moving
        noiseComponent.SetNoiseLevel(noiseRange , 0.01f);
    }

    public override void MoveRight()
    {
        //Create vector for movement
        Vector3 rightMovement = new Vector3(1, 0, 0) * moveSpeed * walkMultiplier / Time.deltaTime;
        //add force to rigidbody
        pawnRB.AddForce(rightMovement);
        //make noise when moving
        noiseComponent.SetNoiseLevel(noiseRange, 0.01f);
    }

    public override void MoveForwards()
    {
        //Create vector for movement
        Vector3 forwardsMovement = gameObject.transform.forward * moveSpeed * Time.deltaTime;
        //move rigidbody
        pawnRB.MovePosition(gameObject.transform.position + forwardsMovement);
    }

    public override void RotateClockwise()
    {
        //Create vector for rotation
        Vector3 vectorRotation = new Vector3(0, turnSpeed, 0);
        //create rotation quaternion from rotator
        Quaternion clockwiseRotation = Quaternion.Euler(vectorRotation * Time.deltaTime);
        //rotate rigidbody
        pawnRB.MoveRotation(pawnRB.rotation * clockwiseRotation);
    }

    public override void RotateCounterClockwise()
    {
        //Create vector for rotation
        Vector3 vectorRotation = new Vector3(0, -turnSpeed, 0);
        //create rotation quaternion from rotator
        Quaternion counterClockwiseRotation = Quaternion.Euler(vectorRotation * Time.deltaTime);
        //rotate rigidbody
        pawnRB.MoveRotation(pawnRB.rotation * counterClockwiseRotation);
    }

    public override void RotateTowards(Vector3 targetPosition, float steerMultiplier)
    {
        //vector pointing at target
        Vector3 vectorToTarget = targetPosition - transform.position;
        //making Quaternion out of vector
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        //rotating towards vector
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * steerMultiplier * Time.deltaTime);
    }
}
