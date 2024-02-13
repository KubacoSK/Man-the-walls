using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string Music = "Music";
    public string Effects = "Effects";
    public Slider volumeSlider;
    public Slider effectsSlider;
    public Slider musicSlider;

    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        LoadSettings();
        // Initialize resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionsIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionsIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionsIndex);
        resolutionDropdown.RefreshShownValue();

        // Set initial values for volume sliders
        if (PlayerPrefs.HasKey("Volume"))
        {
            float volume = PlayerPrefs.GetFloat("Volume");
            audioMixer.SetFloat("Volume", volume);
            volumeSlider.value = volume;
        }
        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            float effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");
            audioMixer.SetFloat("Effects", effectsVolume);
            effectsSlider.value = effectsVolume;
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            audioMixer.SetFloat("Music", musicVolume);
            musicSlider.value = musicVolume;
        }

        // Set initial quality level and fullscreen mode
        SetQuality(PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel()));
        SetFullscreen(PlayerPrefs.GetInt("IsFullscreen", Screen.fullScreen ? 1 : 0) == 1);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetEffects(float volume)
    {
        audioMixer.SetFloat("Effects", volume);
        PlayerPrefs.SetFloat("EffectsVolume", volume);
    }

    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("IsFullscreen", isFullscreen ? 1 : 0);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            float volume = PlayerPrefs.GetFloat("Volume");
            audioMixer.SetFloat("Volume", volume);
        }
        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            float effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");
            audioMixer.SetFloat("Effects", effectsVolume);
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            audioMixer.SetFloat("Music", musicVolume);
        }
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            int qualityLevel = PlayerPrefs.GetInt("QualityLevel");
            QualitySettings.SetQualityLevel(qualityLevel);
        }
        if (PlayerPrefs.HasKey("IsFullscreen"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen") == 1;
            Screen.fullScreen = isFullscreen;
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
}