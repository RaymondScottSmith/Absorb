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
    
    private void Awake()
    {
        if (TalkScript.Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }
    
    public void QueueLine(string message)
    {
        queuedMessages.Add(message);
    }

    public void DisplayMessages()
    {
        if (queuedMessages.Any())
        {
            StartCoroutine(DisplayLine());
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
        
    }
    
    
     private IEnumerator DisplayLetters()
    {
        textBox.text = "";
        string message = queuedMessages[0];
        foreach (var t in message)
        {
            textBox.text += t;
            yield return new WaitForSeconds(delayBetweenLetter);
        }

        queuedMessages.RemoveAt(0);

    }
    
}
