
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class AbilityMenuController : MonoBehaviour
{
    [SerializeField] private Abilities startAbility;
    [SerializeField] private int maxAbilities;
    private int newAbilityIndex;
    public GameObject cantClickLayer;

    [Space]
    [SerializeField] private GameObject abilityGridPrefab;
    private GameObject healAbiltiesGrid;
    private GameObject tier1AbiltiesGrid;
    private GameObject tier2AbiltiesGrid;
    //private GameObject tier3AbiltiesGrid;

    private List<AbilityMenuEntry> allEntries = new List<AbilityMenuEntry>();

    [SerializeField] private Abilities[] healAbilities;
    [SerializeField] private Abilities[] tier1Abilities;
    [SerializeField] private Abilities[] tier2Abilities;
    //[SerializeField] private Abilities[] tier3Abilities;

    public float ressourceCostMultipler;
    public RessourceCosts[] upgradeCosts;

    private void Start()
    {
        healAbiltiesGrid = transform.GetChild(0).GetChild(0).gameObject;
        tier1AbiltiesGrid = transform.GetChild(0).GetChild(1).gameObject;
        tier2AbiltiesGrid = transform.GetChild(0).GetChild(2).gameObject;
        //tier3AbiltiesGrid = transform.GetChild(0).GetChild(3).gameObject;

        for (int i = 0; i < healAbilities.Length; i++)
        {
            CreatePrefab(healAbiltiesGrid, healAbilities[i]);
        }

        for (int r = 0; r < tier1Abilities.Length; r++)
        {
            CreatePrefab(tier1AbiltiesGrid, tier1Abilities[r]);
        }

        for (int t = 0; t < tier2Abilities.Length; t++)
        {
            CreatePrefab(tier2AbiltiesGrid, tier2Abilities[t]);
        }
        //for (int u = 0; u < tier3Abilities.Length; u++)
        //{
        //    CreatePrefab(tier3AbiltiesGrid, tier3Abilities[u]);
        //}
    }
    private void CreatePrefab(GameObject grid, Abilities ability)
    {
        GameObject prefab = Instantiate(abilityGridPrefab, grid.transform);

        prefab.GetComponent<TooltipWindow>().ability = ability;

        AbilityMenuEntry abilityMenuEntry = prefab.GetComponent<AbilityMenuEntry>();
        abilityMenuEntry.ability = ability;
        abilityMenuEntry.abilityMenuController = this;
        abilityMenuEntry.abilitySlot = -1;

        if (ability == startAbility)
        {
            abilityMenuEntry.abilitySlot = newAbilityIndex;
            abilityMenuEntry.gotAbility = true;
            abilityMenuEntry.currentAbilityLvl++;
            newAbilityIndex++;
        }

        abilityMenuEntry.PrefabUpdate();
        abilityMenuEntry.CostsUpdate();

        allEntries.Add(abilityMenuEntry);
    }
    public void BuyUpgradeAbility(AbilityMenuEntry abilityMenuEntry)
    {
        if (abilityMenuEntry.ability == null) return;

        ActivateCantClickLayer();
        if (abilityMenuEntry.gotAbility == false)
        {
            if (newAbilityIndex >= maxAbilities)
            {
                Debug.Log("Got 8 Abilties");
                return;
            }
            //Buy
            if (PurchaseAbility(abilityMenuEntry.currentAbilityLvl, (int)abilityMenuEntry.ability.abilityTier))
            {
                Player.Instance.abilities.Add(abilityMenuEntry.ability);
                abilityMenuEntry.abilitySlot = newAbilityIndex;
                abilityMenuEntry.gotAbility = true;
                abilityMenuEntry.currentAbilityLvl++;
                abilityMenuEntry.PrefabUpdate();
                PlayerUI.Instance.cooldownController.ActivateCooldownObj(Player.Instance.abilities.Count - 1);

                newAbilityIndex++;

                CostsUpdate();
            }
        }
        else
        {
            //Upgrade
            if (PurchaseAbility(abilityMenuEntry.currentAbilityLvl, (int)abilityMenuEntry.ability.abilityTier))
            {
                AbilityController abilityController = Player.Instance.abilityController;
                abilityController.slotUpgrades[abilityMenuEntry.abilitySlot].slotDamage += abilityMenuEntry.ability.damageUpgradeValue;
                abilityController.slotUpgrades[abilityMenuEntry.abilitySlot].slotArea += abilityMenuEntry.ability.areaUpgradeValue;
                abilityController.slotUpgrades[abilityMenuEntry.abilitySlot].slotHeal += abilityMenuEntry.ability.healUpgradeValue;
                abilityController.slotUpgrades[abilityMenuEntry.abilitySlot].slotLifesteal += abilityMenuEntry.ability.lifestealUpgradeValue;

                abilityMenuEntry.currentAbilityLvl++;
                abilityMenuEntry.PrefabUpdate();

                CostsUpdate();
            }
        }
        
        if(newAbilityIndex >= maxAbilities)
        {
            for (int i = 0; i < allEntries.Count; i++)
            {
                if (allEntries[i].abilitySlot == -1) allEntries[i].gameObject.SetActive(false);
            }
        } 

    }
    public bool PurchaseAbility(int abilityLevel, int tier)
    {
        int woodCosts = upgradeCosts[abilityLevel].wood;
        if(tier > 0) woodCosts = Mathf.RoundToInt(woodCosts * (ressourceCostMultipler * (tier + 1)));

        int copperCosts = upgradeCosts[abilityLevel].copper;
        if(tier > 0) copperCosts = Mathf.RoundToInt(copperCosts * (ressourceCostMultipler * (tier + 1)));

        int goldCosts = upgradeCosts[abilityLevel].gold;
        if (tier > 0) goldCosts = Mathf.RoundToInt(goldCosts * (ressourceCostMultipler * (tier + 1)));

        if (Player.Instance.SubtractResources(new() {
                    { ResourceType.Wood, woodCosts},
                    { ResourceType.Copper, copperCosts},
                    { ResourceType.Gold, goldCosts}
            })
        )
        {
            return true;
        }
        return false;
    }

    private void CostsUpdate()
    {
        for (int i = 0; i < allEntries.Count; i++)
        {
            if (allEntries[i].gameObject.activeSelf == true) allEntries[i].CostsUpdate();
        }
    }
    private void ActivateCantClickLayer()
    {
        cantClickLayer.SetActive(true);
    }
    [Serializable]
    public struct RessourceCosts
    {
        public int wood;
        public int copper;
        public int gold;
    }
}
