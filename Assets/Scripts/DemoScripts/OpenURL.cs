using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void NavigateToSteam()
    {
        Application.OpenURL("https://store.steampowered.com/app/2152970/Absorb/");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenTransition.Instance.BackToMenu();
        }
    }
}
