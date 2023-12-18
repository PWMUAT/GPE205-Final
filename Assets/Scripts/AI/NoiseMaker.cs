using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour {

    private float volumeDistance;
    private float noiseDuration;
    private bool taskCompleted = false;
    public bool makesSound;
    public Transform audioVisual;
    private Transform defaultVisualTransform;

    private void Start()
    {
        if (makesSound)
        {
            defaultVisualTransform = audioVisual;
        }
    }

    private void Update()
    {
        //count time if duration is left
        if(noiseDuration >= 0) 
        {
            IncrementTime();
        }
        //revert volume distance
        else
        {
            //be sure to only activate once
            if(!taskCompleted)
            {
                taskCompleted = true;
                volumeDistance = 0;
                if (makesSound)
                {
                    audioVisual.localScale = Vector3.zero;
                }
            }
        }
    }
    public float GetVolumeDistance()
    {
        return volumeDistance;
    }
    public void SetNoiseLevel(float level, float duration)
    {
        //set distance for noice to be heard
        volumeDistance = level;
        //set duration noise will last in seconds
        noiseDuration = duration;
        if (makesSound)
        {
            //set range object equal to range
            audioVisual.localScale = new Vector3(volumeDistance, volumeDistance, volumeDistance);
        }
        //allow update function to start
        taskCompleted = false;
    }
    private void IncrementTime()
    {
        //decrement total duration
        noiseDuration -= Time.deltaTime;
    }
}
