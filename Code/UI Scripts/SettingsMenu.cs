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


    void Start()
    {
        LoadSettings();

        // nastaví počiatočné hodnoty podľa toho čo sme tam mali predtým
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

        SetQuality(PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel()));
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
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
}