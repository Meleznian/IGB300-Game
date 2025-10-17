using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderKeyboardControl : MonoBehaviour
{
    public float stepAmount = 0.05f; // how much the slider moves per arrow press

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        // Only react when this slider is currently selected
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                slider.value += stepAmount;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                slider.value -= stepAmount;
        }
    }
}
