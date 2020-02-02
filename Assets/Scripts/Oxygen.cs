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
    [SerializeField] float startFadeLevel;
    [SerializeField] float fadeTime;
    bool isFading;
    bool isAntiFading;
    bool isDead;

    [SerializeField] DialogueManager dialogueManager;

    protected override void UpdateMe()
    {
        if (isPowered)
        {
            if (!isAntiFading)
            {
                SteamVR_Fade.Start(Color.clear, fadeTime);
                isFading = false;
                isAntiFading = true;
            }
            Increase();
        }
        else
        {
            if (currentParameter < startFadeLevel && !isFading)
            {
                SteamVR_Fade.Start(Color.black, fadeTime);
                isFading = true;
                isAntiFading = false;
            }
            Decrease();
        }
        if (currentParameter == 0)
        {
            Fail();
        }
    }

    protected override void UpdateUI()
    {
        float proportionalValue = (currentParameter - minParameter) / (maxParameter - minParameter);
        if (slider)
        {
            slider.value = proportionalValue;

        }

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

    void Fail()
    {
        if (!isDead)
        {
            isDead = true;
            if (dialogueManager)
            {
                dialogueManager.FailWithNode("FailState-Oxygen");
            }
        }
    }
}

