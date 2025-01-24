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

    [Header("AOE")]
    public float aoeRange;
    public float maxBulletHeight;

    [Header("MultiShot")]
    public int multipleProjectiles;
    public float multiShotAngle;

    [Header("Slow, Range 1 = Normal Movement Speed")]
    public bool slow;
    [Range(0.6f, 1)]
    public float slowStrength;
    public float slowDuration;

    [Header("Stun")]
    public bool stun;
    public float stunDuration;

    [Space]
    public float timeToDestroy;
    public LayerMask hitLayer;
    public bool mirrorAttack;

    [Header("Area")]
    public bool createArea;
    public GameObject areaPrefab;

    [Header("Upgrades")]
    public Upgrades.UpgradeValues damageUpgrade;
    public Upgrades.UpgradeValues aoeSizeUpgrade;
}
    public enum ProjectileType{
    single,
    aoe,
    explosion,
    piercing,
    heal,
    summon,

}
