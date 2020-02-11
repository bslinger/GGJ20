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
    [SerializeField] Slider slider;
    [SerializeField] GameObject alarm;
    public TextMeshProUGUI statusText;

    // Update is called once per frame
    protected override void UpdateMe()
    {
        if (isPowered)
        {
            Decrease();
        }
        else
        {
            Increase();
        }
        target.Rotate(rotationAxis * currentParameter);
        statusText.text = Math.Round((0.1f - currentParameter) * 1000) + "%";
    }

    protected override void UpdateUI()
    {
        float proportionalValue = (maxParameter - currentParameter) / (maxParameter - minParameter);
        if (slider) {
            slider.value = proportionalValue;

        }
        if (alarm)
        {
            if (proportionalValue <= 0.2f)
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
