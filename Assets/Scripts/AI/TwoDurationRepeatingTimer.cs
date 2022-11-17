using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwoDurationRepeatingTimer : MonoBehaviour
{

    public bool isOn = false;

    public bool startOn = false;

    public float initialDelay;
    
    public float durationOne;

    public float durationTwo;

    public UnityEvent OnTimerOneReached, OnTimerTwoReached;
    // Start is called before the first frame update
    void Start()
    {
        if (isOn)
        {
            StartCoroutine(StartTimers(initialDelay));
        }
    }

    public IEnumerator StartTimers(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(TwoDuration());
    }

    private IEnumerator TwoDuration()
    {
        if (isOn)
        {
            OnTimerOneReached?.Invoke();
            yield return new WaitForSeconds(durationOne);
            OnTimerTwoReached?.Invoke();
            yield return new WaitForSeconds(durationTwo);
            StartCoroutine(TwoDuration());
        }
    }
}
