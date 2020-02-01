using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SystemBase : MonoBehaviour
{
    [SerializeField] string parameterName;
    [SerializeField] protected float startingParameter;
    [SerializeField] protected float maxParameter;
    [SerializeField] protected float minParameter;
    [SerializeField] float step;
    protected float currentParameter;
    protected bool isPowered;
    [SerializeField] GameObject powerSourceObject;
    PowerSource powerSource;
    AudioSource audioSource;

    private void Start()
    {
        if (powerSourceObject)
        {
            powerSource = powerSourceObject.GetComponent<PowerSource>();
        }
        if (!powerSourceObject || powerSource == null)
        {
            Debug.LogError("Power source has not been assigned for this system. System " + name + " will be disabled.", this);
            gameObject.SetActive(false);
        }
        audioSource = GetComponent<AudioSource>();
        currentParameter = startingParameter;
        StartMe();
    }

    protected virtual void StartMe()
    {

    }

    public void Increase(float stepProportion)
    {
        currentParameter += step * Time.deltaTime * stepProportion;
        currentParameter = Mathf.Clamp(currentParameter, minParameter, maxParameter);
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
        currentParameter -= step * Time.deltaTime * stepProportion;
        currentParameter = Mathf.Clamp(currentParameter, minParameter, maxParameter);
    }

    [SerializeField] bool isDebug;

    public void OnGUI()
    {
        if (isDebug)
        {
            GUILayout.Label(parameterName + " is " + currentParameter);
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
        if (audioSource)
        {
            audioSource.enabled = isPowered;
        }
    }

    protected virtual void UpdateMe()
    {

    }

    protected virtual void UpdateUI()
    {

    }
}
