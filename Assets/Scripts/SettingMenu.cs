using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject GraphicsPanel;
    public GameObject controlsPanel;
    public GameObject audioPanel;

    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;

    private float lastMasterVolume = 1f;

    void Start()
    {
        ShowPanel(GraphicsPanel);

        masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 1f);
        muteToggle.isOn = PlayerPrefs.GetInt("Muted", 0) == 1;

        ApplyVolumes();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        muteToggle.onValueChanged.AddListener(SetMute);

    }

    public void ShowPanel(GameObject panel)
    {

        audioPanel.SetActive(false);
        GraphicsPanel.SetActive(false);
        controlsPanel.SetActive(false);

 
        panel.SetActive(true);
    }

    public void SetMasterVolume(float value)
    {
        if (!muteToggle.isOn)
            audioMixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
        lastMasterVolume = value;
        PlayerPrefs.SetFloat("MasterVol", value);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVol", value);
    }

    public void SetMute(bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat("MasterVol", -80f); // Effectively mute
        }
        else
        {
            audioMixer.SetFloat("MasterVol", Mathf.Log10(lastMasterVolume) * 20);
        }
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    private void ApplyVolumes()
    {
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetMute(muteToggle.isOn);
    }
}
