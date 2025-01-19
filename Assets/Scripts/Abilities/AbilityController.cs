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

    [SerializeField] private GameObject testobj;
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
    private void CastAbility()
    {
        switch (currentAbility.projectileObj.projectileType)
        {
            case ProjectileType.single:
                ShootSingleBullet();
                break;
            case ProjectileType.aoe:
                ShootAOEBullet();
                break;
        }


    }
    private void ShootSingleBullet()
    {
        GameObject bullet = Instantiate(currentAbility.projectileObj.prefab, transform.position, Quaternion.identity);
        if (bullet.TryGetComponent(out Projectile projectile))
        {
            Vector3 mousePosi = cam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -cam.transform.position.z));
            mousePosi.z = 0;

            Vector2 direction = ((Vector2)mousePosi - (Vector2)transform.position).normalized;

            bullet.transform.right = direction;

            projectile.SetProjectileSingle(currentAbility, direction);
        }
    }
    private void ShootAOEBullet()
    {
        GameObject bullet = Instantiate(currentAbility.projectileObj.prefab, transform.position, Quaternion.identity);
        {
            if (bullet.TryGetComponent(out Projectile projectile))
            {
                Vector3 mousePosi = cam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -cam.transform.position.z));
                mousePosi.z = 0;

                float dist = Vector2.Distance(mousePosi,transform.position);

                projectile.SetProjectileAOE(currentAbility,transform.position, mousePosi);
            }
        }
    }
}
