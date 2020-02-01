﻿using System.Collections;
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
    [SerializeField] Text temperatureText;
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

        if (currentParameter < deathTemperature)
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
        float proportionalValue = (maxParameter - currentParameter) / (maxParameter - minParameter);
        if (temperatureText)
        {
            temperatureText.text = (int)currentParameter + "C";
            if (proportionalValue < 0.5f)
            {
                temperatureText.color = Color.green;
            }
            else if (proportionalValue < 0.75f)
            {
                temperatureText.color = Color.yellow;
            }
            else
            {
                temperatureText.color = Color.red;
            }

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
