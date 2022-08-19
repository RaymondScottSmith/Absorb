using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DontLaunch : MonoBehaviour
{
    //This script exists so that when the mouse is over a UI object and clicks, the player won't launch

    private PlayerController playerController;
    private int UILayer;

    private bool isHoldingClick;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        UILayer = LayerMask.NameToLayer("UI");
    }

    private void Update()
    {
        //Remember to refactor later
        //This is hideous
        if (IsPointerOverUIElement())
        {
            playerController.isOverUI = true;
        }
        
        if (IsPointerOverUIElement() && Input.GetMouseButtonDown(0))
        {
            playerController.isOverUI = true;
        }
        else if (IsPointerOverUIElement() && Input.GetMouseButton(0))
        {
            playerController.isOverUI = true;
            isHoldingClick = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(UIReleaseMouse());
        }
        else if (!IsPointerOverUIElement() && !isHoldingClick)
        {
            playerController.isOverUI = false;
        }

    }
    
    private IEnumerator UIReleaseMouse()
    {
        yield return new WaitForFixedUpdate();
        isHoldingClick = false;
        playerController.isOverUI = false;
    }
 
 
    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
 
 
    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
 
 
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}