using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    [SerializeField] GameObject alarm;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        while (true)
        {
            alarm.SetActive(true);
            yield return new WaitForSeconds(onTime);
            alarm.SetActive(false);
            yield return new WaitForSeconds(offTime);
        }
    }

    // Update is called once per frame
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
