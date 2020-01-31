using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : SystemBase
{
    List<GameObject> lights;

    protected override void UpdateMe()
    {
        if (isPowered)
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(false);
            }
        }
    }

}
