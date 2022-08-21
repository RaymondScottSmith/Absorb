using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform startingPosition;

    [SerializeField] public GameObject player;

    public GameObject eatingPanel;

    public static LevelManager Instance;

    [SerializeField] private Collider2D firstTriggerZone;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerController>().readyToLaunch = false;
        TalkScript.Instance.QueueLine("Eat Him");
        
        player.transform.position = startingPosition.position;
        BeginFall();
        
        
    }

    public void StartInstructions()
    {
        TalkScript.Instance.DisplayMessages();
    }

    //For when the level begins by falling from a ceiling
    void BeginFall()
    {
        StartCoroutine(playerIntangible(1f));
        StartCoroutine(player.GetComponent<PlayerController>().Fall(1));
    }

    private IEnumerator playerIntangible(float howLong)
    {
        player.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(howLong);
        player.GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if(firstTriggerZone.Tri)
    }
}
