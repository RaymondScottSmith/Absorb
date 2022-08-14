using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    private const int leaderboardID = 5471;

    public bool alive = true;

    public float time = 0;
    
    [SerializeField]
    private TMP_Text timeLabel;
    
    
    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        time = 0;
    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            time += Time.deltaTime;
            timeLabel.SetText("Time: " + Mathf.Round(time).ToString());
        }
    }
}
