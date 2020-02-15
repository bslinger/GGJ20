using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
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
    [SerializeField] public Slider slider;
    [SerializeField] public TextMeshProUGUI statusText;
    
    protected float currentParameter;
    protected bool isPowered;
    [SerializeField] GameObject powerSourceObject;
    PowerSource powerSource;
    AudioSource audioSource;
    protected DialogueManager dialogue;

    private CanvasGroup panelEnabledObjectCanvasGroup;
    private Image panelEnabledObjectImage;
    private Color panelEnabledObjectImageVisibleColor;
    private Color panelEnabledObjectImageInvisibleColor;
    
    private CanvasGroup panelDisabledObjectCanvasGroup;
    private Image panelDisabledObjectImage;
    private Color panelDisabledObjectImageVisibleColor;
    private Color panelDisabledObjectImageInvisibleColor;
    
    private bool animatingPanelColor = false;
    private float panelTransitionPercent = 1;

    protected float proportionalValue = 1f;

    private void Start()
    {
        if (panelEnabledObject)
        {
            panelEnabledObjectCanvasGroup = panelEnabledObject.GetComponent<CanvasGroup>();
            panelEnabledObjectImage = panelEnabledObject.GetComponent<Image>();
            if (panelEnabledObjectImage)
            {
                panelEnabledObjectImageVisibleColor =
                    panelEnabledObjectImageInvisibleColor = panelEnabledObjectImage.color;
                panelEnabledObjectImageInvisibleColor.a = 0;
            } 
        }
        if (panelDisabledObject)
        {
            panelDisabledObjectCanvasGroup = panelDisabledObject.GetComponent<CanvasGroup>();
            panelDisabledObjectImage = panelDisabledObject.GetComponent<Image>();
            if (panelDisabledObjectImage)
            {
                panelDisabledObjectImageVisibleColor =
                    panelDisabledObjectImageInvisibleColor = panelDisabledObjectImage.color;
                panelDisabledObjectImageInvisibleColor.a = 0;
            }
        }

        if (powerSourceObject)
        {
            powerSource = powerSourceObject.GetComponent<PowerSource>();
            isPowered = powerSource.IsPowered();
            if (panelEnabledObjectCanvasGroup)
            {
                panelEnabledObjectCanvasGroup.alpha = powerSource.IsPowered() ? 1 : 0;
                Debug.Log(this.GetType()+" is "+(powerSource.IsPowered() ? "powered" : "not powered"));
                Debug.Log("panelEnabledObjectCanvasGroup.alpha = "+(panelEnabledObjectCanvasGroup.alpha));
            }
            if (panelDisabledObjectCanvasGroup)
            {
                panelDisabledObjectCanvasGroup.alpha = powerSource.IsPowered() ? 0 : 1;
                Debug.Log("panelDisabledObjectCanvasGroup.alpha = "+(panelDisabledObjectCanvasGroup.alpha));
            }
            
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
        proportionalValue = (currentParameter - minParameter) / (maxParameter - minParameter);
        if (slider)
        {
            slider.value = proportionalValue;
        }
        if (statusText != null)
        {
            //Debug.Log(currentParameter + " " + minParameter + " " + maxParameter + " " + proportionalValue.ToString());
            statusText.text = Math.Round(proportionalValue * 100) + "%";
        }

        CheckPower();
        UpdateMe();
        UpdateUI();
    }

    void CheckPower()
    {
        if (powerSource == null)
        {
            return;
        }
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
            if (panelEnabledObjectCanvasGroup)
            {
                panelEnabledObjectCanvasGroup.alpha = Mathf.Lerp(
                    isPowered ? 0 : 1,
                    isPowered ? 1 : 0,
                    panelTransitionPercent
                );
            }
            if (panelDisabledObjectCanvasGroup)
            {
                panelDisabledObjectCanvasGroup.alpha = Mathf.Lerp(
                    isPowered ? 1 : 0,
                    isPowered ? 0 : 1,
                    panelTransitionPercent
                );
            }
            if (panelEnabledObjectImage)
            {
                panelEnabledObjectImage.color = Color.Lerp(
                    isPowered ? panelEnabledObjectImageInvisibleColor : panelEnabledObjectImageVisibleColor,
                    isPowered ? panelEnabledObjectImageVisibleColor : panelEnabledObjectImageInvisibleColor,
                    panelTransitionPercent
                );
            }
            if (panelDisabledObjectImage)
            {
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
