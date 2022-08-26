using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform startingPosition;

    [SerializeField] public GameObject player;

    public GameObject eatingPanel;

    public static LevelManager Instance;

    public bool startPlayerOnAwake = false;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerController>().readyToLaunch = true;
        TalkScript.Instance.QueueLine("Eat Him");
        
        player.transform.position = startingPosition.position;
        if (startPlayerOnAwake)
        {
            player.GetComponent<PlayerController>().readyToPlay = true;
        }
        BeginFall();
        
        
    }

    public void StartInstructions()
    {
        TalkScript.Instance.DisplayMessages();
    }

    //For when the level begins by falling from a ceiling
    void BeginFall()
    {
        //StartCoroutine(playerIntangible(2f));
        //StartCoroutine(player.GetComponent<PlayerController>().Fall(3));
        
    }

    private IEnumerator playerIntangible(float howLong)
    {
        //player.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(howLong);
        //player.GetComponent<Collider2D>().enabled = true;
        StartCoroutine(player.GetComponent<PlayerController>().Fall(1));
    }

    // Update is called once per frame
    void Update()
    {
        //if(firstTriggerZone.Tri)
    }
}
