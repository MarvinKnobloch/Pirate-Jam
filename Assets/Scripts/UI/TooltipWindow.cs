using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
        text.text += Scaling();
    }
    private string Name()
    {
        return "<b><u>" + ability.name + "</u></b>\n";
    }
    private string Discription()
    {
        return ability.Description +"\n\n";
    }
    private string Damage()
    {

        if (ability.projectileObj.damage != 0)
        {
            string text;
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
    private string Scaling()
    {
        return string.Empty;
    }
}
