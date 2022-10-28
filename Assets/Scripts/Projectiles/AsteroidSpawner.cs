using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;

    [SerializeField] private Vector2 minSpawnCoords, maxSpawnCoords, launchDirection, maxMinSize, maxMinSpeed;

    public float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(launchAsteroid());
    }

    private IEnumerator launchAsteroid()
    {
        float startX = Random.Range(minSpawnCoords.x, maxSpawnCoords.x);
        float startY = Random.Range(minSpawnCoords.y, maxSpawnCoords.y);
        Instantiate(asteroidPrefab, new Vector3(startX, startY, 0), asteroidPrefab.transform.rotation)
            .GetComponent<Asteroid>().Activate(Vector3.left, new Vector3(1f, 0.5f, 0f), 
            new Vector2(10f, 5f), new Vector2(1, -1));
        
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(launchAsteroid());
    }
}
