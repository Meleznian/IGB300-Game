using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstButtonLocate : MonoBehaviour
{
    private Selectable firstSelectable;

    void OnEnable()
    {
        // Wait and to ensure the UI is ready
        StartCoroutine(FocusAfterFrame());
    }

    private System.Collections.IEnumerator FocusAfterFrame()
    {
        yield return null;

        // Try to find the first interactable button or selectable
        FindFirstSelectable();

        if (firstSelectable != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
        }
    }

    void Update()
    {
        // Reassign selection if player clicks outside
        if (gameObject.activeInHierarchy &&
            EventSystem.current.currentSelectedGameObject == null &&
            firstSelectable != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
        }
    }

    private void FindFirstSelectable()
    {
        // Search for first active and interactable button in this canvas
        Selectable[] allSelectables = GetComponentsInChildren<Selectable>(true);

        foreach (var selectable in allSelectables)
        {
            if (selectable.gameObject.activeInHierarchy && selectable.interactable)
            {
                firstSelectable = selectable;
                break;
            }
        }
    }
}
