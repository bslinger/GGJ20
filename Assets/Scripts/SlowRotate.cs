using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowRotate : SystemBase
{
    [SerializeField] Transform target;
    [SerializeField] Slider slider;
    [SerializeField] GameObject alarm;

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
        target.Rotate(Vector3.forward * currentParamater);
    }

    protected override void UpdateUI()
    {
        float proportionalValue = (maxParamater - currentParamater) / (maxParamater - minParamater);
        if (slider) {
            slider.value = proportionalValue;

        }
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
