using UnityEngine;

public class RisingText : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float stopTime;
    [SerializeField] float riseSpeed;

    float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= stopTime)
        {
            riseSpeed = 0;
        }
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }

       transform.position = new Vector2(transform.position.x, transform.position.y + (riseSpeed*Time.deltaTime));
    }
}
