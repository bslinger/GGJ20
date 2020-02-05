using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Yarn.Unity;

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
    [SerializeField] GameObject playerTeleportParticlePrefab;

    public bool hasFailed = false;
    public bool hasWon = false;
    public bool allDead = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.AddCommandHandler("break", (string[] parameters) => {
            comms.AddLineBreak();
        }
        );
    }

    // Update is called once per frame
    void Update()
    {
       // List<NarrativeEvent> triggered = new List<NarrativeEvent>();
        for(int i = 0; i < narrativeEvents.Length; i++)
        {
            NarrativeEvent ev = narrativeEvents[i];
            if (!ev.triggered && ev.triggerPercentage < comms.PercentageOfJourney && !allDead)
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
        if (cryo.GetAliveCryoBeds().Count == 6)
        {
            dialogueRunner.StartDialogue("WinState-AllColonistsRemain");
        } else
        {
            dialogueRunner.StartDialogue("WinState-SomeColonistsRemain");
        }
        yield return new WaitWhile(() => {
            return dialogueRunner.isDialogueRunning;
        });
        foreach (GameObject cryo in cryo.GetAliveCryoBeds())
        {
            Instantiate(teleportParticlePrefab, cryo.transform.position, cryo.transform.rotation, cryo.transform);
            yield return new WaitForSeconds(1f);
        }

        Instantiate(playerTeleportParticlePrefab, playerTransform.position, playerTransform.rotation, playerTransform);

        LoadScene load = new LoadScene(0, 10, "Attract", Color.grey);
        StartCoroutine(load.LoadGame());
    }

    public void OnColonistDie()
    {
        if (cryo.GetAliveCryoBeds().Count == 0 && !hasFailed)
        {
            hasFailed = true;
            StartCoroutine(AllColonistsDeadRoutine());
        }
    }

    public IEnumerator AllColonistsDeadRoutine()
    {
        allDead = true;
        dialogueRunner.StartDialogue("FailState-AllColonistsDead");
        yield return new WaitWhile(() => {
            return dialogueRunner.isDialogueRunning;
            });

        Debug.Log("Fading to black");
        LoadScene load = new LoadScene(0, 4, "Attract", Color.black);
        StartCoroutine(load.LoadGame());
    }

    public IEnumerator OxygenOutFailStateRoutine()
    {
        LoadScene load = new LoadScene(0, 1, "Attract", Color.black);
        StartCoroutine(load.LoadGame());
        yield return null;
    }

    public IEnumerator CoreBreakRoutine(string node)
    {
        PowerCore brokenCore = null;
        PowerCore[] cores = FindObjectsOfType<PowerCore>();
        List<PowerCore> pcList = new List<PowerCore>();
        foreach (PowerCore core in cores)
        {
            if (core.powered)// && core.GetPoweredPoint() != "CommsConnectPoint")
            {
                pcList.Add(core);
            }
        }
        brokenCore = pcList[UnityEngine.Random.Range(0,pcList.Count-1)];
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
