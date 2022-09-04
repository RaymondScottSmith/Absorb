using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private bool startIsFinished = false;
    private bool endIsFinished = false;

    private PlayerController player;
    [SerializeField] private PlayableDirector director;

    public static TimelineManager Instance;

    [SerializeField] private PlayableDirector endingCutscene;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startIsFinished && director.state != PlayState.Playing && player.readyToPlay == false)
        {
            startIsFinished = true;
            player.readyToPlay = true;
            LevelManager.Instance.StartInstructions();
        }
        else if (endIsFinished && endingCutscene.state != PlayState.Playing)
        {
            //Debug.Log("End Level Here");
            //Destroy(player.gameObject);
            Time.timeScale = 0f;
        }
    }

    public void FoundExit()
    {

        //Get rid of any remaining food sticking to player
        PlayerShrink playerShrink = player.GetComponent<PlayerShrink>();
        foreach (Shrink food in playerShrink.currentlyEating)
        {
            Destroy(food.gameObject);
        }
        
        //Start the ending cutscene
        endingCutscene.Play();
        endIsFinished = true;
    }
    
    
}
