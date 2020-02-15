using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        float startDelay = 0f;
        float interval = 0.75f;
        InvokeRepeating("BlinkIt", startDelay, interval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlinkIt()
    {
        Debug.Log("BlinkIt");
        canvasGroup.alpha = ((canvasGroup.alpha == 1) ? 0 : 1);
    }
    
}
