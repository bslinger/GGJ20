using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FadeIn : MonoBehaviour
{
    [SerializeField] float fadeTime = 2f; 

    // Start is called before the first frame update
    void Start()
    {
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
