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

    public GameObject commsUI;
    public int maxLines = 10;
    public float lineDelay = .1f;

    public string startNode = "Start";

    private DialogueRunner dialogueRunner;
    private GameObject dialogueCanvas;
    private TextMeshProUGUI textMesh;

    public float initialDistance = 1000f;
    private float distanceLeft;
    public float fullyPoweredStepPerSecond = 1f;

    public AudioClip messageStartAudio;
    public AudioClip messageEndAudio;
    public AudioClip characterAudio;

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
    }

    protected override void UpdateMe()
    {
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
        // change distance based on current signal strength
        distanceLeft -= fullyPoweredStepPerSecond * (currentParameter / maxParameter);
        _percentageOfJourney = 1 - (distanceLeft / initialDistance);
    }

    void ChangeToPowered()
    {
        dialogueCanvas.SetActive(true);
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
        lastNode = dialogueRunner.dialogue.currentNode;
        textMesh.text = "";
        dialogueRunner.Stop();
        dialogueCanvas.SetActive(false);
        commsUI.gameObject.GetComponent<AudioSource>().mute = true;
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
        commsUI.gameObject.GetComponent<AudioSource>().PlayOneShot(characterAudio);

    }

    private void SwitchedToNode(string lastNode)
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
        Debug.Log("Dialogue Started");
        textMesh.gameObject.SetActive(true);
        commsUI.gameObject.GetComponent<AudioSource>().PlayOneShot(messageStartAudio);
    }

    public void OnDialogueEnd()
    {
        StartCoroutine(this.TurnScreenOffAfterDelay());
    }

    private IEnumerator TurnScreenOffAfterDelay(float delay = 3f)
    {
        // show transmission end screen, play sound
        yield return new WaitForSeconds(delay);
        textMesh.gameObject.SetActive(false);
        commsUI.gameObject.GetComponent<AudioSource>().PlayOneShot(messageEndAudio);
        Transform transform1 = dialogueCanvas.transform.Find("TransmissionEndPanel");
        transform1.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        transform1.gameObject.SetActive(false);
    }

}
