using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UpgradeSystem;

public class AreaAbility : MonoBehaviour
{
    [SerializeField] Vector2 areaSize;
    [SerializeField] private AreaType areaCollider;
    [SerializeField] private float lifeTime;
    [SerializeField] private float tickInterval;
    [SerializeField] private int damage;
    [SerializeField] private int heal;
    [Range(0.6f, 1f)]
    [SerializeField] private float speedReduction;
    [SerializeField] private float speedReductionTime;
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
    public void SetAreaValues(ProjectileObj projectile)
    {
        damage = Upgrades.Instance.DamageUpgradeCalculation(damage, projectile.damageUpgrade.type, projectile.damageUpgrade.percentage);

        float scaling = Upgrades.Instance.AoeSizeCalculation(projectile.aoeSizeUpgrade.type, projectile.aoeSizeUpgrade.percentage);
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
        if(angle > 180)
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
            if (damage != 0)
            {
                if (obj.gameObject.transform.parent.TryGetComponent(out Health parentHealth))
                {
                    parentHealth.TakeDamage(damage);
                }
            }
            if (heal != 0)
            {

            }
            if (speedReduction < 1)
            {
                if (obj.gameObject.transform.parent.TryGetComponent(out EnemyController enemyController))
                {
                    float finalSlow = speedReduction - Upgrades.Instance.SlowCalculation();
                    if (finalSlow < 0.1f) finalSlow = 0.1f;
                    enemyController.DoSlow(finalSlow, speedReductionTime);
                }
            }
        }
    }
}
