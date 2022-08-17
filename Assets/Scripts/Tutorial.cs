using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutBoxes;

    [SerializeField] private GameObject crewMember;

    [SerializeField] private Vector3 cameraPos2;

    [SerializeField] private Vector3 cameraPos3;

    [SerializeField] private Vector3 cameraPos4;

    [SerializeField] private GameObject dangers;

    [SerializeField] private Animator bluePanelAnimator;

    private float zoomOut = 28f;

    private bool isReadyToZoom;

    private static readonly int IsShowing = Animator.StringToHash("IsShowing");

    // Start is called before the first frame update
    void Start()
    {
        ResetBoxes();
        tutBoxes[0].SetActive(true);
    }

    private void ResetBoxes()
    {
        foreach (GameObject box in tutBoxes)
        {
            box.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && tutBoxes[0].activeSelf)
        {
            ResetBoxes();
            tutBoxes[1].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && tutBoxes[1].activeSelf)
        {
            ResetBoxes();
            tutBoxes[2].SetActive(true);
            crewMember.SetActive(true);
        }

        if (isReadyToZoom && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            FindObjectOfType<Camera>().transform.position = cameraPos4;
            FindObjectOfType<Camera>().orthographicSize = 28;
        }
        else if (isReadyToZoom && (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)))
        {
            FindObjectOfType<Camera>().transform.position = cameraPos3;
            FindObjectOfType<Camera>().orthographicSize = 5.8f;
        }
        
    }

    public void CrewWasEaten()
    {
        ResetBoxes();
        tutBoxes[3].SetActive(true);
    }

    public void ProceedToRoom2()
    {
        ResetBoxes();
        FindObjectOfType<Camera>().transform.position = cameraPos2;
        tutBoxes[4].SetActive(true);
    }

    public void ShowDangers()
    {
        ResetBoxes();
        tutBoxes[5].SetActive(true);
        dangers.SetActive(true);
        
    }

    public void ProceedToRoom3()
    {
        ResetBoxes();
        tutBoxes[6].SetActive(true);
        FindObjectOfType<Camera>().transform.position = cameraPos3;
        bluePanelAnimator.SetBool(IsShowing, true);
    }
    
    public void TalkAboutShift()
    {
        bluePanelAnimator.SetBool(IsShowing, false);
        ResetBoxes();
        tutBoxes[7].SetActive(true);
        isReadyToZoom = true;
    }

    public void TellOver()
    {
        ResetBoxes();
        tutBoxes[8].SetActive(true);
    }

    public void DoneWithShift()
    {
        ResetBoxes();
        SceneManager.LoadScene(0);
    }

    public void LeaveTutorial()
    {
        SceneManager.LoadScene(0);
    }

}
