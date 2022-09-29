using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchOnAndOff : MonoBehaviour
{

    [SerializeField]
    private bool isOff;
    
    public UnityEvent TurnOn;
    public UnityEvent TurnOff;
    // Start is called before the first frame update
    

    public void Switch()
    {
        if (isOff)
        {
            TurnOn?.Invoke();
        }
        else
        {
            TurnOff?.Invoke();
        }

        isOff = !isOff;
    }
}
