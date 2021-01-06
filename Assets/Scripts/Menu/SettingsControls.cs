using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsControls : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Dropdown displayDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        populateResolutionsDropdown();

        // Set a proper value for the display dropdown. We're not using Maximized Window, so have to work around that too
        if (Screen.fullScreenMode == FullScreenMode.MaximizedWindow || Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            displayDropdown.value = 2;
        }
        else displayDropdown.value = (int) Screen.fullScreenMode;

        // Same for quality levels - we only use 3 out of 6
        if (QualitySettings.GetQualityLevel() == 0) qualityDropdown.value = 2;
        else if (QualitySettings.GetQualityLevel() == 3) qualityDropdown.value = 1;
        else if (QualitySettings.GetQualityLevel() == 5) qualityDropdown.value = 0;
        else
        {
            QualitySettings.SetQualityLevel(5);
            qualityDropdown.value = 0;
        }
    }

    private void populateResolutionsDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int idx = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + "x" + resolutions[i].height);

            if (resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].width == Screen.currentResolution.width)
            {
                idx = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = idx;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }

    public void SetVolumeBGM(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
    }

    public void SetResolution(int idx)
    {
        Resolution res = resolutions[idx];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetQuality(int idx)
    {
        switch (idx)
        {
            case 0:
                QualitySettings.SetQualityLevel(5);
                break;
            case 1:
                QualitySettings.SetQualityLevel(3);
                break;
            case 2:
                QualitySettings.SetQualityLevel(0);
                break;
        }
    }

    public void SetDisplayMode(int idx)
    {
        switch (idx)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
}
