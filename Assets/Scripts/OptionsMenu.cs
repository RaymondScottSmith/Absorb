using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider scrollSlider;
    [SerializeField] private Toggle autoStickToggle;
    [SerializeField] private Toggle scrollSliderToggle;

    [SerializeField] private Slider volumeSlider;

    [SerializeField] private AudioMixer mainAudioMixer;
    public void Start()
    {
        //Set Scroll Multiplier
        if (PlayerPrefs.HasKey("ScrollMultiplier"))
        {
            scrollSlider.value = PlayerPrefs.GetFloat("ScrollMultiplier");
        }
        else
        {
            scrollSlider.value = 0.5f;
            PlayerPrefs.SetFloat("ScrollMultiplier", 0.5f);
        }
        
        //Set autostick state
        if (PlayerPrefs.HasKey("AutoStick"))
        {
            autoStickToggle.isOn = PlayerPrefs.GetInt("AutoStick") == 1;
        }
        else
        {
            autoStickToggle.isOn = false;
            PlayerPrefs.SetInt("AutoStick", 0);
        }
        
        //Set Slider Zoom
        if (PlayerPrefs.HasKey("ScrollSlider"))
        {
            scrollSliderToggle.isOn = PlayerPrefs.GetInt("ScrollSlider") == 1;
        }
        else
        {
            scrollSliderToggle.isOn = false;
            PlayerPrefs.SetInt("ScrollSlider", 0);
        }
        
        //Set volume
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mainAudioMixer.SetFloat("MasterVolume", Mathf.Log(PlayerPrefs.GetFloat("MasterVolume")) * 20);
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }
        else
        {
            volumeSlider.value = 1f;
            mainAudioMixer.SetFloat("MasterVolume", 0);
        }

        Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
    }
    public void ScrollSpeedChanged(float newValue)
    {
        PlayerPrefs.SetFloat("ScrollMultiplier", newValue);
    }

    public void ChangeAutoStick(bool stick)
    {
        PlayerPrefs.SetInt("AutoStick", stick ? 1 : 0);
    }

    public void ChangeSliderZoom(bool sliderZoom)
    {
        PlayerPrefs.SetInt("ScrollSlider", sliderZoom ? 1 : 0);
    }

    public void MasterVolumeChanged(float newVolume)
    {
        PlayerPrefs.SetFloat("MasterVolume", newVolume);
        mainAudioMixer.SetFloat("MasterVolume", Mathf.Log(newVolume) * 20);
    }
}
