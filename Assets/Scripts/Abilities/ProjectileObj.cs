using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileObj : ScriptableObject
{
    public GameObject prefab;
    public float speed;
    public int damage;
    public int amount;
    public ProjectileType projectileType;
    public float timeToDestroy;
    public LayerMask hitLayer;
}
public enum ProjectileType{
    single,
    aoe,
    explosion,
    heal,
    summon,

}
