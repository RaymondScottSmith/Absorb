using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour
{
    [SerializeField]
    protected GameObject relevantGameObject;

    protected bool activated = true;
    public virtual void Activate()
    {
        
        relevantGameObject.SetActive(!activated);
        activated = !activated;
    }
}
