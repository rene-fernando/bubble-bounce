using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameAudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Image muteButtonImage; 
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool isMuted = false;
    private float lastVolumeBeforeMute = 1f;

    private const string volumeKey = "MasterVolume";

    void Start()
    {
        
        float savedVolume = PlayerPrefs.GetFloat(volumeKey, 1f);
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        if (isMuted)
        {
            audioMixer.SetFloat(volumeKey, -80f);
            muteButtonImage.sprite = soundOffSprite;
        }
        else
        {
            audioMixer.SetFloat(volumeKey, LinearToDecibel(savedVolume));
            muteButtonImage.sprite = soundOnSprite;
        }

        lastVolumeBeforeMute = savedVolume;
    }

    public void ToggleMute()
    {
        if (!isMuted)
        {
            audioMixer.SetFloat(volumeKey, -80f);
            muteButtonImage.sprite = soundOffSprite;
            isMuted = true;
        }
        else
        {
            audioMixer.SetFloat(volumeKey, LinearToDecibel(lastVolumeBeforeMute));
            muteButtonImage.sprite = soundOnSprite;
            isMuted = false;
        }

        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f)
            return -80f;
        return Mathf.Log10(linear) * 20f;
    }
}