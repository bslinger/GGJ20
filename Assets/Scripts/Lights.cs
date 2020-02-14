using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : SystemBase
{
    [SerializeField] List<GameObject> lights;
    [SerializeField] GameObject lightsOnUI;
    [SerializeField] GameObject lightsOffUI;
    [SerializeField] float timeBetweenLightsMax = 5;
    [SerializeField] float timeBetweenLightsMin = 1;
    [SerializeField] float flickerTime = 0.1f;

    Coroutine powerUp;
    Coroutine powerDown;

    protected override void UpdateMe()
    {
        if (isPowered)
        {
            Increase();
            if (powerDown != null)
            {
                StopAllCoroutines();
                powerDown = null;
            }
            if (powerUp == null)
            {
                powerUp = StartCoroutine(PowerUp());
            }

        }
        else
        {
            Decrease(); // was Increase();
            if (powerUp != null)
            {
                StopAllCoroutines();
                powerUp = null;
            }
            if (powerDown == null)
            {
                powerDown = StartCoroutine(PowerDown());
            }
        }
    }

    IEnumerator PowerUp()
    {
        yield return null;
        foreach (GameObject light in lights)
        {
            StartCoroutine(TurnOn(light));
            yield return new WaitForSeconds(Random.Range(timeBetweenLightsMin, timeBetweenLightsMax));
        }
    }

    IEnumerator PowerDown()
    {
        yield return null;
        foreach (GameObject light in lights)
        {
            StartCoroutine(TurnOff(light));
            yield return new WaitForSeconds(Random.Range(timeBetweenLightsMin, timeBetweenLightsMax));
        }
    }

    IEnumerator TurnOn(GameObject light)
    {
        yield return null;
        if (light.activeSelf) yield break;
        int cycles = Random.Range(1, 10);
        for (int i = 0; i< cycles; i++)
        {
            light.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0, flickerTime));
            light.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0, flickerTime));
        }
    }

    IEnumerator TurnOff (GameObject light)
    {
        yield return null;
        if (!light.activeSelf) yield break;
        int cycles = Random.Range(1, 10);
        for (int i = 0; i < cycles; i++)
        {
            light.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0, flickerTime));
            light.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0, flickerTime));
        }
    }

    protected override void UpdateUI()
    {
        lightsOnUI.SetActive(isPowered);
        lightsOffUI.SetActive(!isPowered);
    }

}
