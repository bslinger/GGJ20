using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Valve.VR;
using UnityEngine.UI;



public class Oxygen : SystemBase
{
    [SerializeField] Slider slider;

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

        SteamVR_Fade.Start(Color.black * value, 0);
    }

    protected override void UpdateUI()
    {
        float proportionalValue = (maxParameter - currentParameter) / (maxParameter - minParameter);
        if (slider)
        {
            slider.value = proportionalValue;

        }
    }
}

