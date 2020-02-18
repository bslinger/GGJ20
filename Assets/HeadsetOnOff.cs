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
    [SerializeField] bool overrideHeadset = false;
    

    // Start is called before the first frame update
    void Start()
    {

        activeAction.AddOnStateDownListener(this.HeadsetOn, input);
        activeAction.AddOnStateUpListener(this.HeadsetOff, input);
    }

    private void Update()
    {
        if(overrideHeadset)
        {
            enableHeadset();
        }
    }

    void enableHeadset()
    {
        ExternalCamera.enabled = false;
        canvas.enabled = false;
    }

    void disableHeadset()
    {
        ExternalCamera.enabled = true;
        canvas.enabled = true;
    }

    void HeadsetOn(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        enableHeadset();
    }

    void HeadsetOff(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        disableHeadset();
    }
}
