using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        text.text += Cooldown();
        text.text += Costs();
        text.text += Utility(ability.projectileObj.slowDuration, ability.projectileObj.slowStrength, ability.projectileObj.stunDuration);
        if(ability.projectileObj.areaPrefab != null) text.text += Area();
        text.text += Scaling();
    }
    private string Name()
    {
        return "<b><u>" + ability.AbilityName + "</u></b>\n";
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
            if (ability.projectileObj.damageType == UpgradeSystem.Upgrades.UpgradeType.Damage) text = "Damage Type: Normal\n";
            else text = "Damage Type: Damage over time\n";
            text += "Damage: <color=green>" + ability.projectileObj.damage + "</color>\n";
            return text;
        }
        else return "Damage: <color=green>" + 0 + "</color>\n";
    }
    private string Cooldown()
    {
        return "Cooldown: <color=green>" +ability.AbilityCooldown + "</color> Seconds\n";
    }
    private string Costs()
    {
        return "Costs: <color=green>" +ability.AbilityCost + "</color> Energy \n";
    }
    private string Utility(float slowDuration, float slowStrength, float stunDuration)
    {
        string text = string.Empty;
        if(slowDuration > 0) text += "Slow: <color=green>" + (1 - slowStrength) * 100 + "</color>% Duration: <color=green>" + slowDuration + "</color> seconds\n";
        if (stunDuration > 0) text += "Stun: <color=green>" + stunDuration + "</color> seconds\n";

        return text;
    }
    private string Area()
    {
        AreaAbility areaAbility = ability.projectileObj.areaPrefab.GetComponent<AreaAbility>();
        string text = string.Empty;
        text = "\nArea: Damage over time\n";
        text += "Damage: <color=green>" + areaAbility.damageAmount + "</color>\n";
        text += "Damage interval: <color=green>" + areaAbility.tickInterval + "</color> seconds\n";
        text += "Lifetime: <color=green>" + areaAbility.lifeTime + "</color> seconds\n";
        text += Utility(areaAbility.slowDuration, areaAbility.slowStrength, areaAbility.stunDuration);

        return text;

    }
    private string Scaling()
    {
        return string.Empty;
    }
}
