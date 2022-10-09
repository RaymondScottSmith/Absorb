using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GB_Spawner : MonoBehaviour
{
    public GB_Stage stage;

    public float spawnDelay1;
    public float spawnDelay3rd;

    public int maxMines;

    private int currentMines;

    [SerializeField] private GameObject minePrefab;

    [SerializeField] private List<Transform> spawnPoints;

    private List<MineV2> mines = new List<MineV2>();
    // Start is called before the first frame update
    void Start()
    {
        UpdateStage(stage);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ClearMines()
    {
        CancelInvoke();
        for (int i = mines.Count(); i > 0; i--)
        {
            mines[i-1].ForceExplosion();
        }
        
    }

    public void UpdateStage(GB_Stage newStage)
    {
        //stage = GB_Stage.Stage1;
        switch (newStage)
        {
            case GB_Stage.Off:
                ClearMines();
                break;
            case GB_Stage.Stage1:
                Debug.Log("Starting stage 1 now");
                if (currentMines < maxMines)
                {
                    InvokeRepeating("SpawnMine", 0, spawnDelay1);
                }
                break;
            
            case GB_Stage.Stage3:
                InvokeRepeating("SpawnMine", 0, spawnDelay3rd);
                break;
            default:
                break;
        }
    }

    private void SpawnMine()
    {
        MineV2 newMine = Instantiate(minePrefab, RandomSpawnPoint().position, minePrefab.transform.rotation).GetComponent<MineV2>();
        newMine.AssignSpawner(this);
        mines.Add(newMine);
        
    }

    public void RemoveMine(MineV2 removed)
    {
        mines.Remove(removed);
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
