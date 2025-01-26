using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UpgradeSystem;

public class TooltipWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Abilities ability;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerUI.Instance.tooltipController.WindowEnable();
        if (ability != null)
        {
            SetText();
            PlayerUI.Instance.tooltipController.SetWindowPosition();
        }
        else PlayerUI.Instance.tooltipController.HideTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerUI.Instance.tooltipController.HideTooltip();
    }
    private void SetText()
    {
        TextMeshProUGUI text = PlayerUI.Instance.tooltipController.tooltipText;
        text.text += Name();
        text.text += Discription();
        text.text += Damage();
        text.text += Size(ability.projectileObj.aoeSizeScaling);
        text.text += Utility(ability.projectileObj.slowDuration, ability.projectileObj.slowStrength, ability.projectileObj.stunDuration);
        text.text += Healing(ability.projectileObj.healAmount, ability.projectileObj.lifeStealAmount, ability.projectileObj.healScaling, ability.projectileObj.lifeStealScaling);

        if(ability.projectileObj.areaPrefab != null) text.text += Area();

        PlayerUI.Instance.tooltipController.energyText.text = ability.AbilityCost.ToString();
        PlayerUI.Instance.tooltipController.cooldownText.text = ability.AbilityCooldown.ToString();
    }
    private string Name()
    {
        return "<b><u>" + ability.AbilityName + "</u></b>\n\n";
    }
    private string Discription()
    {
        return ability.Description +"\n\n";
    }
    private string Damage()
    {
        if (ability.projectileObj.damage != 0)
        {
            string text = string.Empty;
            if (ability.projectileObj.damageType == Upgrades.UpgradeType.Damage) text = "Damage Type: Normal\n";
            else text = "Damage Type: Damage over time\n";
            text += "Damage: <color=green>" + Upgrades.Instance.DamageUpgradeCalculation(ability.projectileObj.damage, ability.projectileObj.damageType, ability.projectileObj.damageScaling) + 
                     "</color> (<color=yellow>" + ability.projectileObj.damageScaling + "</color>%)\n";
            return text;
        }
        else return "Damage: <color=green>" + 0 + "</color>\n";
    }
    private string Size(float scaling)
    {
        string text = string.Empty;
        if (scaling != 0) text += "Area Scaling: (<color=yellow>" + scaling + "</color>%)\n";

        return text;
    }
    private string Utility(float slowDuration, float slowStrength, float stunDuration)
    {
        string text = string.Empty;
        if (slowDuration > 0)
        {
            slowStrength -= Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.Slow) * 0.01f;
            text += "Slow: <color=green>" + (1 - slowStrength) * 100 + "</color>% Duration: <color=green>" + slowDuration + "</color> seconds\n"; 
        }
        if (stunDuration > 0)
        {
            stunDuration = Upgrades.Instance.StunCalculation(stunDuration);
            text += "Stun: <color=green>" + stunDuration + "</color> seconds\n";
        }

        return text;
    }
    private string Healing(int healAmount, int lifestealAmount, float healScaling, float lifestealScaling)
    {
        string text = string.Empty;
        if (healAmount > 0)
        {
            healAmount = Upgrades.Instance.HealCalculation(healAmount, healScaling);
            text += "Minion Heal: <color=green>" + healAmount + "</color> (<color=yellow>" + healScaling + "</color>%)\n";
        }
        if (lifestealAmount > 0)
        {
            lifestealAmount = Upgrades.Instance.LifeStealCalculation(lifestealAmount, lifestealScaling);
            text += "Lifesteal: <color=green>" + lifestealAmount + "</color> (<color=yellow>" + lifestealScaling + "</color>%)\n"; 
        }

        return text;
    }
    private string Area()
    {
        AreaAbility areaAbility = ability.projectileObj.areaPrefab.GetComponent<AreaAbility>();
        string text = string.Empty;
        text = "\nArea: Damage over time\n";
        text += "Damage: <color=green>" + Upgrades.Instance.DamageUpgradeCalculation(areaAbility.damageAmount, areaAbility.damageType, areaAbility.damageScaling) + "</color> " +
                "(<color=yellow>" + areaAbility.damageScaling + "</color>%)\n";
        text += "Damage interval: <color=green>" + areaAbility.tickInterval + "</color> seconds\n";
        text += "Lifetime: <color=green>" + areaAbility.lifeTime + "</color> seconds\n";
        text += Size(areaAbility.aoeSizeScaling);
        text += Utility(areaAbility.slowDuration, areaAbility.slowStrength, areaAbility.stunDuration);
        text += Healing(areaAbility.healAmount, areaAbility.lifeStealAmount, areaAbility.healScaling, areaAbility.lifeStealScaling);

        return text;

    }
}
