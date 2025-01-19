using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityController : MonoBehaviour
{
    private Controls controls;
    private Camera cam;

    private Abilities currentAbility;
    private AbilityState state;
    private float abilityTimer;
    private enum AbilityState{
        WaitForAbility,
        PrepareAbility,
        ExecuteAbility,
    }

    void Awake()
    {
        controls = new Controls();
        cam = Camera.main;
    }
    void Update()
    {
        if(state == AbilityState.ExecuteAbility){

        abilityTimer += Time.deltaTime;

        if(abilityTimer > currentAbility.AbilityTime){
            state = AbilityState.WaitForAbility;
        }
        }
    }


    public void CheckForAbility(Abilities ability){
        if(Player.Instance.CurrentEnergy < ability.AbilityCost) return;
        if(state == AbilityState.ExecuteAbility) return;

        currentAbility = ability;
        abilityTimer = 0;

        state = AbilityState.ExecuteAbility;
        CastAbility();
    }
    private void CastAbility(){
        GameObject bullet = Instantiate(currentAbility.projectileObj.prefab, transform.position, Quaternion.identity);
        if(bullet.TryGetComponent(out Projectile projectile)){
            Vector3 mousePosi = cam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, cam.nearClipPlane));
            Vector2 direction = ((Vector2)mousePosi - (Vector2)transform.position).normalized;

            bullet.transform.right = direction;

            projectile.SetProjectile(currentAbility, direction);
        }

    }
}
