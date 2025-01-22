using System;
using System.Collections;
using UnityEngine;

namespace UpgradeSystem 
{
    public static class Upgrades
    {
        private static int healthValue;
        private static int damageValue;

        public static float GetUpgradeStat(UpgradeType slot)
        {
            switch (slot)
            {
                case UpgradeType.Health:
                    return healthValue;
                case UpgradeType.Damage:
                    return damageValue;
            }
            return 0;
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
            Damage,
            AoeSize,
            Slow
        }
    }
}

