using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject runnerPrefab;

    [SerializeField] private Transform[] spawnPoints;

    public GameObject[] ladderTops;
    public GameObject[] ladderBottoms;

    public Door[] doors;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] screams;

    private PlayerShrink player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerShrink>();
        player.GetComponent<PlayerController>().readyToPlay = true;
        ladderTops = GameObject.FindGameObjectsWithTag("LadderTop");
        ladderBottoms = GameObject.FindGameObjectsWithTag("LadderBottom");
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartSpawning());
        
    }

    private IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnRunner());
        StartCoroutine(SpawnRunner());

        yield return new WaitForSeconds(10);
        while (player.alive)
        {
            StartCoroutine(SpawnRunner());
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator SpawnRunner()
    {
        int startingRoom = 0;
            int exitRoom = 0;
        
            //Make sure the exit room is different than the starting room
            while (exitRoom == startingRoom)
            {
                startingRoom = Random.Range(0, spawnPoints.Length);
                exitRoom = Random.Range(0, spawnPoints.Length);
            }

            int startingFloor = (int)Mathf.Ceil((float)(startingRoom+1) / 2);
            int endingFloor = (int)Mathf.Ceil((float)(exitRoom+1) / 2);
            
            doors[startingRoom].OpenDoor();
            yield return new WaitForSeconds(1.5f);
            Runner newRunner = Instantiate(runnerPrefab, spawnPoints[startingRoom].position, runnerPrefab.transform.rotation)
                .GetComponent<Runner>();
            newRunner.StartJourney(spawnPoints[exitRoom].position, startingFloor, endingFloor, this);

    }

    public void PlayScream()
    {
        audioSource.PlayOneShot(screams[Random.Range(0, screams.Length)]);
    }
}
