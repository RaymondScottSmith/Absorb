using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GB_Spawner : MonoBehaviour
{
    public GB_Stage stage;

    public float spawnDelay;

    public int maxMines;

    private int currentMines;

    [SerializeField] private GameObject minePrefab;

    [SerializeField] private List<Transform> spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        UpdateStage();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void UpdateStage()
    {
        stage = GB_Stage.Stage1;
        switch (stage)
        {
            case GB_Stage.Off:
                break;
            case GB_Stage.Stage1:
                Debug.Log("Starting stage 1 now");
                if (currentMines < maxMines)
                {
                    InvokeRepeating("SpawnMine", 0, spawnDelay);
                }
                break;
            default:
                break;
        }
    }

    private void SpawnMine()
    {
        Instantiate(minePrefab, RandomSpawnPoint().position, minePrefab.transform.rotation);
    }

    private Transform RandomSpawnPoint()
    {
        int num = Random.Range(0, spawnPoints.Count);
        return spawnPoints[num];
    }
    
    public enum GB_Stage
    {
       Off,
       Stage1,
       Stage2,
       Stage3
    }
}
