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
        // Disable automatic navigation for all elements
        foreach (var ui in uiElements)
        {
            if (ui == null) continue;
            Navigation nav = ui.navigation;
            nav.mode = Navigation.Mode.None;
            ui.navigation = nav;
        }

        if (uiElements.Count > 0)
            SelectElement(0);
    }

    void Update()
    {
        if (uiElements.Count == 0) return;

        // Move Up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-1);
        }

        // Move Down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(1);
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

        // Ensure EventSystem always points to the correct element
        if (EventSystem.current.currentSelectedGameObject != uiElements[currentIndex].gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(uiElements[currentIndex].gameObject);
        }
    }

    void MoveSelection(int direction)
    {
        if (uiElements.Count == 0) return;

        int newIndex = currentIndex;

        do
        {
            newIndex = (newIndex + direction + uiElements.Count) % uiElements.Count;
        }
        while (uiElements[newIndex] == null || !uiElements[newIndex].gameObject.activeInHierarchy);

        SelectElement(newIndex);
    }

    void SelectElement(int index)
    {
        if (uiElements[index] == null) return;

        currentIndex = index;

        // Deselect and delay select to force refresh
        StartCoroutine(ReselectAfterFrame(uiElements[index].gameObject));
    }

    System.Collections.IEnumerator ReselectAfterFrame(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null; // wait one frame
        EventSystem.current.SetSelectedGameObject(go);
        Debug.Log("Selected: " + go.name);
    }
}
