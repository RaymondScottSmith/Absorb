using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class V2AsteroidSpawner : MonoBehaviour
{
    [Serializable]
    public class PosTimePair
    {
        public List<Vector2> positions;
        public int timer;
    }

    public List<PosTimePair> waves = new List<PosTimePair>();

    public List<PosTimePair> healthDrops = new List<PosTimePair>();
    
    
    [SerializeField] private int endingTime = 20;
    private Dictionary<int, List<Vector2>> waveDict = new Dictionary<int, List<Vector2>>();

    private Dictionary<int, List<Vector2>> pickupsDict = new Dictionary<int, List<Vector2>>();

    [SerializeField] private bool isRandomized;
    [SerializeField] private GameObject asteroidPrefab, indicatorPrefab, healthPrefab;
    
    
    [SerializeField]
    private Vector2 indicatorOffset;

    [SerializeField] private Vector2 minSpawnCoords, maxSpawnCoords, launchDirection;

    [SerializeField] private float asteroidSpeed, indicatorDelay;

    [SerializeField] private Slider journeySlider;



    private int currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        foreach (var kvp in healthDrops)
        {
            pickupsDict[kvp.timer] = kvp.positions;
            
        }
        journeySlider.maxValue = endingTime;
        journeySlider.minValue = 0;
        if (isRandomized)
            InvokeRepeating("LaunchAsteroid", 4, 5);
        else
        {
            //StartCoroutine(SpawnWaves(waves[0]));
            foreach (var kvp in waves) {
                waveDict[kvp.timer] = kvp.positions;
            }

            StartCoroutine(WaveAfterWave());
        }

        
    }

    private IEnumerator WaveAfterWave()
    {
        while (currentTime < endingTime)
        {
            if (waveDict.ContainsKey(currentTime))
            {
                List<Vector2> spawns = waveDict[currentTime];
                foreach (Vector2 pos in spawns)
                {
                    LaunchAsteroidScript(pos);
                }
            }

            if (pickupsDict.ContainsKey(currentTime))
            {
                List<Vector2> spawns = pickupsDict[currentTime];
                foreach (Vector2 pos in spawns)
                {
                    StartCoroutine(SpawnHealthPickup(pos));
                }
            }
            yield return new WaitForSeconds(1f);
            currentTime++;
            journeySlider.value = currentTime;
        }
    }

    private IEnumerator SpawnWaves(PosTimePair pair)
    {
        yield return new WaitForSeconds(pair.timer);
        foreach (var pos in pair.positions)
        {
            LaunchAsteroidScript(pos);
        }

        waves.RemoveAt(0);
        StartCoroutine(SpawnWaves(waves[0]));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchAsteroidScript(Vector2 spawnLocation)
    {
        StartCoroutine(SpawnAsteroid(spawnLocation));
    }

    private void LaunchAsteroid()
    {
        float startX = Random.Range(minSpawnCoords.x, maxSpawnCoords.x);
        float startY = Random.Range(minSpawnCoords.y, maxSpawnCoords.y);

        StartCoroutine(SpawnAsteroid(new Vector2(startX, startY)));
    }

    private IEnumerator SpawnAsteroid(Vector2 spawnLocation)
    {
        Vector2 indicatorPosition = spawnLocation + indicatorOffset;
        Instantiate(indicatorPrefab, indicatorPosition, indicatorPrefab.transform.rotation);

        yield return new WaitForSeconds(indicatorDelay);
        Instantiate(asteroidPrefab, spawnLocation, asteroidPrefab.transform.rotation)
            .GetComponent<Asteroid>().Activate(launchDirection, Vector3.one, 
                new Vector2(asteroidSpeed, asteroidSpeed), new Vector2(1, -1));
    }

    private IEnumerator SpawnHealthPickup(Vector2 spawnLocation)
    {
        yield return new WaitForSeconds(indicatorDelay);
        Instantiate(healthPrefab, spawnLocation, healthPrefab.transform.rotation).GetComponent<ShipHealthUp>().Activate(launchDirection,
            Vector3.one, new Vector2(asteroidSpeed, asteroidSpeed));
    }
}
