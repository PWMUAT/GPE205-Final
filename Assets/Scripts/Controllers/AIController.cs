using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.UIElements;
using System;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[System.Serializable]
public class AIController : Controller
{
    //define variables
    private float lastStateChangeTime;
    public float hearingDistance;
    public Transform hearingVisual;
    public UnityEngine.UI.Image sightVisual;
    public Transform sightVisualTransform;
    public float fieldOfView;
    public float viewDistance;
    public float waypointStopDistance;
    protected bool doUpdate = true;
    public float waitTime;
    private float waitedTime;
    public float loseInterestTime;
    private float timeLostSight;

    //variables for collision detection
    private float steeringAmount = 1f;
    public float collisionRange = 3f;

    public GameObject target;

    public Transform[] waypoints;
    private int currentWaypoint = 0;

    public enum AIState { Idle, Chase, ChooseTarget, Patrol, Wait};
    public AIState currentState;

    // Start is called before the first frame update
    public override void Start()
    {
        // Run the parent (base) Start
        base.Start();

        //make hearing visual the size of hearing range
        hearingVisual.localScale = new Vector3(hearingDistance, hearingDistance, hearingDistance);

        //make sight visual angle of sight
        sightVisual.fillAmount = fieldOfView*2/360;

        //fix sight visual angle
        sightVisualTransform.Rotate(new Vector3(0,0,fieldOfView));

        //fix sight visual scale
        sightVisualTransform.localScale = new Vector3(viewDistance, viewDistance, viewDistance);


        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //faster if hard mode
            if (GameManager.Instance.hardMode)
            {
                pawn.moveSpeed = pawn.moveSpeed * 2;
                pawn.turnSpeed = pawn.turnSpeed * 2;
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (doUpdate)
        {
            // Make decisions
            MakeDecisions();
            // Run the parent (base) Update
            base.Update();
        }
    }

    public virtual void MakeDecisions()
    {
        if (target == null)
        {
            ChangeState(AIState.ChooseTarget);
        }
        switch (currentState)
        {
            case AIState.Idle:
                // Do work 
                DoIdleState();
                // Check for transitions
                if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Chase:
                // Do work
                Seek(target);
                // Check for transitions
                if (!CanSee(target) && !CanHear(target))
                {
                    //if AI loses interest after losing sight of player
                    if(timeLostSight >= loseInterestTime)
                    {
                        //swap back to patrol
                        ChangeState(AIState.Patrol);
                    }
                    else
                    {
                        //increment time
                        timeLostSight += Time.deltaTime;
                    }
                }
                else
                {
                    //reset time  since player was last seen
                    timeLostSight = 0;
                }
                break;
            case AIState.ChooseTarget:
                //Do work
                TargetPlayer();
                break;
            case AIState.Patrol:
                // Do work 
                Patrol();
                // Check for transitions
                if (CanSee(target) || CanHear(target))
                {
                    //reset time  since player was last seen
                    timeLostSight = 0;
                    //swap to chasing player
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Wait:
                DoWaitState();
                break;
            default:
                ChangeState(AIState.ChooseTarget);
                break;
        }
    }
    public virtual void ChangeState(AIState newState)
    {
        // Change the current state
        currentState = newState;
        // Save the time when we changed states
        lastStateChangeTime = Time.time;

    }
    public void TargetPlayer()
    {
        // If the GameManager exists
        if (GameManager.Instance != null)
        {
            // And the array of players exists
            if (GameManager.Instance.player != null)
            {
                //Then target the gameObject of the pawn of the player controller
                target = GameManager.Instance.player.pawn.gameObject;
                ChangeState(AIState.Patrol);
            }
        }
    }
    protected bool IsHasTarget()
    {
        // return true if we have a target, false if we don't
        return (target != null);
    }
    #region Seek
    public void Seek(GameObject target)
    {
        // Seek the position of our target Transform
        Seek(target.transform.position);
    }
    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target Transform
        Seek(targetTransform.position);
    }
    public void Seek(Pawn targetPawn)
    {
        // Seek the pawn's Transform
        Seek(targetPawn.transform);
    }
    public void Seek(Vector3 targetPosition) //this is the one all the others refer to
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(targetPosition, steeringAmount);
        // Move Forward
        pawn.MoveForwards();

    }
    #endregion

    #region OnlyRoatateTowards
    public void OnlyRoatateTowards(GameObject target)
    {
        // Seek the position of our target Transform
        OnlyRoatateTowards(target.transform.position);
    }
    public void OnlyRoatateTowards(Transform targetTransform)
    {
        // Seek the position of our target Transform
        OnlyRoatateTowards(targetTransform.position);
    }
    public void OnlyRoatateTowards(Pawn targetPawn)
    {
        // Seek the pawn's transform!
        OnlyRoatateTowards(targetPawn.transform);
    }
    public void OnlyRoatateTowards(Vector3 targetPosition) //this is the one all the others refer to
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(targetPosition, steeringAmount);

    }
    #endregion

    protected virtual void DoChaseState()
    {
        Seek(target);
    }
    protected virtual void DoIdleState()
    {
        // Do Nothing
    }
    protected bool IsDistanceLessThan(GameObject target, float distance)
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void Patrol()
    {
        // If we have a enough waypoints in our list to move to a current waypoint
        if (waypoints.Length > currentWaypoint)
        {
            //debug line to waypoint
            Debug.DrawLine(pawn.transform.position, waypoints[currentWaypoint].position, color: Color.green);

            //If we are close enough, then increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {
                //set next waypoint
                currentWaypoint++;
            }
            else
            {
                //get looking vector to waypoint
                Vector3 vectorToWaypoint = waypoints[currentWaypoint].position - pawn.transform.position;
                //find the angle between the direction forwards and the vector to the target
                float angleToWaypoint = Vector3.Angle(vectorToWaypoint, pawn.transform.forward);

                //if looking angle is close enough
                if (angleToWaypoint < 5)
                {
                    //seek that waypoint
                    Seek(waypoints[currentWaypoint]);
                }
                else
                {
                    //only rotate at it
                    OnlyRoatateTowards(waypoints[currentWaypoint]);
                }
            }
        }
        else
        {
            RestartPatrol();
        } 
    }
    protected void RestartPatrol()
    {
        // Set the index to 0
        currentWaypoint = 0;
    }
    public void DoWaitState()
    {
        if(waitedTime >= waitTime)
        {
            ChangeState(AIState.Chase);
            waitedTime = 0;
        }
        else
        {
            waitedTime += Time.deltaTime;
        }
    }
    public bool CanHear(GameObject target)
    {
        // Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();
        // If they don't have one, they can't make noise, so return false
        if (noiseMaker == null)
        {
            return false;
        }
        // If they are making 0 noise, they also can't be heard
        if (noiseMaker.GetVolumeDistance() <= 0)
        {
            return false;
        }
        // If they are making noise, add the volumeDistance in the noisemaker to the hearingDistance of this AI
        float totalDistance = noiseMaker.GetVolumeDistance() + hearingDistance;
        // If the distance between our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            // ... then we can hear the target
            return true;
        }
        else
        {
            // Otherwise, we are too far away to hear them
            return false;
        }
    }
    public bool CanSee(GameObject target)
    {
        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - pawn.transform.position;
        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);
        //raycast from AI to player
        //do raycast and assign to bool
        RaycastHit hit;
        bool hasSightLine = false;
        Physics.Raycast(pawn.transform.position, agentToTargetVector, out hit, viewDistance);
        //set sight line to true if raycast hits player
        if(hit.collider != null)
        {
            if (hit.collider.gameObject == target)
            {
                hasSightLine = true;
            }
        }
        Debug.DrawRay(pawn.transform.position, agentToTargetVector);
        // if that angle is less than our field of view and has line of sight
        if (angleToTarget < fieldOfView && hasSightLine)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    public void CollisionDetect()
    {
        Vector3 offsetAngle = pawn.transform.forward;
        //do multiple raycasts in front of AI and test which hits shortest
        float closestHit = collisionRange;
        RaycastHit hit;
        Physics.Raycast(pawn.transform.position, offsetAngle, out hit, collisionRange);
        Debug.DrawLine(pawn.transform.position, pawn.transform.position + offsetAngle*collisionRange + new Vector3(0f, 0.5f, 0f), color:Color.red);

        if(hit.collider != null)
        {
            steeringAmount = collisionRange / hit.distance;
        }
    }
    public override void AddScore(int modifyScore)
    {
        base.AddScore(modifyScore);
        score += modifyScore;
    }
}
