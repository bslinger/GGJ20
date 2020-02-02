using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Valve.VR;
using UnityEngine.UI;



public class Oxygen : SystemBase
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject alarm;

    protected override void UpdateMe()
    {
        if (isPowered)
        {
            Increase();
        }
        else
        {
            Decrease();
        }

        float value = (startingParameter - currentParameter) / (startingParameter - minParameter);

        
    }

    protected override void UpdateUI()
    {
        float proportionalValue = (currentParameter - minParameter) / (maxParameter - minParameter);
        if (slider)
        {
            slider.value = proportionalValue;

        }

        SteamVR_Fade.Start(Color.black * proportionalValue, 0);

        if (alarm != null)
        {
            if (proportionalValue <= 0.2f)
            {
                //             Debug.Log("Turning alam on");
                alarm.SetActive(true);
            }
            else
            {
                //            Debug.Log("Turning alam off");
                alarm.SetActive(false);
            }
        }
    }
}

