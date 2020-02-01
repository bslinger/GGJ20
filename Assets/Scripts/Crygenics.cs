using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crygenics : SystemBase
{

    [SerializeField] float deathTemperature;
    [SerializeField] int startingHumans;
    [SerializeField] float timeBetweenDeaths;
    [SerializeField] Slider deadHumansSlider;
    [SerializeField] Slider aliveHumansSlider;
    [SerializeField] Slider temperatureSlider;
    [SerializeField] GameObject alarm;

    int aliveHumans;
    float deathTimer;

    override protected void StartMe()
    {
        aliveHumans = startingHumans;
    }


    // Update is called once per frame
    override protected void UpdateMe()
    {
        if (isPowered)
        {
            Increase();
        } else
        {
            Decrease();
        }

        if (currentParamater < deathTemperature)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer > timeBetweenDeaths)
            {
                deathTimer = 0;
                aliveHumans -= 1;
                Debug.Log("A hummie died");
            }
        }
    }

    protected override void UpdateUI()
    {
        float proportionalValue = (maxParamater - currentParamater) / (maxParamater - minParamater);
        if (temperatureSlider)
        {
            temperatureSlider.value = proportionalValue;

        }
        if (alarm)
        {
            if (proportionalValue >= 0.5f)
            {
                alarm.SetActive(true);
            }
            else
            {
                alarm.SetActive(false);
            }
        }
        if (deadHumansSlider)
        {
            deadHumansSlider.value = startingHumans - aliveHumans;
        }
        if (aliveHumansSlider)
        {
            aliveHumansSlider.value = aliveHumans;
        }
    }
}
