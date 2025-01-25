using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeController : MonoBehaviour
{
    public static AbilityUpgradeController Instance;

    public float SpeedUpgrade = 0.0f;
    public float DamageUpgrade = 0.0f;
    public float HealUpgrade = 0.0f;
    public float SpeedReductionUpgrade = 0.0f;
    public float AOERangeUpgrade = 0.0f;

    private int _upgradeLevel = 1;
    private int _abilityLevel = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public bool PurchaseAbility()
    {
        if (_abilityLevel < 3)
        {
            if (Player.Instance.SubtractResources(new() { { ResourceType.Iron, 4 * _abilityLevel } }))
            {
                _abilityLevel++;
                return true;
            }
        }
        else if (_abilityLevel < 8)
        {
            if (
                Player.Instance.SubtractResources(new() {
                    { ResourceType.Iron, _abilityLevel },
                    { ResourceType.Copper, 4 * _abilityLevel }
                })
            )
            {
                _abilityLevel++;
                return true;
            }
        }
        else
        {
            if (
                Player.Instance.SubtractResources(new() {
                    { ResourceType.Iron, _abilityLevel },
                    { ResourceType.Copper, _abilityLevel },
                    { ResourceType.Wood, _abilityLevel }
                })
            )
            {
                _abilityLevel++;
                return true;
            }
        }

        return false;
    }

    private bool PurchaseUpgrade()
    {
        if (_upgradeLevel < 6)
        {
            if (Player.Instance.SubtractResources(new() { { ResourceType.Iron, 4 * _abilityLevel } }))
            {
                _upgradeLevel++;
                return true;
            }
        }
        else if (_upgradeLevel < 16)
        {
            if (
                Player.Instance.SubtractResources(new() {
                    { ResourceType.Iron, _abilityLevel },
                    { ResourceType.Copper, 4 * _abilityLevel }
                })
            )
            {
                _upgradeLevel++;
                return true;
            }
        }
        else
        {
            if (
                Player.Instance.SubtractResources(new() {
                    { ResourceType.Iron, _abilityLevel },
                    { ResourceType.Copper, _abilityLevel },
                    { ResourceType.Wood, _abilityLevel }
                })
            )
            {
                _upgradeLevel++;
                return true;
            }
        }

        return false;
    }

    public bool UpgradeAbility(int abilityIndex)
    {
        if (!PurchaseUpgrade())
        {
            return false;
        }

        switch (abilityIndex)
        {
            case 0:
                UpgradeAbility1();
                break;
            case 1:
                UpgradeAbility2();
                break;
            case 2:
                UpgradeAbility3();
                break;
            case 3:
                UpgradeAbility4();
                break;
            case 4:
                UpgradeAbility5();
                break;
            case 5:
                UpgradeAbility6();
                break;
            case 6:
                UpgradeAbility7();
                break;
        }

        return true;
    }

    // Cannon
    public void UpgradeAbility1()
    {
        SpeedUpgrade += 0.1f;
        DamageUpgrade += 0.1f;
    }

    // Explosion
    public void UpgradeAbility2()
    {
        DamageUpgrade += 0.1f;
        AOERangeUpgrade += 0.25f;
    }

    // FireArea
    public void UpgradeAbility3()
    {
        SpeedReductionUpgrade += 0.1f;
        AOERangeUpgrade += 0.1f;
    }

    // Heal
    public void UpgradeAbility4()
    {
        HealUpgrade += 1.0f;
    }

    // Multishot
    public void UpgradeAbility5()
    {
        DamageUpgrade += 0.2f;
    }

    // Piercing
    public void UpgradeAbility6()
    {
        DamageUpgrade += 0.1f;
    }

    // PoisonArea
    public void UpgradeAbility7()
    {
        AOERangeUpgrade += 0.1f;
        SpeedReductionUpgrade += 0.1f;
    }
}
