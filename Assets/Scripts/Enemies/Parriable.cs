using UnityEngine;

public class Parriable : MonoBehaviour
{
    public bool parriable = false;
    public BehaviourAgent agent;

    void Start()
    {
        agent = transform.parent.gameObject.GetComponent<BehaviourAgent>();
    }

    void Update()
    {
        agent.parriable = parriable;
    }

    public void StartParriable()
    {
        parriable = true;
    }

    public void EndParriable()
    {
        parriable = false;
    }
}
