using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTrigger : MonoBehaviour
{
    [SerializeField] private string[] messagesToDisplay;

    public bool triggered = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            TalkScript.Instance.ClearQueue();
            triggered = true;
            foreach (var message in messagesToDisplay)
            {
                TalkScript.Instance.QueueLine(message);
            }
            TalkScript.Instance.DisplayMessages();

        }
    }
}
