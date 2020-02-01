using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;
using UnityEngine.UI;
using TMPro;

public class Comms : SystemBase
{

    public GameObject commsUI;
    public int maxLines = 10;
    public float lineDelay = .1f;

    private DialogueRunner dialogueRunner;
    private GameObject dialogueCanvas;
    private TextMeshProUGUI textMesh;

   
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
        }
        else
        {
            if (powered)
            {
                powered = false;
                ChangeToUnpowered();
            }
        }
    }

    void ChangeToPowered()
    {
        dialogueCanvas.SetActive(true);
        if (!hasStarted)
        {
            dialogueRunner.StartDialogue("Start");
            hasStarted = true;
        }
        else
        {
            dialogueRunner.StartDialogue("PowerReturn");
        }
       
    }

    void ChangeToUnpowered()
    {
        // save the node we were on so we can start it back up next time
        lastNode = dialogueRunner.dialogue.currentNode;
        textMesh.text = "";
        dialogueRunner.Stop();
        dialogueCanvas.SetActive(false);
        
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
        buffer.Add("");
    }

    public IEnumerator ContinueDialog()
    {
        yield return new WaitForSeconds(.5f);
        dialogueRunner.dialogue.Continue();
    }

}
