using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileObj : ScriptableObject
{
    public GameObject prefab;
    public float speed;
    public int damage;
    public ProjectileType projectileType;
    public float aoeRange;
    public int multipleProjectiles;
    public float maxBulletHeight;
    public float timeToDestroy;
    public LayerMask hitLayer;
}
public enum ProjectileType{
    single,
    aoe,
    explosion,
    piercing,
    heal,
    summon,

}
