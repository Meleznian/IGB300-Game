using UnityEngine;
using TMPro;

public class RisingText : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float stopTime;
    [SerializeField] float riseSpeed;
    [SerializeField] float fadeSpeed;
    [SerializeField] TMP_Text text;

    float timer;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //if (timer >= stopTime)
        //{
        //    riseSpeed = 0;
        //}
        if (timer >= lifeTime)
        {
            Fade();
            if (text.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

        transform.position = new Vector2(transform.position.x, transform.position.y + (riseSpeed * Time.deltaTime));
    }


    void Fade()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeSpeed));
    }
}
