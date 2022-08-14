using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject runnerPrefab;

    [SerializeField] private Transform[] spawnPoints;
    
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(runnerPrefab)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
