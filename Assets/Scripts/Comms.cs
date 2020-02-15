using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[Serializable] public class StringEvent : UnityEvent<string> { }

public class Comms : SystemBase
{

    [Header("Text Display Settings")]
    public int maxLines = 10;
    public float lineDelay = .1f;
    public string startNode = "Start";

    [Header("References")]
    public GameObject commsUI;
    public Slider signalStrengthSlider;
    public TextMeshProUGUI distanceText;
    public DialogueManager dialogueManager;
    public GameObject shipIcon;
    public GameObject shipIconStartReference;
    public GameObject shipIconEndReference;
    public GameObject shipIconPowered;
    public GameObject shipIconUnpowered;

    [Header("Distance Settings")]
    public float initialDistance = 1000f;
    public float fullyPoweredStepPerSecond = 1f;
    public float signalThreshold = .2f;
    
    private DialogueRunner dialogueRunner;
   
    private GameObject dialogueCanvas;
    private TextMeshProUGUI textMesh;
    private float distanceLeft;
    private Vector3 shipIconStartingPosition;
    private Vector3 shipIconEndingPosition;
    private CanvasGroup shipIconPoweredCanvasGroup;
    private CanvasGroup shipIconUnpoweredCanvasGroup;

    [Header("Audio Clips")]
    public AudioClip messageStartAudio;
    public AudioClip messageEndAudio;
    public AudioClip characterAudio;

    [Header("Console UI")]
    [SerializeField] GameObject onText;
    [SerializeField] GameObject offText;

    private float _percentageOfJourney;

    public float PercentageOfJourney { get { return _percentageOfJourney; } }


    [SerializeField] StringEvent stringEvent = null;

    List<string> buffer;
    string currentLine = "";

    private string lastNode;

    private bool powered;
    private bool hasStarted = false;

    private void Awake()
    {
        buffer = new List<string>();
    }

    protected override void StartMe()
    {
        dialogueRunner = commsUI.GetComponentInChildren<DialogueRunner>();
        dialogueCanvas = commsUI.transform.Find("Dialogue Canvas").gameObject;
        textMesh = dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
        distanceLeft = initialDistance;
        shipIconPoweredCanvasGroup = shipIconPowered.GetComponent<CanvasGroup>();
        shipIconUnpoweredCanvasGroup = shipIconUnpowered.GetComponent<CanvasGroup>();
        if (shipIconStartReference != null && shipIconEndReference != null)
        {
            shipIconStartingPosition = shipIconStartReference.transform.localPosition;
            shipIconEndingPosition = shipIconEndReference.transform.localPosition;

            shipIcon.transform.position = shipIconStartingPosition;
        }
    }

    protected override void UpdateMe()
    {

        if(PercentageOfJourney >= 1 && !dialogueManager.hasWon && !dialogueManager.hasFailed)
        {
            StartCoroutine(dialogueManager.TriggerWinState());
        }

        if (isPowered)
        {
          if (!powered)
            {
                powered = true;
                ChangeToPowered();
            }
            Increase();
        }
        else
        {
            if (powered)
            {
                powered = false;
                ChangeToUnpowered();
            }
            Decrease();
        }

        signalStrengthSlider.value = currentParameter;

        // change distance based on current signal strength
        if (currentParameter > signalThreshold)
        {
            distanceLeft -= fullyPoweredStepPerSecond * (currentParameter / maxParameter);
            distanceLeft = Mathf.Clamp(distanceLeft, 0, initialDistance);
            _percentageOfJourney = 1 - (distanceLeft / initialDistance);
            shipIconPoweredCanvasGroup.alpha = 1;
            shipIconUnpoweredCanvasGroup.alpha = 0;
        } else {
            shipIconPoweredCanvasGroup.alpha = 0;
            shipIconUnpoweredCanvasGroup.alpha = 1;
        }
    }

    void ChangeToPowered()
    {
        if (!hasStarted)
        {
            dialogueRunner.StartDialogue(startNode);
            hasStarted = true;
        }
        commsUI.gameObject.GetComponent<AudioSource>().mute = false;
    }

    void ChangeToUnpowered()
    {
        // save the node we were on so we can start it back up next time
        commsUI.gameObject.GetComponent<AudioSource>().mute = true;
    }

    public void StartLine()
    {
        commsUI.gameObject.GetComponent<AudioSource>().PlayOneShot(characterAudio);
    }

    public void EndLine()
    {
        buffer.Add(currentLine);
        currentLine = "";
        if (buffer.Count > maxLines)
        {
            buffer.RemoveRange(0, buffer.Count - maxLines);
        }

        StartCoroutine(ContinueDialog());
    }

    public void UpdateLine(string line)
    {
        if (dialogueRunner.currentNodeName != lastNode)
        {
            lastNode = dialogueRunner.currentNodeName;
            SwitchedToNode(lastNode);
        }
        currentLine = line;
        textMesh.text = String.Join("\n", buffer.ToArray()) + "\n" + currentLine;

    }

    private void SwitchedToNode(string lastNode)
    {
        AddLineBreak();
    }

    public void AddLineBreak()
    {
        buffer.Add("");
    }

    public IEnumerator ContinueDialog()
    {
        yield return new WaitForSeconds(lineDelay);
        dialogueRunner.dialogue.Continue();
    }

    public void OnDialogueStart()
    {
        textMesh.text = "";
        buffer.Clear();
        textMesh.gameObject.SetActive(true);
        StartCoroutine(CommsSounds());
    }

    public void OnDialogueEnd()
    {
        StartCoroutine(this.TurnScreenOffAfterDelay());
    }

    private IEnumerator CommsSounds()
    {
        AudioSource commsAudioSource = commsUI.gameObject.GetComponent<AudioSource>();
        commsAudioSource.PlayOneShot(messageStartAudio);
        yield return new WaitUntil(() => !commsAudioSource.isPlaying);
        while (dialogueRunner.isDialogueRunning)
        {
            commsAudioSource.PlayOneShot(characterAudio);
            yield return new WaitUntil(() => !commsAudioSource.isPlaying);
        }
    }

    private IEnumerator TurnScreenOffAfterDelay(float delay = 5f)
    {
        // show transmission end screen, play sound
        yield return new WaitForSeconds(delay);
        if (dialogueRunner.isDialogueRunning)
        {
            yield break;
        }
        textMesh.gameObject.SetActive(false);
        textMesh.text = "";
        buffer.Clear();
        currentLine = "";
        commsUI.gameObject.GetComponent<AudioSource>().PlayOneShot(messageEndAudio);
        Transform transform1 = dialogueCanvas.transform.Find("TransmissionEndPanel");
        transform1.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        transform1.gameObject.SetActive(false);
    }

    protected override void UpdateUI()
    {
        if (onText)
        {
            onText.SetActive(isPowered);
        }

        if (offText)
        {
            offText.SetActive(!isPowered);
        }

        if (statusText != null)
        {
            if (isPowered)
            {
                if (currentParameter == 1)
                {
                    statusText.text = "Full Thrust";
                }
                else if (currentParameter > signalThreshold)
                {
                    float thrustValue = (currentParameter - signalThreshold) / (maxParameter - signalThreshold);
                    statusText.text = "Thrust: " + Math.Round(thrustValue * 100) + "%";
                }
                else
                {
                    statusText.text = "Powering Up";
                }
            } else {
                if (currentParameter > signalThreshold)
                {
                    statusText.text = "Slowing";
                }
                else
                {
                    statusText.text = "No Thrust";
                }
            }
        }
        if (distanceText != null)
        {
            distanceText.text = "Remaining: " + ((int)distanceLeft).ToString() + "km";
        }

        if (shipIcon != null && shipIconStartingPosition != null && shipIconEndingPosition != null)
        {
            shipIcon.transform.localPosition =
                Vector3.Lerp(shipIconStartingPosition, shipIconEndingPosition, _percentageOfJourney);
        }
    }
}
