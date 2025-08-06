using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown; // Use TMP_Dropdown for TextMeshPro
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    private Resolution[] resolutions;

    void Start()
    {
        // Populate resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Load saved volume settings
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        Debug.Log("Loaded Master Volume: " + masterVolume);
        Debug.Log("Loaded SFX Volume: " + sfxVolume);
        Debug.Log("Loaded Music Volume: " + musicVolume);

        if (audioMixer != null)
        {
            SetMasterVolume(masterVolume);
            SetSFXVolume(sfxVolume);
            SetMusicVolume(musicVolume);
        }

        if (masterVolumeSlider != null) masterVolumeSlider.SetValueWithoutNotify(masterVolume);
        if (sfxVolumeSlider != null) sfxVolumeSlider.SetValueWithoutNotify(sfxVolume);
        if (musicVolumeSlider != null) musicVolumeSlider.SetValueWithoutNotify(musicVolume);

        // Optional: Keep slider direction as RightToLeft for visual preference but no inversion in code.
        if (masterVolumeSlider != null) masterVolumeSlider.direction = Slider.Direction.RightToLeft;
        if (sfxVolumeSlider != null) sfxVolumeSlider.direction = Slider.Direction.RightToLeft;
        if (musicVolumeSlider != null) musicVolumeSlider.direction = Slider.Direction.RightToLeft;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        float displayVolume = volume;

        // Invert and clamp volume to valid range
        float invertedVolume = Mathf.Clamp(1f - volume, 0.0001f, 1f);

        // Convert to decibels
        float dB = LinearToDecibel(invertedVolume);

        // Apply to AudioMixer
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", dB);
        }
        else
        {
            Debug.LogWarning("AudioMixer is null. Cannot set Master volume.");
        }

        // Save to PlayerPrefs
        PlayerPrefs.SetFloat("MasterVolume", displayVolume);
        PlayerPrefs.Save();

        // Debug logs (only in development builds)
#if UNITY_EDITOR
        Debug.Log($"[Slider Raw Input] Master Volume: {displayVolume}");
        Debug.Log($"[SetMasterVolume] Volume: {volume} => {dB} dB");
#endif
    }

    public void SetSFXVolume(float volume)
    {
        float displayVolume = volume;

        // Invert and clamp volume to valid range
        float invertedVolume = Mathf.Clamp(1f - volume, 0.0001f, 1f);

        // Convert to decibels
        float dB = LinearToDecibel(invertedVolume);

        // Apply to AudioMixer
        if (audioMixer != null)
        {
            audioMixer.SetFloat("SFXVolume", dB);
        }
        else
        {
            Debug.LogWarning("AudioMixer is null. Cannot set SFX volume.");
        }

        // Save to PlayerPrefs
        PlayerPrefs.SetFloat("SFXVolume", displayVolume);
        PlayerPrefs.Save();

        // Debug logs (only in development builds)
#if UNITY_EDITOR
        Debug.Log($"[Slider Raw Input] SFX Volume: {displayVolume}");
        Debug.Log($"[SetSFXVolume] Volume: {volume} => {dB} dB");
#endif
    }

    public void SetMusicVolume(float volume)
    {
        float displayVolume = volume;

        // Invert and clamp volume to valid range
        float invertedVolume = Mathf.Clamp(1f - volume, 0.0001f, 1f);

        // Convert to decibels
        float dB = LinearToDecibel(invertedVolume);

        // Apply to AudioMixer
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", dB);
        }
        else
        {
            Debug.LogWarning("AudioMixer is null. Cannot set Music volume.");
        }

        // Save to PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", displayVolume);
        PlayerPrefs.Save();

        // Debug logs (only in development builds)
#if UNITY_EDITOR
        Debug.Log($"[Slider Raw Input] Music Volume: {displayVolume}");
        Debug.Log($"[SetMusicVolume] Volume: {volume} => {dB} dB");
#endif
    }

    private float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f)
            return -80f;
        return Mathf.Log10(linear) * 20f;
    }
    
    private bool isMuted = false;
    private float lastVolumeBeforeMute = 1f;

    public void ToggleMute()
    {
        if (!audioMixer.GetFloat("MasterVolume", out float currentVolume))
        {
            Debug.LogWarning("Couldn't get MasterVolume.");
            return;
        }

        if (!isMuted)
        {
            // Save current volume to restore later
            lastVolumeBeforeMute = Mathf.Pow(10, currentVolume / 20f);
            audioMixer.SetFloat("MasterVolume", -80f);
            isMuted = true;
        }
        else
        {
            // Restore previous volume
            float dB = LinearToDecibel(lastVolumeBeforeMute);
            audioMixer.SetFloat("MasterVolume", dB);
            isMuted = false;
        }
    }
}