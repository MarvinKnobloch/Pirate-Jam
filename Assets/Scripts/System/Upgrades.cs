using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        private int healValue;

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
                    return healValue;
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
                    healValue += Mathf.RoundToInt(value);
                    break;
            }
        }
        public int DamageUpgradeCalculation(int baseDamage, UpgradeType upgradeType, float percentage)
        {
            int finalDamage = baseDamage + Mathf.RoundToInt(Upgrades.Instance.GetUpgradeStat(upgradeType) * (percentage * 0.01f));
            finalDamage += Mathf.RoundToInt(damagePercentageUpgradeValue * 0.01f * finalDamage);
            return finalDamage;
        }
        public float AoeSizeCalculation(UpgradeType upgradeType, float percentage)
        {
            return (1 + GetUpgradeStat(upgradeType) * (percentage * 0.0001f));
        }
        public float SlowCalculation()
        {
            return GetUpgradeStat(UpgradeType.Slow) * 0.01f;
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
        }
    }
}


