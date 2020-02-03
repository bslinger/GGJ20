using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartGame : SystemBase
{

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] AudioSource audio;
    bool sceneLoading = false;
    float vol;

    override protected void UpdateMe()
    {
        if (isPowered)
        {
            Decrease();
        }
        else
        {
            Increase();
        }

        if (text)
        {
            text.text = ((int)currentParameter).ToString();
        }

        if((int)currentParameter == 0 && !sceneLoading)
        {
            sceneLoading = true;
            LoadScene load = new LoadScene(1, 2, "Intro");
            StartCoroutine(load.LoadGame());
            vol = audio.volume;
        }

        if (sceneLoading)
        {
            float newVol = (vol * (audio.volume / vol)) * Time.deltaTime;
            Mathf.Clamp(newVol, 0, 1);
            audio.volume -= newVol;
            Debug.Log(newVol);
        }
    }
}
