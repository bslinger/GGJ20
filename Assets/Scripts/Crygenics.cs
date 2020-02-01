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
    [SerializeField] Text temperatureText;
    [SerializeField] GameObject alarm;
    
    [SerializeField] List<GameObject> cryoBeds;
    
    private int nextDead = 0;
    private StatusLights nextDeadStatusLights;

    int aliveHumans;
    float deathTimer;

    override protected void StartMe() {
        
        // Shuffle order of beds
        for (int i = 0; i < cryoBeds.Count; i++) {
            GameObject temp = cryoBeds[i];
            int randomIndex = Random.Range(i, cryoBeds.Count);
            cryoBeds[i] = cryoBeds[randomIndex];
            cryoBeds[randomIndex] = temp;
        }

        startingHumans = cryoBeds.Count;
        
        aliveHumans = startingHumans;
        
        nextDeadStatusLights = cryoBeds[nextDead].GetComponent<StatusLights>();
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
            float deathPercentage = deathTimer / timeBetweenDeaths;
            if ( nextDeadStatusLights ) {
                if (deathPercentage > 0.5f)
                {
                    nextDeadStatusLights.SetStatus(2);
                } else if (deathPercentage > 0f)
                {
                    nextDeadStatusLights.SetStatus(1);
                }
            }
            if (deathTimer > timeBetweenDeaths)
            {
                nextDeadStatusLights.SetStatus(3);
                deathTimer = 0;
                aliveHumans -= 1;
                nextDead++;
                if (nextDead < cryoBeds.Count)
                {
                    nextDeadStatusLights = cryoBeds[nextDead].GetComponent<StatusLights>();
                }
                else
                {
                    nextDeadStatusLights = null;
                }

                Debug.Log("A hummie died");
            }
        }
    }

    protected override void UpdateUI()
    {
        float proportionalValue = (maxParameter - currentParameter) / (maxParameter - minParameter);
    //    Debug.Log(proportionalValue);
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
        if (alarm != null)
        {
            if (proportionalValue >= 0.75f)
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
