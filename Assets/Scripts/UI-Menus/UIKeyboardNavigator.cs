using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIKeyboardNavigator : MonoBehaviour
{
    [Tooltip("Assign all your UI elements here in the order you want them navigated.")]
    public List<Selectable> uiElements = new List<Selectable>();

    private int currentIndex = 0;

    void Start()
    {
        if (uiElements.Count > 0)
            SelectElement(0);
    }

    void Update()
    {
        if (uiElements.Count == 0) return;

        // Move Up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = uiElements.Count - 1;
            SelectElement(currentIndex);
        }

        // Move Down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if (currentIndex >= uiElements.Count) currentIndex = 0;
            SelectElement(currentIndex);
        }

        // Adjust sliders
        if (uiElements[currentIndex] is Slider slider)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                slider.value -= Time.unscaledDeltaTime * 0.5f;

            if (Input.GetKey(KeyCode.RightArrow))
                slider.value += Time.unscaledDeltaTime * 0.5f;
        }

        // Toggle mute on space or enter
        if (uiElements[currentIndex] is Toggle toggle)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                toggle.isOn = !toggle.isOn;
        }

        // Press buttons with Enter/Space
        if (uiElements[currentIndex] is Button button)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                button.onClick.Invoke();
        }
    }

    void SelectElement(int index)
    {
        if (uiElements[index] == null) return;
        EventSystem.current.SetSelectedGameObject(uiElements[index].gameObject);
    }
}
