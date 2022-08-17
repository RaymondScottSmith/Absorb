using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider scrollSlider;
    public void Start()
    {
        if (PlayerPrefs.HasKey("ScrollMultiplier"))
        {
            scrollSlider.value = PlayerPrefs.GetFloat("ScrollMultiplier");
        }
        else
        {
            scrollSlider.value = 0.5f;
            PlayerPrefs.SetFloat("ScrollMultiplier", 0.5f);
            
        }
    }
    public void ScrollSpeedChanged(float newValue)
    {
        PlayerPrefs.SetFloat("ScrollMultiplier", newValue);
    }
}
