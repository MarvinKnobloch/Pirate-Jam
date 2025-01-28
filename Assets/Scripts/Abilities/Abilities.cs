using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability")]
public class Abilities : ScriptableObject
{
    public string AbilityName;
    public int AbilityID;
    public int AbilityCost;
    public float AbilityCooldown;
    public float AbilityTime;
    public ProjectileObj projectileObj;

    [Space]
    [TextArea][SerializeField] public string Description;

    [Space]
    public Tier abilityTier;
    [NonSerialized] public int maxLevel = 5;
    public int damageUpgradeValue;
    public int areaUpgradeValue;
    public int healUpgradeValue;
    public int lifestealUpgradeValue;

    public enum Tier
    {
        Tier1,
        Tier2,
    }


}


