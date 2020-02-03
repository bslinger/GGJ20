using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class HeadsetOnOff : MonoBehaviour
{
    public SteamVR_Action_Boolean activeAction;
    [SerializeField] SteamVR_Input_Sources input;
    [SerializeField] Camera ExternalCamera;
    [SerializeField] Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        if (activeAction.GetActive(input))
        {
            enableHeadset();
        }
        activeAction.AddOnStateDownListener(this.HeadsetOn, input);
        activeAction.AddOnStateUpListener(this.HeadsetOff, input);
    }

    void enableHeadset()
    {
        ExternalCamera.enabled = false;
        canvas.enabled = false;
    }

    void HeadsetOn(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        enableHeadset();
    }

    void HeadsetOff(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ExternalCamera.enabled = true;
        canvas.enabled = true;
    }
}
