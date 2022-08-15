using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class UserManager : MonoBehaviour
{

    public TMP_InputField playerNameInputfield;

    //public static UserManager Instance;
    public Leaderboard leaderboard;

    /*
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
    */
    
    // Start is called before the first frame update
    void Start()
    {
        leaderboard = FindObjectOfType<Leaderboard>();
        //DontDestroyOnLoad(this);
        StartCoroutine(SetupRoutine());
    }

    private IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return leaderboard.FetchTopHighscoresRoutine();
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) =>
        {
            if (response.success)
            {
                //Debug.Log("Successfully set player name");
            }
            else
            {
                //Debug.Log("Could not set player name: " + response.Error);
            }
        });
    }
    
    private IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    // Update is called once per frame
    
}
