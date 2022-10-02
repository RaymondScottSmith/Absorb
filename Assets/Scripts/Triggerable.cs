using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Triggerable : MonoBehaviour
{
    [SerializeField]
    protected GameObject relevantGameObject;

    protected bool activated = true;

    [SerializeField] private float delay = 0f;
    
    
    [SerializeField]
    private bool isOff;
    
    public UnityEvent TurnOn;
    public UnityEvent TurnOff;
    public virtual void Activate()
    {
        if (relevantGameObject != null)
            relevantGameObject.SetActive(!activated);
        activated = !activated;
        Switch();
    }
    
    public void Switch()
    {
        if (isOff)
        {
            StartCoroutine(TurnItOn());
            
        }
        else
        {
            StartCoroutine(TurnItOff());
        }

        isOff = !isOff;
    }

    private IEnumerator TurnItOn()
    {
        yield return new WaitForSeconds(delay);
        TurnOn?.Invoke();
    }
    
    private IEnumerator TurnItOff()
    {
        yield return new WaitForSeconds(delay);
        TurnOff?.Invoke();
    }
}
