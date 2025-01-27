using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UpgradeSystem;

public class Projectile : MonoBehaviour
{
    private int abilitySlot;
    private ProjectileObj projectile;
    private Vector2 direction;

    //stats
    private int damage;
    private float aoeSize;
    private int heal;
    private int lifeSteal;

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
    public void SetProjectileSingle(Abilities aby, Vector2 _direction, int _abilitySlot)
    {
        projectile = aby.projectileObj;
        direction = _direction;
        abilitySlot = _abilitySlot;

        StatsUpdate(_abilitySlot);

        Destroy(gameObject, projectile.timeToDestroy);
    }
    public void SetProjectileAOE(Abilities aby, Vector2 startPosi, Vector2 endPosi, int _abilitySlot)
    {
        projectile = aby.projectileObj;
        endPosition = endPosi;
        direction = ((Vector2)endPosi - (Vector2)transform.position).normalized;
        abilitySlot = _abilitySlot;

        StatsUpdate(_abilitySlot);

        distance = Vector2.Distance(endPosi, startPosi);

        startHeight = transform.position.z;
        currentSpeed = bulletSpeed * (1 + distance / distance * 3);
        maxHeight = projectile.maxBulletHeight; //distance / 3;
        bulletLifeTime = distance / currentSpeed;
        currentLifeTime = bulletLifeTime;

        Destroy(gameObject, projectile.timeToDestroy);
    }
    private void StatsUpdate(int _abilitySlot)
    {
        if (projectile.damage != 0) damage = Upgrades.Instance.DamageUpgradeCalculation(projectile.damage, projectile.damageType, projectile.damageScaling, _abilitySlot);
        if (projectile.aoeRange != 0)
        { 
            aoeSize = (projectile.aoeRange + Player.Instance.abilityController.slotUpgrades[abilitySlot].slotArea) * Upgrades.Instance.AoeSizeCalculation(projectile.aoeSizeScaling);
        }
        if (projectile.healAmount != 0) heal = Upgrades.Instance.HealCalculation(projectile.healAmount, projectile.healScaling, _abilitySlot);
        if (projectile.lifeStealAmount != 0) lifeSteal = Upgrades.Instance.LifeStealCalculation(projectile.lifeStealAmount, projectile.lifeStealScaling, _abilitySlot);

        float percentage = Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.BulletSpeed);
        bulletSpeed = projectile.speed + (percentage * 0.01f * projectile.speed);
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
        if (projectile.damageType == Upgrades.UpgradeType.DamageOverTime) DealDamageOverTime(obj);
        else
        {
            if (obj.transform.parent.TryGetComponent(out EnemyController enemyController))
            {
                EnemyInteraction(enemyController);
            }
            else if (obj.transform.parent.TryGetComponent(out NPCController nPCController))
            {
                if (projectile.healAmount != 0) NPCHeal(nPCController);
            }

            if (projectile.createArea)
            {
                GameObject area = Instantiate(projectile.areaPrefab, transform.position, transform.rotation);
                area.GetComponent<AreaAbility>().SetValues(abilitySlot);
            }
        }
    }

    private void DealAoeDamage()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, aoeSize, projectile.hitLayer);

        foreach (Collider2D obj in cols)
        {
            if (obj.gameObject.transform.parent.TryGetComponent(out EnemyController enemyController))
            {
                EnemyInteraction(enemyController);
            }
            else if (obj.transform.parent.TryGetComponent(out NPCController nPCController))
            {
                if (projectile.healAmount != 0) NPCHeal(nPCController);
            }
        }

        if (projectile.createArea)
        {
            GameObject area = Instantiate(projectile.areaPrefab, transform.position, transform.rotation);
            area.GetComponent<AreaAbility>().SetValues(abilitySlot);
        }
    }
    private void DealDamageOverTime(GameObject obj)
    {
        if (obj.transform.parent.TryGetComponent(out EnemyController enemyController))
        {
            if (projectile.damage != 0) enemyController.health.DamageOverTime(damage, projectile.damageOverTimeTicks, projectile.damageOverTimeInterval);

            if (projectile.slowDuration > 0)
            {
                enemyController.DoSlow(projectile.slowStrength - Upgrades.Instance.SlowCalculation(), projectile.slowDuration);
            }
            if (projectile.stunDuration > 0)
            {
                enemyController.DoStun(Upgrades.Instance.StunCalculation(projectile.stunDuration));
            }
            if (projectile.lifeStealAmount > 0)
            {
                Player.Instance.health.Heal(lifeSteal);
            }
        }


        if (projectile.createArea)
        {
            GameObject area = Instantiate(projectile.areaPrefab, transform.position, transform.rotation);
            area.GetComponent<AreaAbility>().SetValues(abilitySlot);
        }
    }

    private void EnemyInteraction(EnemyController enemyController)
    {
        if (projectile.damage != 0) enemyController.health.TakeDamage(damage);

        if (projectile.slowDuration > 0)
        {
            enemyController.DoSlow(projectile.slowStrength - Upgrades.Instance.SlowCalculation(), projectile.slowDuration);
        }
        if (projectile.stunDuration > 0)
        {
            enemyController.DoStun(Upgrades.Instance.StunCalculation(projectile.stunDuration));
        }
        if (projectile.lifeStealAmount > 0)
        {
            Player.Instance.health.Heal(lifeSteal);
        }
    }
    private void NPCHeal(NPCController nPCController)
    {
        nPCController.health.Heal(heal);
    }
}
