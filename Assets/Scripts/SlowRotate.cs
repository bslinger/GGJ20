using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlowRotate : SystemBase
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 rotationAxis;
    [SerializeField] GameObject alarm;

    // Update is called once per frame
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
        target.Rotate(rotationAxis * (0.1f - currentParameter));
        //statusText.text = Math.Round((0.1f - currentParameter) * 1000) + "%";
    }

    protected override void UpdateUI()
    {
        if (alarm)
        {
            if (proportionalValue <= 0.8f)
            {
                alarm.SetActive(true);
            }
            else
            {
                alarm.SetActive(false);
            }
        }
    }
}
