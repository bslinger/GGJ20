using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class SystemBase : MonoBehaviour
{
    [SerializeField] string parameterName;
    [SerializeField] protected float startingParameter;
    [SerializeField] protected float maxParameter;
    [SerializeField] protected float minParameter;
    [SerializeField] float step;
    [FormerlySerializedAs("panelObject")] [SerializeField] GameObject panelEnabledObject;
    [FormerlySerializedAs("panelDisableddObject")] [SerializeField] GameObject panelDisabledObject;
    
    protected float currentParameter;
    protected bool isPowered;
    [SerializeField] GameObject powerSourceObject;
    PowerSource powerSource;
    AudioSource audioSource;
    protected DialogueManager dialogue;
    
    private Image panelEnabledObjectImage;
    private Color panelEnabledObjectImageVisibleColor;
    private Color panelEnabledObjectImageInvisibleColor;
    
    private Image panelDisabledObjectImage;
    private Color panelDisabledObjectImageVisibleColor;
    private Color panelDisabledObjectImageInvisibleColor;
    
    private bool animatingPanelColor = false;
    private float panelTransitionPercent = 1;

    private void Start()
    {
        if (panelEnabledObject)
        {
            panelEnabledObjectImage = panelEnabledObject.GetComponent<Image>();
            panelEnabledObjectImageVisibleColor = panelEnabledObjectImageInvisibleColor = panelEnabledObjectImage.color;
            panelEnabledObjectImageInvisibleColor.a = 0;
        }
        if (panelDisabledObject)
        {
            panelDisabledObjectImage = panelDisabledObject.GetComponent<Image>();
            panelDisabledObjectImageVisibleColor = panelDisabledObjectImageInvisibleColor = panelDisabledObjectImage.color;
            panelDisabledObjectImageInvisibleColor.a = 0;
        }

        if (powerSourceObject)
        {
            powerSource = powerSourceObject.GetComponent<PowerSource>();
            if (panelEnabledObjectImage)
            {
                panelEnabledObjectImage.color = powerSource.IsPowered()
                    ? panelEnabledObjectImageVisibleColor
                    : panelEnabledObjectImageInvisibleColor;
            }
            if (panelDisabledObjectImage)
            {
                panelDisabledObjectImage.color = powerSource.IsPowered()
                    ? panelDisabledObjectImageInvisibleColor
                    : panelDisabledObjectImageVisibleColor;
            }
        }
        if (!powerSourceObject || powerSource == null)
        {
            Debug.LogError("Power source has not been assigned for this system. System " + name + " will be disabled.", this);
            gameObject.SetActive(false);
        }
        audioSource = GetComponent<AudioSource>();
        currentParameter = startingParameter;
        dialogue = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
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
        if(dialogue == null || (!dialogue.hasWon && !dialogue.allDead)) Increase(1f);
    }

    public void Decrease ()
    {
        if (dialogue == null || (!dialogue.hasWon && !dialogue.allDead)) Decrease(1f);
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
        const int transitionDuration = 1;
        if (isPowered != powerSource.IsPowered()) // If there's been a change
        {
            isPowered = powerSource.IsPowered();
            animatingPanelColor = true;
            panelTransitionPercent = 1 - panelTransitionPercent;
        } // "#00DAFF" : "#ABABAB"

        if (animatingPanelColor)
        {
            panelTransitionPercent += Time.deltaTime / transitionDuration;
            if (panelTransitionPercent >= 1)
            {
                animatingPanelColor = false;
                panelTransitionPercent = 1;
            }
            if (panelEnabledObjectImage)
            {
                Debug.Log("Animating panelEnabledObjectImage.color");
                panelEnabledObjectImage.color = Color.Lerp(
                    isPowered ? panelEnabledObjectImageInvisibleColor : panelEnabledObjectImageVisibleColor,
                    isPowered ? panelEnabledObjectImageVisibleColor : panelEnabledObjectImageInvisibleColor,
                    panelTransitionPercent
                );
            }
            if (panelDisabledObjectImage)
            {
                Debug.Log("Animating panelDisabledObjectImage.color");
                panelDisabledObjectImage.color = Color.Lerp(
                    isPowered ? panelDisabledObjectImageVisibleColor : panelDisabledObjectImageInvisibleColor,
                    isPowered ? panelDisabledObjectImageInvisibleColor : panelDisabledObjectImageVisibleColor,
                    panelTransitionPercent
                );
            }
        }

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
