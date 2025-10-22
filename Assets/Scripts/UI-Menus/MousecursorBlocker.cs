using UnityEngine;
using UnityEngine.EventSystems;

public class MousecursorBlocker : MonoBehaviour
{
    public GameObject defaultButton;
    void Start()
    {

    }

    // The system will force selection of the default button to restore navigation
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

    }
}