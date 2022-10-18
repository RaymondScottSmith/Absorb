using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
    

    [SerializeField]
    private GameObject losePanel;

    [SerializeField] private TMP_Text timeText;
    private CameraController cam;

    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        //cam.PlayOneShot(loseMusic);
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

    public void LoadCheckPoint()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetToTitle()
    {
        ScreenTransition.Instance.BackToMenu();
    }
}
