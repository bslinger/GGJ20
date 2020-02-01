using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Valve.VR;



public class Oxygen : SystemBase
{
    [SerializeField] List<Transform> cameraBlockers;

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
        float scale = Mathf.Lerp(0.5f, 0.1f, value);
        //    foreach (Transform blocker in cameraBlockers)
        //    {
        //        blocker.localScale = Vector3.one * scale;
        //    }
        SteamVR_Fade.Start(Color.black * value, 0);
        Debug.Log(Color.black * value);
    }
}

