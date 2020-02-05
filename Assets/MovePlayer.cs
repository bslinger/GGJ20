using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] Transform IdealHeadPosition;
    [SerializeField] Transform Player;
    [SerializeField] Transform Head;
    [SerializeField] SteamVR_Action_Boolean RecenterAction = SteamVR_Input.GetBooleanAction("Recenter");
    [SerializeField] bool startRecentered;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if(startRecentered) Recenter();
    }

    // Update is called once per frame
    void Update()
    {
        if(RecenterAction.GetStateDown(SteamVR_Input_Sources.LeftHand)|| RecenterAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Recenter();
        }
    }

    void Recenter()
    {
        offset = IdealHeadPosition.position - Head.position;
        Player.position += offset;
    }
}
