using UnityEngine;

public class PageNavigator : MonoBehaviour
{
    public GameObject page1Panel;
    public GameObject page2Panel;
    public void GoToNextPage()
    {
        page1Panel.SetActive(false); 
        page2Panel.SetActive(true); 
    }

    public void GoToPreviousPage()
    {
        page2Panel.SetActive(false);
        page1Panel.SetActive(true);
    }
}
