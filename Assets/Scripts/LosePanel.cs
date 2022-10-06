using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    

    [SerializeField]
    private GameObject losePanel;

    [SerializeField] private TMP_Text timeText;
    

    void Start()
    {
        losePanel.SetActive(false);
    }

    public void GameOver(int time)
    {
        StartCoroutine(ShowPanel(time));
    }

    private IEnumerator ShowPanel(int time)
    {
        yield return new WaitForSeconds(1f);
        losePanel.SetActive(true);
        timeText.text = "Your Time: " + time;
        Time.timeScale = 0f;
    }

    public void StartNewGame()
    {
        ScreenTransition.Instance.StartGame();
    }

    public void ResetToTitle()
    {
        ScreenTransition.Instance.BackToMenu();
    }
}
