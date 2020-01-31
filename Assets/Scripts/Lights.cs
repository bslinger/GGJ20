using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : SystemBase
{
    [SerializeField] List<GameObject> lights;

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

}
