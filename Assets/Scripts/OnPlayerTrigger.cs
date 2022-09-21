using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayerTrigger : MonoBehaviour
{

    public string targetTag = "Player";

    [SerializeField]
    private bool triggerOnce;

    private bool alreadyTriggered;

    public UnityEvent OnTriggerEnterEvent, OnTriggerExitEvent;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            if (triggerOnce && alreadyTriggered)
                return;
            OnTriggerEnterEvent?.Invoke();
            alreadyTriggered = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            OnTriggerExitEvent?.Invoke();
        }
    }
}
