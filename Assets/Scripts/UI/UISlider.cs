using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UISlider : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public string mixerName;

    public void Start()
    {
        VolumeChange();
    }

    public void VolumeChange()
    {
        //base slider value
        float newVolume = volumeSlider.value;
        if (newVolume <= 0)
        {
            newVolume = -80;
        }
        else
        {
            //log10 threshold 
            newVolume = Mathf.Log10(newVolume);
            //0-20db range
            newVolume = newVolume * 20;
        }

        audioMixer.SetFloat(mixerName, newVolume);
    }
}
