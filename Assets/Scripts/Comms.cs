using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Comms : SystemBase
{

    public DialogueRunner dialogueRunner;

    public string lastNode;

    private bool powered;
    private bool hasStarted = false;

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
        if (!hasStarted)
        {
            dialogueRunner.StartDialogue("Start");
        }
        else
        {
            dialogueRunner.StartDialogue("ReturnFromPowerOutage");
        }
       
    }

    void ChangeToUnpowered()
    {
        // save the node we were on so we can start it back up next time
        lastNode = dialogueRunner.dialogue.currentNode;
        dialogueRunner.Stop();
        dialogueRunner.Clear();
    }

}
