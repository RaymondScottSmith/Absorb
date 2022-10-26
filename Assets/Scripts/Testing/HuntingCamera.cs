using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingCamera : MonoBehaviour
{

    private bool isHunting;

    private Vector2 areaMins, areaMaxes;

    private CamHuntState myState;

    private Vector3 targetPos;

    [SerializeField]
    private float searchSpeed = 2f;

    public void StartHunting(Vector2 areaMins, Vector2 areaMaxes)
    {
        this.areaMins = areaMins;
        this.areaMaxes = areaMaxes;
        isHunting = true;
        StartSearching();
    }

    private void StartSearching()
    {
        targetPos = FindRandomPos();
        myState = CamHuntState.Searching;
    }

    private void FixedUpdate()
    {
        switch (myState)
        {
            case CamHuntState.Searching:

                Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, searchSpeed * Time.deltaTime);
                Debug.Log(newPos);
                transform.position = newPos;
                if (Vector3.Distance(transform.position, targetPos) < 0.5f)
                    targetPos = FindRandomPos();
                break;
        }
    }

    private Vector3 FindRandomPos()
    {
        float xPos = Random.Range(areaMins.x, areaMaxes.x);

        float yPos = Random.Range(areaMins.y, areaMaxes.y);

        return new Vector3(xPos, yPos, transform.position.z);
    }

    public void StopHunting()
    {
        isHunting = false;
    }

    private enum CamHuntState
    {
        Off,
        Searching,
        Detected,
        Shooting
    }
}
