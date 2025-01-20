using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    private Abilities ability;
    private ProjectileObj projectile;
    private Vector2 direction;

    //AOE
    [SerializeField] private GameObject aoeVisualBullet;
    private Vector3 endPosition;
    private Vector3 lastPosition;
    private float currentSpeed;
    private float distance;
    private float startHeight;
    private float maxHeight;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float currentLifeTime;

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

        Destroy(gameObject, projectile.timeToDestroy);
    }
    public void SetProjectileAOE(Abilities aby, Vector2 startPosi, Vector2 endPosi)
    {
        ability = aby;
        projectile = aby.projectileObj;
        endPosition = endPosi;
        direction = ((Vector2)endPosi - (Vector2)transform.position).normalized;

        distance = Vector2.Distance(endPosi, startPosi);

        startHeight = transform.position.z;
        currentSpeed = projectile.speed * (1 + distance / distance * 3);
        maxHeight = projectile.maxBulletHeight; //distance / 3;
        bulletLifeTime = distance / currentSpeed;
        currentLifeTime = bulletLifeTime;

        Destroy(gameObject, projectile.timeToDestroy);
    }

    private void StraightMovement()
    {
        transform.Translate(direction * projectile.speed * Time.deltaTime, Space.World);
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
        Debug.Log(collision.gameObject);
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
        if (obj.TryGetComponent(out Health health))
        {
            health.TakeDamage(projectile.damage);
        }
        else if (obj.transform.parent.TryGetComponent(out Health parentHealth))
        {
            parentHealth.TakeDamage(projectile.damage);
        }

        if (projectile.createArea)
        {
            GameObject area = Instantiate(projectile.areaPrefab, transform.position, Quaternion.identity);
        }
    }

    private void DealAoeDamage()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, projectile.aoeRange, projectile.hitLayer);

        foreach (Collider2D enemy in cols)
        {
            if (enemy.gameObject.TryGetComponent(out Health health))
            {
                health.TakeDamage(projectile.damage);
            }
            else if (enemy.gameObject.transform.parent.TryGetComponent(out Health parentHealth))
            {
                parentHealth.TakeDamage(projectile.damage);
            }
        }
    }
}
