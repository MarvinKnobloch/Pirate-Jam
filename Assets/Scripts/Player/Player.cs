using UnityEngine;
using UnityEngine.Android;
using System;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [NonSerialized] public Controls controls;
    private AbilityController abilityController;

    [SerializeField]private int currentEnergy;
    [SerializeField] private int maxEnergy;

    public Abilities[] abilities;


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
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }

        controls = new Controls();//Keybindinputmanager.inputActions;
    }
    void Start()
    {
        abilityController = GetComponent<AbilityController>();
    }

    void Update()
    {
        if(controls.Player.Ability1.WasPerformedThisFrame()){
            abilityController.CheckForAbility(abilities[0]);

        }
    }
}
