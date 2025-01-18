using Unity.VisualScripting;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private Controls controls;
    private Abilities currentAbility;
    private AbilityState state;
    private enum AbilityState{
        WaitForAbility,
        PrepareAbility,
        ExecuteAbility,
    }

    void Awake()
    {
        controls = new Controls();
    }
    void Update()
    {
        if(controls.Player.ConfirmAbility.WasPerformedThisFrame()){
            if(state == AbilityState.PrepareAbility){
                CastAbility();

            }
        }
        if(controls.Player.CancelAbility.WasPerformedThisFrame()){
            if(state == AbilityState.PrepareAbility){

            }     
        }
    }


    public void CheckForAbility(Abilities ability){
        if(Player.Instance.CurrentEnergy < ability.AbilityCost) return;
        if(state == AbilityState.ExecuteAbility) return;

        currentAbility = ability;
        StartAbility();

    }

    private void StartAbility(){

        state = AbilityState.PrepareAbility;
    }
    private void CastAbility(){

    }
}
