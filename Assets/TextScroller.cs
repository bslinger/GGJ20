using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Yarn.Unity;
using UnityEngine.UI;


[Serializable] public class StringEvent : UnityEvent<string> { }

public class TextScroller : MonoBehaviour
{
    public DialogueRunner runner;
    public int maxLines = 10;
    [SerializeField] StringEvent stringEvent = null;

    List<string> buffer;
    string currentLine = "";

    // Start is called before the first frame update
    void Start()
    {
        buffer = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        currentLine = line;
        GetComponent<TextMeshProUGUI>().text = String.Join("\n", buffer.ToArray()) + "\n" + currentLine;
    }

    public IEnumerator ContinueDialog()
    {
        yield return new WaitForSeconds(.5f);
        runner.dialogue.Continue();
    }
}
