using UnityEngine;
using TMPro;

public class ScreenModeManager : MonoBehaviour
{
    public TMP_Dropdown displayModeDropdown;

    void Start()
    {
        int savedIndex = PlayerPrefs.GetInt("DisplayMode", 0);
        displayModeDropdown.value = savedIndex;
        displayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
        SetDisplayMode(savedIndex);
    }

    public void SetDisplayMode(int index)
    {
        switch (index)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Borderless
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        PlayerPrefs.SetInt("DisplayMode", index);
    }
}
