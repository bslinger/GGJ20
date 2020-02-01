using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : SystemBase
{
    [SerializeField] List<GameObject> lights;
    [SerializeField] GameObject lightsOnUI;
    [SerializeField] GameObject lightsOffUI;

    protected override void UpdateMe()
    {
        if (isPowered)
        {
            Increase();
            foreach (GameObject light in lights)
            {
                light.SetActive(true);
            }
        }
        else
        {
            Decrease();
            foreach (GameObject light in lights)
            {
                
                light.SetActive(false);
            }
        }
    }

    protected override void UpdateUI()
    {
        lightsOnUI.SetActive(isPowered);
        lightsOffUI.SetActive(!isPowered);
    }

}
