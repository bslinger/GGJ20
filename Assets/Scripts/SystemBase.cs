﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SystemBase : MonoBehaviour
{
    [SerializeField] string paramaterName;
    [SerializeField] float startingParamater;
    [SerializeField] float maxParamater;
    [SerializeField] float minParamater;
    [SerializeField] float step;
    protected float currentParamater;
    protected bool isPowered;
    [SerializeField] GameObject powerSourceObject;
    PowerSource powerSource;

    private void Start()
    {
        powerSource = powerSourceObject.GetComponent<PowerSource>();
        currentParamater = startingParamater;
        StartMe();
    }

    protected virtual void StartMe()
    {

    }

    public void Increase(float stepProportion)
    {
        currentParamater += step * Time.deltaTime * stepProportion;
        currentParamater = Mathf.Clamp(currentParamater, minParamater, maxParamater);
    }

    public void Increase()
    {
        Increase(1f);
    }

    public void Decrease ()
    {
        Decrease(1f);
    }

    public void Decrease(float stepProportion)
    {
        currentParamater -= step * Time.deltaTime * stepProportion;
        currentParamater = Mathf.Clamp(currentParamater, minParamater, maxParamater);
    }

    [SerializeField] bool isDebug;

    public void OnGUI()
    {
        if (isDebug)
        {
            GUILayout.Label(paramaterName + " is " + currentParamater);
        }
    }

    private void Update()
    {
        if (isDebug)
        {

            if (Input.GetKeyUp(KeyCode.KeypadPlus))
            {
                Increase();
            }
            if (Input.GetKeyUp(KeyCode.KeypadMinus))
            {
                Decrease();
            }
        }

        CheckPower();
        UpdateMe();
        UpdateUI();
    }

    void CheckPower()
    {
        isPowered = powerSource.IsPowered();
    }

    protected virtual void UpdateMe()
    {

    }

    protected virtual void UpdateUI()
    {

    }
}
