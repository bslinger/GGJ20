using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    [SerializeField] GameObject alarm;
    AudioSource audioSource;

    // Start is called before the first frame update
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        while (true)
        {
            alarm.SetActive(true);
            if(audioSource) audioSource.Play();
            yield return new WaitForSeconds(onTime);
            alarm.SetActive(false);
            if (audioSource) audioSource.Stop();
            yield return new WaitForSeconds(offTime);
        }
    }

    // Update is called once per frame
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
