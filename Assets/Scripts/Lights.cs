using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : SystemBase
{
    [SerializeField] List<GameObject> lights;
    [SerializeField] GameObject lightsOnUI;
    [SerializeField] GameObject lightsOffUI;

    Coroutine powerUp;
    Coroutine powerDown;

    protected override void UpdateMe()
    {
        if (isPowered)
        {
            Increase();
            if (powerUp == null)
            {
                powerUp = StartCoroutine(PowerUp());
            }
            if (powerDown != null)
            {
                StopCoroutine(powerDown);
                powerDown = null;
            }
        }
        else
        {
            Increase();
            if (powerDown == null)
            {
                powerDown = StartCoroutine(PowerDown());
            }
            if (powerUp != null)
            {
                StopCoroutine(powerUp);
                powerUp = null;
            }
        }
    }

    IEnumerator PowerUp()
    {
        yield return null;
        foreach (GameObject light in lights)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            light.SetActive(true);
        }
    }

    IEnumerator PowerDown()
    {
        yield return null;
        foreach (GameObject light in lights)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            light.SetActive(true);
        }
    }

    protected override void UpdateUI()
    {
        lightsOnUI.SetActive(isPowered);
        lightsOffUI.SetActive(!isPowered);
    }

}
