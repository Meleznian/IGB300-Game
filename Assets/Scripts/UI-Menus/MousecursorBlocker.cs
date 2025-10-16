using UnityEngine;
using UnityEngine.EventSystems;

public class MousecursorBlocker : MonoBehaviour
{
    public GameObject defaultButton;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
    }
}
