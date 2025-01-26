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
    public int maxLevel;
    public int damageUpgradeValue;
    public float areaUpgradeValue;
    public int healUpgradeValue;
    public int lifestealUpgradeValue;
    public RessourceCosts[] upgradeCosts;

    [Serializable]
    public struct RessourceCosts
    {
        public int wood;
        public int copper;
        public int gold;
    }


}


