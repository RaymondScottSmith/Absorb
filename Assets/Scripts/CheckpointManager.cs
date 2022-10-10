using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    public Vector2 lastCheckpointPos;

    public UnityEvent OnLoadCheckpoint;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCheckpoint(Vector2 checkpoint)
    {
        lastCheckpointPos = checkpoint;
    }

    public Vector2 LoadCheckpoint()
    {
        OnLoadCheckpoint?.Invoke();
        return lastCheckpointPos;
    }
}
