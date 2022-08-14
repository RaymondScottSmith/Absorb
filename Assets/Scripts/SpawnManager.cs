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

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] screams;
    
    // Start is called before the first frame update
    void Start()
    {
        ladderTops = GameObject.FindGameObjectsWithTag("LadderTop");
        ladderBottoms = GameObject.FindGameObjectsWithTag("LadderBottom");
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SpawnRunner());
        
    }

    private IEnumerator SpawnRunner()
    {
        for (int i = 0; i < 10; i++)
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
        
            yield return new WaitForSeconds(2);
            Runner newRunner = Instantiate(runnerPrefab, spawnPoints[startingRoom].position, runnerPrefab.transform.rotation)
                .GetComponent<Runner>();
            newRunner.StartJourney(spawnPoints[exitRoom].position, startingFloor, endingFloor, this);
        }
        
    }

    public void PlayScream()
    {
        audioSource.PlayOneShot(screams[Random.Range(0, screams.Length)]);
    }
}
