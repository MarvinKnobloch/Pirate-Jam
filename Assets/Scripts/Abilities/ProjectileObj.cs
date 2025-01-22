using UnityEngine;
using UpgradeSystem;

[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileObj : ScriptableObject
{
    public GameObject prefab;
    public float speed;
    public int damage;
    public int heal;
    public ProjectileType projectileType;
    public float aoeRange;
    public float maxBulletHeight;

    public int multipleProjectiles;
    public float multiShotAngle;

    public float timeToDestroy;

    public LayerMask hitLayer;

    public bool mirrorAttack;

    [Header("Area")]
    public bool createArea;
    public GameObject areaPrefab;

    [Header("Upgrades")]
    public Upgrades.UpgradeValues damageUpgrade;
    public Upgrades.UpgradeValues aoeSizeUpgrade;
    public Upgrades.UpgradeValues slowUpgrade;
}
    public enum ProjectileType{
    single,
    aoe,
    explosion,
    piercing,
    heal,
    summon,

}
