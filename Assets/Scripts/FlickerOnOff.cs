using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlickerOnOff : MonoBehaviour
{
    [SerializeField] private GameObject flickeringObject;
    [SerializeField] private Collider2D flickerCollider;
    [SerializeField] private float timeOn;
    [SerializeField] private float timeOff;
    [SerializeField] private float startOffset;

    private void Start()
    {
        InvokeRepeating("StartFlicker",startOffset,timeOn+1.2f + timeOff);
    }

    private void StartFlicker()
    {
        flickerCollider.enabled = false;
        flickeringObject.SetActive(false);
        StartCoroutine(FlickerInAndOut());
    }
    private IEnumerator FlickerInAndOut()
    {
        yield return new WaitForSeconds(timeOff);
        for (int i = 0; i < 3; i++)
        {
            flickeringObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            flickeringObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        flickeringObject.SetActive(true);
        flickerCollider.enabled = true;

    }
}
