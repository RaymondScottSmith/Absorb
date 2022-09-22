using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayerCollide : MonoBehaviour
{
    
    public string targetTag = "Player";

    [SerializeField]
    private bool triggerOnce;

    private bool alreadyTriggered;

    public UnityEvent OnCollisionEvent;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(targetTag))
        {
            if (triggerOnce && alreadyTriggered)
                return;
            OnCollisionEvent?.Invoke();
            alreadyTriggered = true;
        }
    }
}
