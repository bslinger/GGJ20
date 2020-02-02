using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using System;

[Serializable]
public struct NarrativeEvent
{
    public UnityEvent eventToFire;
    public float triggerPercentage;   
    public string nodeToPlay;
    public bool waitForDialogue;
    public bool triggered;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    public NarrativeEvent[] narrativeEvents;

    [Header("References")]
    public Comms comms;
    public DialogueRunner dialogueRunner;
    public Crygenics cryo;
    public Transform playerTransform;

    [Header("Prefabs")]
    public GameObject teleportParticlePrefab;

    public bool hasFailed = false;
    public bool hasWon = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // List<NarrativeEvent> triggered = new List<NarrativeEvent>();
        for(int i = 0; i < narrativeEvents.Length; i++)
        {
            NarrativeEvent ev = narrativeEvents[i];
            if (!ev.triggered && ev.triggerPercentage < comms.PercentageOfJourney)
            {
                if (ev.waitForDialogue && dialogueRunner.isDialogueRunning)
                {
                    continue;
                }
                // remove from array and trigger
                if (ev.eventToFire != null)
                {
                    ev.eventToFire.Invoke();
                }
                if (ev.nodeToPlay != null && ev.nodeToPlay != "")
                {
                    dialogueRunner.StartDialogue(ev.nodeToPlay);
                }
                ev.triggered = true;
                narrativeEvents[i] = ev;
            }
        }
        
    }

    public void FirstCoreBreak()
    {
        StartCoroutine(CoreBreakRoutine("FirstCoreBreak"));
    }

    public void SecondCoreBreak()
    {
        StartCoroutine(CoreBreakRoutine("SecondCoreBreak"));
    }

    public void ThirdCoreBreak()
    {
        StartCoroutine(CoreBreakRoutine("ThirdCoreBreak"));
    }

    public void FourthCoreBreak()
    {
        StartCoroutine(CoreBreakRoutine("FourthCoreBreak"));
    }

    internal IEnumerator TriggerWinState()
    {
        hasWon = true;
        dialogueRunner.StartDialogue("WinState-ColonistsRemain");
        yield return new WaitForSeconds(3f);
        foreach (GameObject cryo in cryo.GetAliveCryoBeds())
        {
            Instantiate(teleportParticlePrefab, cryo.transform.position, cryo.transform.rotation, cryo.transform);
            yield return new WaitForSeconds(1f);
        }
        Instantiate(teleportParticlePrefab, playerTransform.position, Quaternion.identity, playerTransform);

    }

    public IEnumerator CoreBreakRoutine(string node)
    {
        PowerCore brokenCore = null;
        PowerCore[] cores = FindObjectsOfType<PowerCore>();
        foreach (PowerCore core in cores)
        {
            if (core.powered && core.GetPoweredPoint() != "CommsConnectPoint")
            {
                brokenCore = core;
                break;
            }
        }

        if (brokenCore != null)
        {
            brokenCore.BurnOut();
        }
        yield return new WaitForSeconds(3f);
        dialogueRunner.StartDialogue(node);
    }

    public void FailWithNode(string node)
    {
        hasFailed = true;
        dialogueRunner.StartDialogue(node);
    }

    public void StartFromNode(string node)
    {
        dialogueRunner.StartDialogue(node);
    }

}
