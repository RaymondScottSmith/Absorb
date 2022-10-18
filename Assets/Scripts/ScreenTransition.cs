using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public static ScreenTransition Instance;
    public Vector2 startingCheckpoint;
    [SerializeField]
    private Animator transitionAnimator;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
            return;
        }
        Destroy(gameObject);
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(1);
        StartCoroutine(LoadTransition(1));
    }

    public void LoadScene(int sceneNum)
    {
        //SceneManager.LoadScene(sceneNum);
        StartCoroutine(LoadTransition(sceneNum));
    }

    public void ReloadScene()
    {
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.lastCheckpointPos = startingCheckpoint;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(LoadTransition(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadDemoEnd()
    {
        //SceneManager.MoveGameObjectToScene(CheckpointManager.Instance.gameObject, SceneManager.GetActiveScene());
        CheckpointManager.Instance.lastCheckpointPos = startingCheckpoint;
        StartCoroutine(LoadTransition(4));
        //SceneManager.LoadScene(4);
    }

    public void BackToMenu()
    {
        CheckpointManager.Instance.lastCheckpointPos = startingCheckpoint;
        Time.timeScale = 1f;
        //SceneManager.LoadScene(0);
        StartCoroutine(LoadTransition(0));
    }

    public void LoadTutorial()
    {
        StartCoroutine(LoadTransition(2));
        //SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadTransition(int sceneNumber)
    {
        transitionAnimator.SetTrigger("Fadeout");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneNumber);
    }
}
