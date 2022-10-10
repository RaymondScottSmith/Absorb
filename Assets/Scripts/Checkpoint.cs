using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public UnityEvent OnLoadCheckpoint, OnTriggerCheckpoint;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerCheckpoint?.Invoke();
            CheckpointManager.Instance.UpdateCheckpoint(transform.position);
        }
    }

    public void LoadTheCheckpoint()
    {
        OnLoadCheckpoint?.Invoke();
    }
}
