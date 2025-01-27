using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;
using static Abilities;

public class AbilityMenuEntry : MonoBehaviour
{
    [NonSerialized] public AbilityMenuController abilityMenuController;

    public Button buyButton;
    public TextMeshProUGUI abilityNameText;
    public TextMeshProUGUI woodCosts;
    public TextMeshProUGUI copperCosts;
    public TextMeshProUGUI goldCosts;
    public Abilities ability;

    public bool gotAbility;
    public int abilitySlot;
    public int currentAbilityLvl;

    private void OnEnable()
    {
        CostsUpdate();
    }
    public void PrefabUpdate()
    {
        if (currentAbilityLvl >= ability.maxLevel)
        { 
            gameObject.SetActive(false);
            return;
        }

        if(gotAbility == false)
        {
            abilityNameText.text = "Buy: <color=green>" + ability.AbilityName + "</color>";
        }
        else
        {
            abilityNameText.text = "Upgrade: <color=yellow>" + ability.AbilityName + " " + (currentAbilityLvl + 1) + "</color>";
        }
    }
    public void CostsUpdate()
    {
        if (currentAbilityLvl >= ability.maxLevel)
        {
            gameObject.SetActive(false);
            return;
        }

        if (abilityMenuController.upgradeCosts[currentAbilityLvl].wood == 0) woodCosts.gameObject.SetActive(false);
        else
        {
            int costs = abilityMenuController.upgradeCosts[currentAbilityLvl].wood;
            if ((int)ability.abilityTier > 0) costs = Mathf.RoundToInt(costs * (abilityMenuController.ressourceCostMultipler * ((int)ability.abilityTier + 1)));
            if (costs <= Player.Instance.Wood) woodCosts.text = "<color=green>" + costs.ToString() + "</color>";
            else woodCosts.text = "<color=red>" + costs.ToString() + "</color>";
            woodCosts.gameObject.SetActive(true);
        }

        if (abilityMenuController.upgradeCosts[currentAbilityLvl].copper == 0) copperCosts.gameObject.SetActive(false);
        else
        {
            int costs = abilityMenuController.upgradeCosts[currentAbilityLvl].copper;
            if ((int)ability.abilityTier > 0) costs = Mathf.RoundToInt(costs * (abilityMenuController.ressourceCostMultipler * ((int)ability.abilityTier + 1)));
            if (costs <= Player.Instance.Copper) copperCosts.text = "<color=green>" + costs.ToString() + "</color>";
            else copperCosts.text = "<color=red>" + costs.ToString() + "</color>";
            copperCosts.gameObject.SetActive(true);
        }

        if (abilityMenuController.upgradeCosts[currentAbilityLvl].gold == 0) goldCosts.gameObject.SetActive(false);
        else
        {
            int costs = abilityMenuController.upgradeCosts[currentAbilityLvl].gold;
            if ((int)ability.abilityTier > 0) costs = Mathf.RoundToInt(costs * (abilityMenuController.ressourceCostMultipler * ((int)ability.abilityTier + 1)));
            if (costs <= Player.Instance.Gold) goldCosts.text = "<color=green>" + costs.ToString() + "</color>";
            else goldCosts.text = "<color=red>" + costs.ToString() + "</color>";
            goldCosts.gameObject.SetActive(true);
        }
    }
    public void BuyUpgradeButton()
    {
        abilityMenuController.BuyUpgradeAbility(this);
    }
}
