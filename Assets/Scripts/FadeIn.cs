﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FadeIn : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
