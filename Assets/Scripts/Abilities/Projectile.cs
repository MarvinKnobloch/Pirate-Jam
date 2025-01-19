using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Abilities ability;
    private ProjectileObj projectile;
    private Vector2 direction;

    void Update()
    {
        if(projectile != null){
            switch (projectile.projectileType){
                case ProjectileType.single:
                StraightMovement();
                break;
            }
        }
    }
    public void SetProjectile(Abilities aby, Vector2 _direction){
        ability = aby;
        projectile = aby.projectileObj;
        direction = _direction;

        Destroy(gameObject, projectile.timeToDestroy);
    }

    private void StraightMovement(){
        transform.Translate(direction * projectile.speed * Time.deltaTime, Space.World);
        transform.right = direction;
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
                    break;
            }
        }
    }
    private void DealSingleDamage(GameObject obj)
    {
        if(obj.TryGetComponent(out Health health))
        {
            health.TakeDamage(projectile.damage);
        }
        gameObject.SetActive(false);
    }
}
