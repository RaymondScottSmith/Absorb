using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundFlyby : MonoBehaviour
{

    [SerializeField] private List<GameObject> flybyPrefabs;

    [SerializeField] private float maxLeftSpawn = -100f;

    [SerializeField] private float maxRightSpawn = 100f;

    [SerializeField] private float maxUpSpawn = 30f;

    [SerializeField] private float maxDownSpawn = -15f;

    [SerializeField] private float minSpawnDelay = 10f;

    [SerializeField] private float maxSpawnDelay = 30f;

    [SerializeField] private float minSize = 0.5f;

    private float spawnDelay;
    // Start is called before the first frame update
    void Start()
    {
        spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        
        InvokeRepeating("SpawnFlyby", 2,spawnDelay);
    }

    void SpawnFlyby()
    {
        int leftRight = Random.Range(0, 2);
        float spawnY = Random.Range(maxDownSpawn, maxUpSpawn);
        float spawnSize = Random.Range(minSize, 1);

        int spawnNum = Random.Range(0, flybyPrefabs.Count());
        
        if (leftRight == 0)
        {
            Vector3 spawnPos = new Vector3(maxLeftSpawn, spawnY, 0);
            GameObject flyby = Instantiate(flybyPrefabs[spawnNum], spawnPos, flybyPrefabs[spawnNum].transform.rotation);
            flyby.GetComponent<MoveFlyby>().SetDespawn(maxRightSpawn);
            flyby.transform.localScale = Vector3.one * spawnSize;
        }
        else
        {
            Vector3 spawnPos = new Vector3(maxRightSpawn, spawnY, 0);
            GameObject flyby = Instantiate(flybyPrefabs[spawnNum], spawnPos, flybyPrefabs[spawnNum].transform.rotation);
            flyby.GetComponent<MoveFlyby>().SetDespawn(maxLeftSpawn);
            flyby.transform.localScale = Vector3.one * spawnSize;
        }
        spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
    }
}
