﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIntroMusic : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip gameMusic;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.loop = false;
        audioSource.Play();
        StartCoroutine(WaitForSongToEnd());
    }

    IEnumerator WaitForSongToEnd()
    {
        while (audioSource.isPlaying)
        {

            yield return new WaitForSeconds(0.1f);

        }
        if (gameMusic == null) 
        {
            Debug.Log("Audio Clip not found");
            Destroy(this); 
        }

        audioSource.clip = gameMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

}
