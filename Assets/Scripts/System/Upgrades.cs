using System;
using System.Collections;
using UnityEngine;

namespace UpgradeSystem 
{
    public static class Upgrades
    {
        private static int healthUpgradeValue;
        private static int minionHealthUpgradeValue;
        private static int damageUpgradeValue;
        private static int maxEnergyUpgradeValue;
        private static float energyIntervalUpgradeValue;
        private static float aoeSizeUpgradeValue;
        private static float slowUpgradeValue;
        private static float stunUpgradeValue;

        public static float GetUpgradeStat(UpgradeType slot)
        {
            switch (slot)
            {
                case UpgradeType.Health:
                    return healthUpgradeValue;
                case UpgradeType.MinionHealth:
                    return minionHealthUpgradeValue;
                case UpgradeType.Damage:
                    return damageUpgradeValue;
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
            }
            return 0;
        }
        public static void IncreaseValue(UpgradeType slot, float value)
        {
            switch (slot)
            {
                case UpgradeType.Health:
                    healthUpgradeValue += Mathf.RoundToInt(value);
                    break;
                case UpgradeType.MinionHealth:
                    minionHealthUpgradeValue += Mathf.RoundToInt(value);
                    break;
                case UpgradeType.Damage:
                    damageUpgradeValue += Mathf.RoundToInt(value);
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
            }
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
            Health,
            MinionHealth,
            Damage,
            MaxEnergy,
            EnergyInterval,
            AoeSize,
            Slow,
            Stun,
        }
    }
}

