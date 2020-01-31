using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crygenics : SystemBase
{

    [SerializeField] float deathTemperature;
    [SerializeField] int startingHumans;
    [SerializeField] float timeBetweenDeaths;

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
}
