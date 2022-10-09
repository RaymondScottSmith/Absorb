using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MultiLock : MonoBehaviour
{

    [SerializeField] private bool checkOnUpdate = false;
    public List<bool> locks;

    public UnityEvent OnAllLocksOpen;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < locks.Count(); i++)
        {
            locks[i] = false;
        }
    }

    public void Unlock(int num)
    {
        if (num < locks.Count)
        {
            locks[num] = true;
        }

        foreach (bool test in locks)
        {
            if (!test)
                return;
        }
        OnAllLocksOpen?.Invoke();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!checkOnUpdate)
            return;
    }
}
