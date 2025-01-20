using UnityEngine;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public static Player Instance;


    [NonSerialized] public Controls controls;
    private AbilityController abilityController;

    [SerializeField] private int currentEnergy;
    [SerializeField] private int maxEnergy;

    public Abilities[] abilities;

    [NonSerialized] public Health Health;

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

        controls = new Controls();//Keybindinputmanager.inputActions;
        Health = GetComponent<Health>();

    }
    void OnEnable(){
        controls.Enable();
    }
    void Start()
    {
        abilityController = GetComponent<AbilityController>();
        CurrentEnergy = MaxEnergy;
    }

    void Update()
    {
        if(controls.Player.Ability1.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[0]);
        }
        if (controls.Player.Ability2.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[1]);
        }
        if (controls.Player.Ability3.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[2]);
        }
        if (controls.Player.Ability4.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[3]);
        }
        if (controls.Player.Ability5.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[4]);
        }
        if (controls.Player.Ability6.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[5]);
        }
        if (controls.Player.Ability7.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[6]);
        }
        if (controls.Player.Ability8.WasPerformedThisFrame())
        {
            abilityController.CheckForAbility(abilities[7]);
        }
    }
}
