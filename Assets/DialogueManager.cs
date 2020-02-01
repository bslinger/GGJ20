using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using System;

[Serializable]
public struct NarrativeEvent
{
    public float triggerPercentage;
    //public string eventToFire;
    public UnityEvent eventToFire;
    public bool waitForDialogue;
    public bool triggered;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    public NarrativeEvent[] narrativeEvents;

    public Comms comms;
    public DialogueRunner dialogueRunner;

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
               // triggered.Add(ev);
                ev.eventToFire.Invoke();
                ev.triggered = true;
                narrativeEvents[i] = ev;
            }
        }
        
    }

    public void FirstCoreBreak()
    {
        PowerCore brokenCore = null;
        PowerCore[]cores = FindObjectsOfType<PowerCore>();
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
        dialogueRunner.StartDialogue("FirstCoreBreak");
        
    }
}
