using UnityEngine;
using System;
using Unity.VisualScripting;
using static UpgradeSystem.Upgrades;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player Instance;


    [NonSerialized] public Controls controls;
    [NonSerialized] public AbilityController abilityController;
    public Transform attackTransform;

    [Header("Energy")]
    [SerializeField] private int currentEnergy;
    [SerializeField] private int maxEnergy;
    public float energyRestoreInterval;
    [SerializeField] private int energyRestoreAmount;
    //Overload
    [NonSerialized] public bool overloadonCooldown;
    [NonSerialized] public bool overloadActive;
    [SerializeField] private int overloadDuration;
    [SerializeField] private int overloadCooldown;

    public List<Abilities> abilities;

    [NonSerialized] public Health health;

    [Header("Resources")]
    public int Wood = 0;
    public int Copper = 0;
    public int Gold = 0;

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
        AddResource(ResourceType.Wood, 0);
        InvokeRepeating("EnergyRestoreTick", energyRestoreInterval, energyRestoreInterval);

        if (health != null) health.dieEvent.AddListener(OnDeath);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        var ability1 = abilities.ElementAtOrDefault(0);
        var ability2 = abilities.ElementAtOrDefault(1);
        var ability3 = abilities.ElementAtOrDefault(2);
        var ability4 = abilities.ElementAtOrDefault(3);
        var ability5 = abilities.ElementAtOrDefault(4);
        var ability6 = abilities.ElementAtOrDefault(5);
        var ability7 = abilities.ElementAtOrDefault(6);
        var ability8 = abilities.ElementAtOrDefault(7);

        if (ability1 != null && controls.Player.Ability1.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(ability1, 0);
        }
        if (ability2 != null && controls.Player.Ability2.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(ability2, 1);
        }
        if (ability3 != null && controls.Player.Ability3.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(ability3, 2);
        }
        if (ability4 != null && controls.Player.Ability4.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(ability4, 3);
        }
        if (ability5 != null && controls.Player.Ability5.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(ability5, 4);
        }
        if (ability6 != null && controls.Player.Ability6.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(ability6, 5);
        }
        if (ability7 != null && controls.Player.Ability7.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(ability7, 6);
        }
        if (ability8 != null && controls.Player.Ability8.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(ability8, 7);
        }
        if (controls.Player.Overload.WasPerformedThisFrame())
        {
            OverloadStart();
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
    public void OverloadStart()
    {
        if(overloadonCooldown == false)
        {
            overloadonCooldown = true;
            overloadActive = true;
            PlayerUI.Instance.cooldownController.OverloadCooldownStart(overloadCooldown);
            StartCoroutine(OverloadEnd());
        }
    }
    IEnumerator OverloadEnd()
    {
        yield return new WaitForSeconds(overloadDuration);
        overloadActive = false;
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:
                Wood += amount;
                break;
            case ResourceType.Copper:
                Copper += amount;
                break;
            case ResourceType.Gold:
                Gold += amount;
                break;
        }

        if (PlayerUI.Instance != null) PlayerUI.Instance.ResourceUIUpdate(Wood, Copper, Gold);
    }

    public bool SubtractResources(Dictionary<ResourceType, int> resources)
    {
        foreach (var resource in resources)
        {
            switch (resource.Key)
            {
                case ResourceType.Wood:
                    if (Wood < resource.Value) return false;
                    break;
                case ResourceType.Copper:
                    if (Copper < resource.Value) return false;
                    break;
                case ResourceType.Gold:
                    if (Gold < resource.Value) return false;
                    break;
            }
        }

        foreach (var resource in resources)
        {
            switch (resource.Key)
            {
                case ResourceType.Wood:
                    Wood -= resource.Value;
                    break;
                case ResourceType.Copper:
                    Copper -= resource.Value;
                    break;
                case ResourceType.Gold:
                    Gold -= resource.Value;
                    break;
            }
        }

        if (PlayerUI.Instance != null) PlayerUI.Instance.ResourceUIUpdate(Wood, Copper, Gold);
        return true;
    }

    private void OnDeath()
    {
        StopAllCoroutines();
        PlayerUI.Instance.gameOverObj.SetActive(true);
    }

}
