using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UpgradeSystem;

public class AreaAbility : MonoBehaviour
{
    [SerializeField] private AreaType areaCollider;
    [SerializeField] Vector2 areaSize;
    [SerializeField] private float aoeSizeScaling;

    [Header("Damage")]
    [SerializeField] private int damageAmount;
    [SerializeField] private Upgrades.UpgradeType damageType;
    [SerializeField] private float damageScaling;
    [SerializeField] private float lifeTime;
    [SerializeField] private float tickInterval;

    [Header("Slow, Range 1 = Normal Movement Speed")]
    [SerializeField] private float slowDuration;
    [Range(0.6f, 1)]
    [SerializeField] private float slowStrength;

    [Header("Stun")]
    [SerializeField] private float stunDuration;

    [Header("Heal")]
    [SerializeField] private int healAmount;
    [SerializeField] private float healScaling;

    [Header("LifeSteal")]
    [SerializeField] private int lifeStealAmount;
    [SerializeField] private float lifeStealScaling;

    [Header("Other")]
    [SerializeField] private LayerMask hitLayer;


    private enum AreaType
    {
        Circle,
        Box,
    }
    private void Start()
    {
        CancelInvoke();
        Destroy(gameObject, lifeTime);
        InvokeRepeating("SetAreaCollider", 0.1f, tickInterval);
    }
    public void SetValues()
    {
        if (damageAmount != 0) damageAmount = Upgrades.Instance.DamageUpgradeCalculation(damageAmount, damageType, damageScaling);
        if (healAmount != 0) healAmount = Upgrades.Instance.HealCalculation(healAmount, healScaling);
        if (lifeStealAmount != 0) lifeStealAmount = Upgrades.Instance.LifeStealCalculation(lifeStealAmount, lifeStealScaling);

        float scaling = Upgrades.Instance.AoeSizeCalculation(aoeSizeScaling);
        areaSize.x *= scaling;
        areaSize.y *= scaling;
        transform.localScale = new Vector3(areaSize.x, areaSize.y, 1);
    }
    private void SetAreaCollider()
    {
        switch (areaCollider)
        {
            case AreaType.Circle:
                CheckInCircle();
                break;
            case AreaType.Box:
                CheckInBox();
                break;
        }
    }
    private void CheckInCircle()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * 0.5f, hitLayer);

        ExecuteAbility(cols);
    }
    private void CheckInBox()
    {
        float angle = transform.eulerAngles.z;
        if (angle > 180)
        {
            float difference = angle - 180;
            angle = (180 - difference) * -1;
        }

        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, areaSize, angle, hitLayer);

        ExecuteAbility(cols);
    }
    private void ExecuteAbility(Collider2D[] cols)
    {
        foreach (Collider2D obj in cols)
        {
            if (obj.gameObject.transform.parent.TryGetComponent(out EnemyController enemyController))
            {
                if (damageAmount != 0)
                {
                    enemyController.health.TakeDamage(damageAmount);
                }
                if (slowDuration > 0)
                {
                    enemyController.DoSlow(slowStrength - Upgrades.Instance.SlowCalculation(), slowDuration);
                }
                if (stunDuration > 0)
                {
                    enemyController.DoStun(Upgrades.Instance.StunCalculation(stunDuration));
                }
                if (lifeStealAmount > 0)
                {
                    Player.Instance.health.Heal(lifeStealAmount);
                }

            }
            else if (obj.transform.parent.TryGetComponent(out NPCController nPCController))
            {
                if (healAmount != 0) nPCController.health.Heal(healAmount);
            }
        }
    }
}
