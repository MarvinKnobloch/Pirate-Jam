using System;
using UnityEngine;
using UpgradeSystem;

[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileObj : ScriptableObject
{
    public GameObject prefab;
    public float speed;
    public ProjectileType projectileType;

    [Header("Damage")]
    public int damage;
    public Upgrades.UpgradeType damageType;
    public float damageScaling;
    public int damageOverTimeTicks;
    public float damageOverTimeInterval;

    [Header("AOE")]
    public float aoeRange;
    public float aoeSizeScaling;
    public float maxBulletHeight;

    [Header("MultiShot")]
    public int multipleProjectiles;
    public float multiShotAngle;

    [Header("Slow, Range 1 = Normal Movement Speed")]
    public float slowDuration;
    [Range(0.6f, 1)]
    public float slowStrength;

    [Header("Stun")]
    public float stunDuration;

    [Header("Heal")]
    public int healAmount;
    public float healScaling;

    [Header("LifeSteal")]
    public int lifeStealAmount;
    public float lifeStealScaling;

    [Header("Other")]
    public float timeToDestroy = 5;
    public LayerMask hitLayer;
    public bool mirrorAttack;
    public GameObject visualEffect;

    [Header("Area")]
    public bool createArea;
    public GameObject areaPrefab;

}
    public enum ProjectileType{
    single,
    aoe,
    explosion,
    piercing,
}
