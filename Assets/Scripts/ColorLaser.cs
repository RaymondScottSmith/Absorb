using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColorLaser : MonoBehaviour
{
    public LaserColor color;
    private Collider2D laserColider;


    private void Start()
    {
        laserColider = GetComponentInChildren<BoxCollider2D>();
        TurnOnDamage();
    }

    public void TurnOffDamage()
    {
        laserColider.enabled = false;
    }

    public void TurnOnDamage()
    {
        laserColider.enabled = true;
    }
}

public enum LaserColor
{
    Red,
    Green,
    Blue,
    Purple,
    Orange,
    Yellow,
    None
}