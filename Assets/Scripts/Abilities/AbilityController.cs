using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UpgradeSystem;
using static UpgradeSystem.Upgrades;

public class AbilityController : MonoBehaviour
{
    //private Controls controls;
    private Camera cam;

    private Abilities currentAbility;
    private int abilitySlot;
    private AbilityState state;
    private float abilityTimer;

    private Vector3 mousePosi;

    private CooldownController cooldownController;

    public SlotUpgrades[] slotUpgrades;
    private enum AbilityState
    {
        WaitForAbility,
        PrepareAbility,
        ExecuteAbility,
    }

    void Awake()
    {
        //controls = Keybindinputmanager.Controls;
        cam = Camera.main;
    }
    private void Start()
    {
        if (PlayerUI.Instance != null)
        {
            cooldownController = PlayerUI.Instance.cooldownController;
        }
    }
    void Update()
    {
        if (state == AbilityState.ExecuteAbility)
        {
            abilityTimer += Time.deltaTime;

            if (abilityTimer > currentAbility.AbilityTime)
            {
                state = AbilityState.WaitForAbility;
            }
        }
    }


    public void CheckForAbility(Abilities ability, int _abilitySlot)
    {
        if (Player.Instance.CurrentEnergy < ability.AbilityCost) return;
        if (state == AbilityState.ExecuteAbility) return;
        if (cooldownController != null) if (cooldownController.onCooldown[_abilitySlot]) return;

        currentAbility = ability;
        abilitySlot = _abilitySlot;
        abilityTimer = 0;

        if (cooldownController != null)
        {
            float cooldown = currentAbility.AbilityCooldown - (currentAbility.AbilityCooldown * Upgrades.Instance.GetUpgradeStat(UpgradeType.Cooldown) * 0.01f);
            if (cooldown <= 0) cooldown = 0;
            cooldownController.CooldownStart(_abilitySlot, cooldown);
        }
        Player.Instance.EnergyUpdate(-ability.AbilityCost);

        state = AbilityState.ExecuteAbility;
        CastAbility();
    }
    private void CastAbility()
    {
        mousePosi = cam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -cam.transform.position.z));
        mousePosi.z = 0;

        switch (currentAbility.projectileObj.projectileType)
        {
            case ProjectileType.single:
                ShootSingleBullet();
                break;
            case ProjectileType.aoe:
                ShootAoeBullet();
                break;
            case ProjectileType.explosion:
                ShootSingleBullet();
                break;
            case ProjectileType.piercing:
                ShootSingleBullet();
                break;
        }
    }
    private void ShootSingleBullet()
    {
        if (currentAbility.projectileObj.multipleProjectiles < 2)
        {
            Vector2 direction = ((Vector2)mousePosi - (Vector2)transform.position).normalized;
            CreateSingleBullet(direction);

            if (currentAbility.projectileObj.mirrorAttack)
            {
                CreateSingleBullet(direction * -1);
            }
        }
        else
        {
            MultipleBullets();
        }
    }
    private void CreateSingleBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(currentAbility.projectileObj.prefab, transform.position, Quaternion.identity);
        if (bullet.TryGetComponent(out Projectile projectile))
        {
            bullet.transform.right = direction;

            projectile.SetProjectileSingle(currentAbility, direction, abilitySlot);
        }
    }
    private void ShootAoeBullet()
    {
        CreateAoeBullet(mousePosi);

        if (currentAbility.projectileObj.mirrorAttack)
        {
            CreateAoeBullet(new Vector3(mousePosi.x * -1, mousePosi.y * -1, mousePosi.z));
        }
    }
    private void CreateAoeBullet(Vector3 mousePosi)
    {
        GameObject bullet = Instantiate(currentAbility.projectileObj.prefab, transform.position, Quaternion.identity);
        {
            if (bullet.TryGetComponent(out Projectile projectile))
            {
                float dist = Vector2.Distance(mousePosi, transform.position);

                projectile.SetProjectileAOE(currentAbility, transform.position, mousePosi, abilitySlot);
            }
        }
    }
    private void MultipleBullets()
    {
        int count = currentAbility.projectileObj.multipleProjectiles;
        float shotAngle = currentAbility.projectileObj.multiShotAngle;

        if (count % 2 == 0)
        {
            float startAngle = shotAngle * count * 0.5f - shotAngle * 0.5f;
            CreateMultiShotBullet(count, shotAngle, startAngle);
        }
        else
        {
            float startAngle = (count - 1) * 0.5f * shotAngle;
            CreateMultiShotBullet(count, shotAngle, startAngle);
        }
    }
    private void CreateMultiShotBullet(int count, float shotAngle, float startAngle)
    {
        Vector2 direction = ((Vector2)mousePosi - (Vector2)transform.position).normalized;

        for (int i = 0; i < count; i++)
        {
            GameObject bullet = Instantiate(currentAbility.projectileObj.prefab, transform.position, Quaternion.identity);

            if (bullet.TryGetComponent(out Projectile projectile))
            {
                bullet.transform.right = direction;
                bullet.transform.Rotate(0, 0, startAngle - i * shotAngle);
                //direction = bullet.transform.right;

                projectile.SetProjectileSingle(currentAbility, bullet.transform.right, abilitySlot);
            }
            if (currentAbility.projectileObj.mirrorAttack)
            {
                CreateSingleBullet(bullet.transform.right * -1);
            }
        }
    }
    [Serializable]
    public struct SlotUpgrades
    {
        public int slotDamage;
        public float slotArea;
        public int slotHeal;
        public int slotLifesteal;
    }
}
