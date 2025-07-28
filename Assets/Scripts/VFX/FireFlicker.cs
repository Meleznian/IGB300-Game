using UnityEngine;
using UnityEngine.UI;
public class FireFlicker : MonoBehaviour
{
    public Sprite fire1;
    public Sprite fire2;
    private Image image;
    private float timer = 0f;
    public float flickerInterval = 0.2f;
    private bool toggle = false;

    void Start()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("No Image component found!");
            enabled = false;
            return;
        }
        image.sprite = fire1;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= flickerInterval)
        {
            timer = 0f;
            toggle = !toggle;
            image.sprite = toggle ? fire1 : fire2;
        }
    }
}