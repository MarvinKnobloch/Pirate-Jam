using UnityEngine;
using System;
using Unity.VisualScripting;
using static UpgradeSystem.Upgrades;

public class Player : MonoBehaviour
{
    public static Player Instance;


    [NonSerialized] public Controls controls;
    private AbilityController abilityController;

    [Header("Energy")]
    [SerializeField] private int currentEnergy;
    [SerializeField] private int maxEnergy;
    public float energyRestoreInterval;
    [SerializeField] private int energyRestoreAmount;

    public Abilities[] abilities;

    [NonSerialized] public Health health;

    [Header("Resources")]
    public int Iron = 0;
    public int Copper = 0;
    public int Wood = 0;

    public int CurrentEnergy
    {
        get { return currentEnergy; }
        set { currentEnergy = Math.Min(Math.Max(0, value), maxEnergy); }
    }
    public int MaxEnergy
    {
        get { return maxEnergy; }
        set { maxEnergy = Math.Max(0, value); currentEnergy = Math.Min(value, currentEnergy); }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        controls = Keybindinputmanager.Controls;
        health = GetComponent<Health>();
        abilityController = GetComponent<AbilityController>();
    }
    void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    void Start()
    {
        HealthUIUpdate();
        EnergyUpdate(MaxEnergy);
        AddResource(ResourceType.Iron, 0);
        InvokeRepeating("EnergyRestoreTick", energyRestoreInterval, energyRestoreInterval);
    }

    void Update()
    {
        if (controls.Player.Ability1.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[0], 0);
        }
        if (controls.Player.Ability2.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[1], 1);
        }
        if (controls.Player.Ability3.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[2], 2);
        }
        if (controls.Player.Ability4.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[3], 3);
        }
        if (controls.Player.Ability5.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[4], 4);
        }
        if (controls.Player.Ability6.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[5], 5);
        }
        if (controls.Player.Ability7.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[6], 6);
        }
        if (controls.Player.Ability8.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[7], 7);
        }
    }

    public void HealthUIUpdate()
    {
        if (PlayerUI.Instance != null) PlayerUI.Instance.HealthUIUpdate(health.Value, health.MaxValue);
    }

    public void EnergyUpdate(int energyChange)
    {
        CurrentEnergy += energyChange;
        if (PlayerUI.Instance != null) PlayerUI.Instance.EnergyUIUpdate(CurrentEnergy, MaxEnergy);
    }
    private void EnergyRestoreTick()
    {
        EnergyUpdate(energyRestoreAmount);
    }
    public void ChangeMaxEnergy(int value)
    {
        Instance.MaxEnergy += Mathf.RoundToInt(value);
        if (PlayerUI.Instance != null) PlayerUI.Instance.EnergyUIUpdate(CurrentEnergy, MaxEnergy);
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Iron:
                Iron += amount;
                break;
            case ResourceType.Copper:
                Copper += amount;
                break;
            case ResourceType.Wood:
                Wood += amount;
                break;
        }

        if (PlayerUI.Instance != null) PlayerUI.Instance.ResourceUIUpdate(Iron, Copper, Wood);
    }

}
