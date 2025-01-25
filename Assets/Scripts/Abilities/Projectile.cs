using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UpgradeSystem;

public class Projectile : MonoBehaviour
{
    private Abilities ability;
    private ProjectileObj projectile;
    private Vector2 direction;
    private int damage;
    private float aoeSize;

    //AOE
    [SerializeField] private GameObject aoeVisualBullet;
    private Vector3 endPosition;
    private Vector3 lastPosition;
    private float bulletSpeed;
    private float currentSpeed;
    private float distance;
    private float startHeight;
    private float maxHeight;
    private float bulletLifeTime;
    private float currentLifeTime;


    void Update()
    {
        if (projectile != null)
        {
            switch (projectile.projectileType)
            {
                case ProjectileType.single:
                    StraightMovement();
                    break;
                case ProjectileType.aoe:
                    CurveMovement();
                    break;
                case ProjectileType.explosion:
                    StraightMovement();
                    break;
                case ProjectileType.piercing:
                    StraightMovement();
                    break;
            }
        }
    }
    public void SetProjectileSingle(Abilities aby, Vector2 _direction)
    {
        ability = aby;
        projectile = aby.projectileObj;
        direction = _direction;

        StatsUpdate();

        Destroy(gameObject, projectile.timeToDestroy);
    }
    public void SetProjectileAOE(Abilities aby, Vector2 startPosi, Vector2 endPosi)
    {
        ability = aby;
        projectile = aby.projectileObj;
        endPosition = endPosi;
        direction = ((Vector2)endPosi - (Vector2)transform.position).normalized;

        StatsUpdate();

        distance = Vector2.Distance(endPosi, startPosi);

        startHeight = transform.position.z;
        currentSpeed = bulletSpeed * (1 + distance / distance * 3);
        maxHeight = projectile.maxBulletHeight; //distance / 3;
        bulletLifeTime = distance / currentSpeed;
        currentLifeTime = bulletLifeTime;

        Destroy(gameObject, projectile.timeToDestroy);
    }
    private void StatsUpdate()
    {
        if (projectile.damage != 0) damage = projectile.damage
            + Upgrades.Instance.DamageUpgradeCalculation(projectile.damage, projectile.damageUpgrade.type, projectile.damageUpgrade.percentage)
            + Mathf.RoundToInt(AbilityUpgradeController.Instance.DamageUpgrade);
        if (projectile.aoeRange != 0) aoeSize = projectile.aoeRange
            * Upgrades.Instance.AoeSizeCalculation(projectile.aoeSizeUpgrade.type, projectile.aoeSizeUpgrade.percentage)
            * AbilityUpgradeController.Instance.AOERangeUpgrade;
    }

    private void StraightMovement()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.World);
        transform.right = direction;
    }
    private void CurveMovement()
    {
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            DealAoeDamage();
            Destroy(gameObject);
        }
        transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);

        if (aoeVisualBullet != null)
        {
            lastPosition = aoeVisualBullet.transform.position;
            if (maxHeight > 0)
            {
                Vector3 deltaHeight = Vector3.back * startHeight * (1 - currentLifeTime / bulletLifeTime) + new Vector3(0, 0, -maxHeight * Mathf.Sin((currentLifeTime / bulletLifeTime) * Mathf.PI));
                aoeVisualBullet.transform.position = transform.position + deltaHeight;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject);
        if (((1 << collision.gameObject.layer) & projectile.hitLayer) != 0)
        {
            switch (projectile.projectileType)
            {
                case ProjectileType.single:
                    DealSingleDamage(collision.gameObject);
                    Destroy(gameObject);
                    break;
                case ProjectileType.explosion:
                    DealAoeDamage();
                    Destroy(gameObject);
                    break;
                case ProjectileType.piercing:
                    DealSingleDamage(collision.gameObject);
                    break;
            }
        }
    }
    private void DealSingleDamage(GameObject obj)
    {
        if (obj.transform.parent.TryGetComponent(out EnemyController enemyController))
        {
            EnemyInteraction(enemyController);
            // if (projectile.heal != 0) parentHealth.Heal(projectile.heal + Mathf.RoundToInt(AbilityUpgradeController.Instance.HealUpgrade));
        }

        if (projectile.createArea)
        {
            GameObject area = Instantiate(projectile.areaPrefab, transform.position, transform.rotation);
            area.GetComponent<AreaAbility>().SetAreaValues(projectile);
        }
    }

    private void DealAoeDamage()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, aoeSize, projectile.hitLayer);

        foreach (Collider2D enemy in cols)
        {
            if (enemy.gameObject.transform.parent.TryGetComponent(out EnemyController enemyController))
            {
                EnemyInteraction(enemyController);
            }
        }
    }

    private void EnemyInteraction(EnemyController enemyController)
    {
        if (projectile.damage != 0) enemyController.health.TakeDamage(damage);

        if (projectile.slow)
        {
            enemyController.DoSlow(projectile.slowStrength - Upgrades.Instance.SlowCalculation(), projectile.slowDuration);
        }
        if (projectile.stun)
        {
            enemyController.DoStun(Upgrades.Instance.StunCalculation(projectile.stunDuration));
        }
    }
}
