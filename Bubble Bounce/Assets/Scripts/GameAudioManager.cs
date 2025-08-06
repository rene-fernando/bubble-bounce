using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class GameAudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TextMeshProUGUI muteButtonText;

    private bool isMuted = false;
    private float lastVolumeBeforeMute = 1f;

    private const string volumeKey = "MasterVolume";

    void Start()
    {
        // Load saved volume from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat(volumeKey, 1f);
        lastVolumeBeforeMute = savedVolume;
        float dB = LinearToDecibel(savedVolume);
        audioMixer.SetFloat("MasterVolume", dB);
    }

    public void ToggleMute()
    {
        if (!audioMixer.GetFloat("MasterVolume", out float currentVolume))
        {
            Debug.LogWarning("Couldn't get MasterVolume.");
            return;
        }

        if (!isMuted)
        {
            lastVolumeBeforeMute = Mathf.Pow(10f, currentVolume / 20f);
            audioMixer.SetFloat("MasterVolume", -80f);
            isMuted = true;
        }
        else
        {
            float dB = LinearToDecibel(lastVolumeBeforeMute);
            audioMixer.SetFloat("MasterVolume", dB);
            isMuted = false;
        }

        if (muteButtonText != null)
        {
            muteButtonText.text = isMuted ? "Unmute" : "Mute";
        }
    }

    private float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f)
            return -80f;
        return Mathf.Log10(linear) * 20f;
    }
}