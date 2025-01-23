using UnityEngine;
using TMPro;
using System;
using UpgradeSystem;

public class UpgradeSlot : MonoBehaviour
{
    private UpgradeController upgradeController;

    [TextArea][SerializeField] private string upgradeString;
    [TextArea][SerializeField] private string upgradeStringAfterFirstValue;
    [TextArea][SerializeField] private string upgradeStringAfterSecondValue;

    private TextMeshProUGUI upgradeText;

    [NonSerialized] public int currentLevel;
    public int maxLevel;

    public float upgradeValue;
    public Upgrades.UpgradeType upgradeType;

    public float secondUpgradeValue;
    public Upgrades.UpgradeType secondUpgradeType;


    private void Awake()
    {
        upgradeText = GetComponentInChildren<TextMeshProUGUI>();
        upgradeController = GetComponentInParent<UpgradeController>();

        TextUpdate();
    }

    private void OnEnable()
    {
        TextUpdate();
    }
    public void TextUpdate()
    {
        upgradeText.text = string.Empty;
        upgradeText.text = upgradeString;

        if (upgradeValue != 0) upgradeText.text += " <color=green>" + upgradeValue + "</color>";

        upgradeText.text += upgradeStringAfterFirstValue;

        if (secondUpgradeValue != 0)
        {
            upgradeText.text += " <color=green>" + secondUpgradeValue + "</color>";
            upgradeText.text += upgradeStringAfterSecondValue;
        }
    }

    public void AddUpgrade()
    {
        currentLevel++;
        Upgrades.IncreaseValue(upgradeType, Mathf.RoundToInt(upgradeValue));
        if(secondUpgradeType != Upgrades.UpgradeType.Empty) Upgrades.IncreaseValue(secondUpgradeType, Mathf.RoundToInt(secondUpgradeValue));


        switch (upgradeType)
        {
            case Upgrades.UpgradeType.Health:
                Player.Instance.health.HealthUpgrade();
                break;
            case Upgrades.UpgradeType.MaxEnergy:
                Player.Instance.MaxEnergy += Mathf.RoundToInt(upgradeValue);
                break;
            case Upgrades.UpgradeType.EnergyInterval:
                Player.Instance.energyRestoreInterval += Mathf.RoundToInt(upgradeValue);
                break;
        }

        switch (secondUpgradeType)
        {
            case Upgrades.UpgradeType.Health:
                Player.Instance.health.HealthUpgrade();
                break;
            case Upgrades.UpgradeType.MaxEnergy:
                Player.Instance.MaxEnergy += Mathf.RoundToInt(upgradeValue);
                break;
            case Upgrades.UpgradeType.EnergyInterval:
                Player.Instance.energyRestoreInterval += Mathf.RoundToInt(upgradeValue);
                break;
        }

        if (currentLevel >= maxLevel)
        {
            upgradeController.RemoveUpgrade(this);
        }
        else upgradeController.CloseController();
    }
}
