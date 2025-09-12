using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class RisingText : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float stopTime;
    [SerializeField] float riseSpeed;
    [SerializeField] float fadeSpeed;

    [SerializeField] float moveDuration = 0.75f;  // shows how fast it flies
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] TMP_Text text;

    float timer;

    private Vector3 targetPos;
    private Action onArrive;

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

    public void Init(Vector3 target, Action callback)
    {
        targetPos = target;
        onArrive = callback;
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPos = transform.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(t));
            yield return null;
        }

        onArrive?.Invoke();
        Destroy(gameObject);
    }

    void Fade()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeSpeed));
    }
}
