
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UpgradeSystem;

public class AbilityMenuController : MonoBehaviour
{
    private Controls controls;
    [SerializeField] private Abilities startAbility;
    [SerializeField] private int maxAbilities;
    private int newAbilityIndex;
    public GameObject cantClickLayer;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI currentWood;
    [SerializeField] private TextMeshProUGUI currentCopper;
    [SerializeField] private TextMeshProUGUI currentGold;

    [Space]
    [SerializeField] private GameObject abilityGridPrefab;
    private GameObject healAbiltiesGrid;
    private GameObject tier1AbiltiesGrid;
    private GameObject tier2AbiltiesGrid;

    private List<AbilityMenuEntry> allEntries = new List<AbilityMenuEntry>();

    [SerializeField] private Abilities[] healAbilities;
    [SerializeField] private Abilities[] tier1Abilities;
    [SerializeField] private Abilities[] tier2Abilities;

    public float ressourceCostMultipler;
    public RessourceCosts[] upgradeCosts;

    [Header("Player Abilities")]
    [SerializeField] private GameObject playerAbilityGrid;
    [SerializeField] private GameObject cooldownPrefab;
    [SerializeField] private int cooldownPrefabCount;

    private List<GameObject> playerAbilityObj = new List<GameObject>();
    private List<TextMeshProUGUI> playerAbilityHotkey = new List<TextMeshProUGUI>();

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {        
            gameObject.SetActive(false);
            return;
        }

        healAbiltiesGrid = transform.GetChild(0).GetChild(0).gameObject;
        tier1AbiltiesGrid = transform.GetChild(0).GetChild(1).gameObject;
        tier2AbiltiesGrid = transform.GetChild(0).GetChild(2).gameObject;

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

        controls = Keybindinputmanager.Controls;

        //PLayerAbilities
        for (int i = 0; i < cooldownPrefabCount; i++)
        {
            GameObject prefab = Instantiate(cooldownPrefab, playerAbilityGrid.transform);

            //Set ability Image  prefab.GetComponent<Image>() = Player.Instance.abilities
            playerAbilityObj.Add(prefab);
            playerAbilityHotkey.Add(prefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>());
            prefab.SetActive(false);
        }
        for (int i = 0; i < Player.Instance.abilities.Count; i++)
        {
            playerAbilityObj[i].SetActive(true);
            playerAbilityObj[i].GetComponent<TooltipWindow>().ability = Player.Instance.abilities[i];
            playerAbilityObj[i].GetComponent<TooltipWindow>().abilitySlot = i;

        }
        HotkeysUpdate();

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
    public void MenuEnable()
    {
        SetStats();
        CurrentRessourceUpdate();
    }
    private void SetStats()
    {
        statsText.text = string.Empty;
        statsText.text += "\n\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.PlayerHealth) + "\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.MinionHealth) + "\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.Damage) + "\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.DamageOverTime) + "\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.DamagePercentage) + "%\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.AoeSize) + "%\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.Slow) + "%\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.Stun) + "%\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.Heal) + "\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.LifeSteal) + "\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.GatherSpeed) + " seconds\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.BulletSpeed) + "%\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.ExpGain) + "%\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.Cooldown) + "%\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.MaxEnergy) + " seconds\n";
        statsText.text += "+" + Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.EnergyInterval) + " seconds\n";
    }
    private void CurrentRessourceUpdate()
    {
        currentWood.text = Player.Instance.Wood.ToString();
        currentCopper.text = Player.Instance.Copper.ToString();
        currentGold.text = Player.Instance.Gold.ToString();
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

                int slot = Player.Instance.abilities.Count - 1;
                PlayerUI.Instance.cooldownController.ActivateCooldownObj(slot);
                playerAbilityObj[slot].SetActive(true);
                playerAbilityObj[slot].GetComponent<TooltipWindow>().ability = Player.Instance.abilities[slot];
                playerAbilityObj[slot].GetComponent<TooltipWindow>().abilitySlot = slot;

                newAbilityIndex++;

                if (newAbilityIndex >= 4) tier2AbiltiesGrid.SetActive(true);

                CurrentRessourceUpdate();
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

                CurrentRessourceUpdate();
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
    public void HotkeysUpdate()
    {
        playerAbilityHotkey[0].text = controls.Player.Ability1.GetBindingDisplayString();
        playerAbilityHotkey[1].text = controls.Player.Ability2.GetBindingDisplayString();
        playerAbilityHotkey[2].text = controls.Player.Ability3.GetBindingDisplayString();
        playerAbilityHotkey[3].text = controls.Player.Ability4.GetBindingDisplayString();
        playerAbilityHotkey[4].text = controls.Player.Ability5.GetBindingDisplayString();
        playerAbilityHotkey[5].text = controls.Player.Ability6.GetBindingDisplayString();
        playerAbilityHotkey[6].text = controls.Player.Ability7.GetBindingDisplayString();
        playerAbilityHotkey[7].text = controls.Player.Ability8.GetBindingDisplayString();
    }
    [Serializable]
    public struct RessourceCosts
    {
        public int wood;
        public int copper;
        public int gold;
    }
}
