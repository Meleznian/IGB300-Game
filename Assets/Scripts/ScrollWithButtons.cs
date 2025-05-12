using UnityEngine;
using UnityEngine.UI;

public class MultiScrollHandler : MonoBehaviour
{
    [Header("Multiple Scrollbars")]
    public Scrollbar[] scrollbars;
    public float scrollStep = 0.05f;


    public void ScrollUp(int index)
    {
        if (IsValidIndex(index))
            scrollbars[index].value += scrollStep;
    }

    public void ScrollDown(int index)
    {
        if (IsValidIndex(index))
            scrollbars[index].value -= scrollStep;
    }

    private bool IsValidIndex(int index)
    {
        return scrollbars != null && index >= 0 && index < scrollbars.Length;
    }
}
