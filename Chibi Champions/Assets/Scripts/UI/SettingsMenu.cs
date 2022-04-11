using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;

    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] AudioMixer audioMixer;

    float savedVolume;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionList = new List<string>();


        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionList.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionList);

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetMute(bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.GetFloat("Volume", out savedVolume);
            audioMixer.SetFloat("Volume", -80);
        }
        else
        {
            audioMixer.SetFloat("Volume", savedVolume);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSensitivity(float sensitivity)
    {
        foreach(PlayerController player in FindObjectsOfType<PlayerController>())
        {
            if (player.GetIsPlayerCharacter())
            {
                player.SetMouseSensitivity(sensitivity);
            }
        }
    }
}
