using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBase : MonoBehaviour
{
    [SerializeField] string paramaterName;
    [SerializeField] float startingParamater;
    [SerializeField] float maxParamater;
    [SerializeField] float minParamater;
    [SerializeField] float step;
    protected float currentParamater;
    protected bool isPowered;

    private void Start()
    {
        currentParamater = startingParamater;
        StartMe();
    }

    protected virtual void StartMe()
    {

    }

    public void Increase()
    {
        currentParamater += step * Time.deltaTime;
        currentParamater = Mathf.Clamp(currentParamater, minParamater, maxParamater);
    }

    public void Decrease ()
    {
        currentParamater -= step * Time.deltaTime;
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
        isPowered = false;
        // TODO: Check for power
    }

    protected virtual void UpdateMe()
    {

    }

    protected virtual void UpdateUI()
    {

    }
}
