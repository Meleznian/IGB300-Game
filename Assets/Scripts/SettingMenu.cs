using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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


        lastMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 100f);
        lastMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 100f);
        lastSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 100f);

     
        if (PlayerPrefs.HasKey("MasterVolume"))
        {

            SetMixerVolume("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            SetMixerVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            SetMixerVolume("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
            muteToggle.isOn = PlayerPrefs.GetInt("Muted", 0) == 1;
            SetSliders();
        }
        else
        {

            SetSliders();
        }
        muteToggle.onValueChanged.AddListener(SetMute);
    }


    public void ShowPanel(GameObject panel)
    {
        audioPanel.SetActive(false);
        GraphicsPanel.SetActive(false);
        controlsPanel.SetActive(false);

        panel.SetActive(true);
    }

    void SetSliders()
    {

        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 100f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 100f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 100f);
    }

    public void UpdateMasterVolume()
    {

        SetMixerVolume("MasterVolume", MasterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", MasterSlider.value);
    }

    public void UpdateSFXVolume()
    {

        SetMixerVolume("SFXVolume", SFXSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }

    public void UpdateMusicVolume()
    {

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
  
            float decibelValue = Mathf.Log10(value / 100f) * 20f;


            mixer.SetFloat(parameterName, decibelValue);
        }
    }
    public void CloseSettings()
    {
        // Fallback if PlayerPrefs is empty
        string lastScene = PlayerPrefs.GetString("LastScene", "MVPLevel");

        SceneManager.LoadScene(lastScene);
        Time.timeScale = 1f; 
    }


}
