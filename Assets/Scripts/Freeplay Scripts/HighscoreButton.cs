using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreButton : MonoBehaviour
{
    [SerializeField] private GameObject highscorePanel;

    private Leaderboard leaderboard;

    void Start()
    {
        leaderboard = FindObjectOfType<Leaderboard>();
    }

    // Start is called before the first frame update

    public void SwitchVisible()
    {
        if (highscorePanel.activeSelf)
        {
            highscorePanel.SetActive(false);
        }
        else
        {
            highscorePanel.SetActive(true);
            StartCoroutine(leaderboard.FetchTopHighscoresRoutine());
        }
    }

    public void StartGame()
    {
        ScreenTransition.Instance.StartGame();
    }

    public void StartTutorial()
    {
        ScreenTransition.Instance.LoadTutorial();
    }
}
