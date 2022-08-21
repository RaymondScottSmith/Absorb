using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private bool isFinished = false;

    private PlayerController player;
    [SerializeField] private PlayableDirector director;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished && director.state != PlayState.Playing)
        {
            isFinished = true;
            player.readyToPlay = true;
            LevelManager.Instance.StartInstructions();
        }
    }
}
