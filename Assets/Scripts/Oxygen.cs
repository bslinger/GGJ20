using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Oxygen : SystemBase
{
    [SerializeField] Vignette vingette;

    protected override void StartMe() {
        vingette = GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>();
        Debug.Log(vingette);
    }


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

        if(currentParamater < startingParamater)
        {
            float value = (startingParamater - currentParamater) / (startingParamater - minParamater);
            vingette.intensity.Override(value);
        }

    }
}
