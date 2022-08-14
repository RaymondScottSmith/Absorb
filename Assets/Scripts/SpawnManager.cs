using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject runnerPrefab;

    [SerializeField] private Transform[] spawnPoints;

    public GameObject[] ladderTops;
    public GameObject[] ladderBottoms;
    
    // Start is called before the first frame update
    void Start()
    {
        ladderTops = GameObject.FindGameObjectsWithTag("LadderTop");
        ladderBottoms = GameObject.FindGameObjectsWithTag("LadderBottom");
        StartCoroutine(SpawnRunner());
    }

    private IEnumerator SpawnRunner()
    {
        yield return new WaitForSeconds(2);
        Runner newRunner = Instantiate(runnerPrefab, spawnPoints[6].position, runnerPrefab.transform.rotation)
            .GetComponent<Runner>();
        newRunner.StartJourney(spawnPoints[1].position, 4, 1, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
