using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TalkScript : MonoBehaviour
{
    public static TalkScript Instance;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private float delayBetweenLetter = 0.5f;
    [SerializeField] private float delayBetweenLines = 5f;
    private List<string> queuedMessages = new List<string>();

    private bool displaying = false;

    private bool stop;
    
    private void Awake()
    {
        if (TalkScript.Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
        stop = false;
    }
    
    public void QueueLine(string message)
    {
        queuedMessages.Add(message);
    }

    public void ClearQueue()
    {
        stop = true;
        queuedMessages.Clear();
        StopCoroutine(DisplayLine());
        StopCoroutine(DisplayLetters());
        //textBox.text = "";
    }

    public void ClearText()
    {
        stop = true;
        textBox.text = "";
        queuedMessages.Clear();
        StopCoroutine(DisplayLine());
        StopCoroutine(DisplayLetters());
        textBox.text = "";
    }

    public void DisplayMessages()
    {
        if (!displaying)
        {
            displaying = true;
            textBox.text = "";
            if (queuedMessages.Any())
            {
                StartCoroutine(DisplayLine());
            }
        }
        
    }

    private IEnumerator DisplayLine()
    {
        textBox.text = "";
        while (queuedMessages.Any())
        {
            yield return StartCoroutine(DisplayLetters());
            yield return new WaitForSeconds(delayBetweenLines);

        }

        displaying = false;

    }
    
    
     private IEnumerator DisplayLetters()
    {
        textBox.text = "";
        string message = queuedMessages[0];
        
        foreach (var t in message)
        {
            if (stop)
            {
                textBox.text = "";
                stop = false;
                yield break;
            }
            textBox.text += t;
            yield return new WaitForSeconds(delayBetweenLetter);
        }

        if (stop)
        {
            textBox.text = "";
            stop = false;
            yield break;
        }
        
        queuedMessages.RemoveAt(0);

    }
    
}
