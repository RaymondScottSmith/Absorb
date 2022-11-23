using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class TimerRepeat : MonoBehaviour
{

    public UnityEvent OnTimerReachedEvent;

    public bool isActiveAtStart;

    public bool isRepeating;

    public float timerDuration;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (isActiveAtStart)
        {
            StartTimer(timerDuration, isRepeating, 0);
        }
    }

    public void StartTimer()
    {
        if (isRepeating)
        {
            InvokeRepeating("RepeatingTimer", timerDuration,timerDuration);
        }
        else
        {
            StartCoroutine(NonRepeatingTimer(timerDuration));
        }
    }
    

    private void StartTimer(float duration, bool repeating, float delay = 0f)
    {

        isRepeating = repeating;
        timerDuration = duration;

        if (isRepeating)
        {
            InvokeRepeating("RepeatingTimer", duration,duration);
        }
        else
        {
            StartCoroutine(NonRepeatingTimer(duration));
        }
    }

    private void RepeatingTimer()
    {
        OnTimerReachedEvent?.Invoke();
    }

    private IEnumerator NonRepeatingTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnTimerReachedEvent?.Invoke();
    }
}
