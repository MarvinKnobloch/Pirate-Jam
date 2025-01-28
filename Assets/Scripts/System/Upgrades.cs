using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UpgradeSystem.Upgrades;

namespace UpgradeSystem
{
    public class Upgrades : MonoBehaviour
    {
        public static Upgrades Instance;

        private int healthUpgradeValue;
        private int minionHealthUpgradeValue;
        private int damageUpgradeValue;
        private float damageOverTimeUpgradeValue;
        private int maxEnergyUpgradeValue;
        private float energyIntervalUpgradeValue;
        private float aoeSizeUpgradeValue;
        private float slowUpgradeValue;
        private float stunUpgradeValue;
        private float expGainUpgradeValue;
        private float cooldownUpgradeValue;
        private float gatherSpeedUpgradeValue;
        private float bulletSpeedUpgradeValue;
        private float damagePercentageUpgradeValue;
        private int healUpgradeValue;
        private float lifeStealUpgradeValue;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public float GetUpgradeStat(UpgradeType slot)
        {
            switch (slot)
            {
                case UpgradeType.PlayerHealth:
                    return healthUpgradeValue;
                case UpgradeType.MinionHealth:
                    return minionHealthUpgradeValue;
                case UpgradeType.Damage:
                    return damageUpgradeValue;
                case UpgradeType.DamageOverTime:
                    return damageOverTimeUpgradeValue;
                case UpgradeType.MaxEnergy:
                    return maxEnergyUpgradeValue;
                case UpgradeType.EnergyInterval:
                    return energyIntervalUpgradeValue;
                case UpgradeType.AoeSize:
                    return aoeSizeUpgradeValue;
                case UpgradeType.Slow:
                    return slowUpgradeValue;
                case UpgradeType.Stun:
                    return stunUpgradeValue;
                case UpgradeType.ExpGain:
                    return expGainUpgradeValue;
                case UpgradeType.Cooldown:
                    return cooldownUpgradeValue;
                case UpgradeType.GatherSpeed:
                    return gatherSpeedUpgradeValue;
                case UpgradeType.BulletSpeed:
                    return bulletSpeedUpgradeValue;
                case UpgradeType.DamagePercentage:
                    return damagePercentageUpgradeValue;
                case UpgradeType.Heal:
                    return healUpgradeValue;
                case UpgradeType.LifeSteal:
                    return lifeStealUpgradeValue;
            }
            return 0;
        }
        public void IncreaseValue(UpgradeType slot, float value)
        {
            switch (slot)
            {
                case UpgradeType.PlayerHealth:
                    healthUpgradeValue += Mathf.RoundToInt(value);
                    break;
                case UpgradeType.MinionHealth:
                    minionHealthUpgradeValue += Mathf.RoundToInt(value);
                    break;
                case UpgradeType.Damage:
                    damageUpgradeValue += Mathf.RoundToInt(value);
                    break;
                case UpgradeType.DamageOverTime:
                    damageOverTimeUpgradeValue += value;
                    break;
                case UpgradeType.MaxEnergy:
                    maxEnergyUpgradeValue += Mathf.RoundToInt(value);
                    break;
                case UpgradeType.EnergyInterval:
                    energyIntervalUpgradeValue += value;
                    break;
                case UpgradeType.AoeSize:
                    aoeSizeUpgradeValue += value;
                    break;
                case UpgradeType.Slow:
                    slowUpgradeValue += value;
                    break;
                case UpgradeType.Stun:
                    stunUpgradeValue += value;
                    break;
                case UpgradeType.ExpGain:
                    expGainUpgradeValue += value;
                    break;
                case UpgradeType.Cooldown:
                    cooldownUpgradeValue += value;
                    break;
                case UpgradeType.GatherSpeed:
                    gatherSpeedUpgradeValue += value;
                    break;
                case UpgradeType.BulletSpeed:
                    bulletSpeedUpgradeValue += value;
                    break;
                case UpgradeType.DamagePercentage:
                    damagePercentageUpgradeValue += value;
                    break;
                case UpgradeType.Heal:
                    healUpgradeValue += Mathf.RoundToInt(value);
                    break;
                case UpgradeType.LifeSteal:
                    lifeStealUpgradeValue += value;
                    break;
            }
        }
        public int DamageUpgradeCalculation(int baseDamage, UpgradeType upgradeType, float percentage, int abilitySlot)
        {
            int damage = baseDamage + Player.Instance.abilityController.slotUpgrades[abilitySlot].slotDamage;
            int finalDamage = damage + Mathf.RoundToInt(Upgrades.Instance.GetUpgradeStat(upgradeType) * (percentage * 0.01f));
            finalDamage += Mathf.RoundToInt(damagePercentageUpgradeValue * 0.01f * finalDamage);
            return finalDamage;
        }
        public float AoeSizeCalculation(int upgradeAoeSize, float percentage)
        {
            return (aoeSizeUpgradeValue + upgradeAoeSize) * (percentage * 0.0001f);
        }
        public float SlowCalculation()
        {
            float slow = GetUpgradeStat(UpgradeType.Slow) * 0.01f;
            if (slow < 0.1f) slow = 0.1f;
            return slow;
        }
        public float StunCalculation(float stunDuration)
        {
            return stunDuration + (stunUpgradeValue * 0.01f * stunDuration);
        }
        public int HealCalculation(int baseHeal, float percentage, int abilitySlot)
        {
            int heal = baseHeal + Player.Instance.abilityController.slotUpgrades[abilitySlot].slotHeal;
            return heal + Mathf.RoundToInt(healUpgradeValue * (percentage * 0.01f));
        }
        public int LifeStealCalculation(int baseLifesteal, float percentage, int abilitySlot)
        {
            int lifesteal = baseLifesteal + Player.Instance.abilityController.slotUpgrades[abilitySlot].slotLifesteal;
            return lifesteal + Mathf.RoundToInt(lifeStealUpgradeValue * (percentage * 0.01f));
        }


        [Serializable]
        public struct UpgradeValues
        {
            public UpgradeType type;
            public float percentage;
        }
        [Serializable]
        public enum UpgradeType
        {
            Empty,
            PlayerHealth,
            MinionHealth,
            Damage,
            DamageOverTime,
            MaxEnergy,
            EnergyInterval,
            AoeSize,
            Slow,
            Stun,
            ExpGain,
            Cooldown,
            GatherSpeed,
            BulletSpeed,
            DamagePercentage,
            Heal,
            LifeSteal,
        }
    }
}


