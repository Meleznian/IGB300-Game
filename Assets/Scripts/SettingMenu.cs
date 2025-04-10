using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject GraphicsPanel;
    public GameObject controlsPanel;
    public GameObject audioPanel;

    public AudioMixer mixer;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Toggle muteToggle;

    private float lastMasterVolume = 1f;
    private float lastMusicVolume = 1f;
    private float lastSFXVolume = 1f;

    void Start()
    {
        ShowPanel(GraphicsPanel);

        // Get saved volume preferences from PlayerPrefs
        lastMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 100f);
        lastMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 100f);
        lastSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 100f);

        // Check if saved volume data exists
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            // Set the mixer volume levels based on the saved player prefs
            SetMixerVolume("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            SetMixerVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            SetMixerVolume("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
            muteToggle.isOn = PlayerPrefs.GetInt("Muted", 0) == 1;
            SetSliders();
        }
        else
        {
            // No saved preferences, just set the sliders to defaults
            SetSliders();
        }

        // Add listener to mute toggle
        muteToggle.onValueChanged.AddListener(SetMute);
    }

    // Function to show specific panel
    public void ShowPanel(GameObject panel)
    {
        audioPanel.SetActive(false);
        GraphicsPanel.SetActive(false);
        controlsPanel.SetActive(false);

        panel.SetActive(true);
    }

    void SetSliders()
    {
        // Set sliders based on PlayerPrefs (assuming values are in 0-100 range)
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 100f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 100f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 100f);
    }

    public void UpdateMasterVolume()
    {
        // Update the mixer with the converted volume value
        SetMixerVolume("MasterVolume", MasterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", MasterSlider.value);
    }

    public void UpdateSFXVolume()
    {
        // Update the mixer with the converted volume value
        SetMixerVolume("SFXVolume", SFXSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }

    public void UpdateMusicVolume()
    {
        // Update the mixer with the converted volume value
        SetMixerVolume("MusicVolume", MusicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
    }

    public void SetMute(bool isMuted)
    {
        if (isMuted)
        {
            mixer.SetFloat("MasterVolume", -100f);
            mixer.SetFloat("MusicVolume", -100f);
            mixer.SetFloat("SFXVolume", -100f);
        }
        else
        {
            // Restore volumes based on the last saved values
            SetMixerVolume("MasterVolume", lastMasterVolume);
            SetMixerVolume("MusicVolume", lastMusicVolume);
            SetMixerVolume("SFXVolume", lastSFXVolume);
        }
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    private void SetMixerVolume(string parameterName, float value)
    {
        // If value is 0, apply -80dB (muted)
        if (value == 0)
        {
            mixer.SetFloat(parameterName, -80f);
        }
        else
        {
            // Convert the slider value (0-100) to a logarithmic decibel value
            float decibelValue = Mathf.Log10(value / 100f) * 20f;

            // Apply the value to the mixer parameter
            mixer.SetFloat(parameterName, decibelValue);
        }
    }
}
