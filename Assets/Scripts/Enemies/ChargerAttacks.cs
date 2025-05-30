using UnityEngine;

public class ChargerAttacks : MonoBehaviour
{
    [SerializeField] ChargerAgent agent;
    int bashDamage;
    Transform attackPoint;

    private void Start()
    {
        bashDamage = agent.bashDamage;
        attackPoint = agent.attackPoint;
    }

    public void Bash()
    {
        DealDamage(attackPoint.position, bashDamage);
        Debug.Log("Bashing");
        agent.attacking = false;
        print(agent.attacking);
    }

    void DealDamage(Vector2 origin, int damage)
    {
        float range = 1f;

        var hits = Physics2D.OverlapCircleAll(origin, range, LayerMask.NameToLayer("Player"));
        foreach (var h in hits)
        {
            var player = h.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

    }

}
