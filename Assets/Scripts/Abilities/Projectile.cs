using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Abilities ability;
    private ProjectileObj projectile;
    [SerializeField] private Vector2 direction;


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
}
