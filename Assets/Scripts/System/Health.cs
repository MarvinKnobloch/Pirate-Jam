using UnityEngine;
using System;
using UnityEngine.Events;
using UpgradeSystem;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    public RectTransform HealthBar;
    [NonSerialized] public Image HealthBarImage;
    public bool HealthBarPermanent = false;
    public float HealthBarOffset = 1f;

    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    [HideInInspector]
    public UnityEvent dieEvent;

    public Upgrades.UpgradeValues healthUpgrade;
    public int Value
    {
        get { return currentHealth; }
        set { currentHealth = Math.Min(Math.Max(0, value), maxHealth); }
    }

    public int MaxValue
    {
        get { return maxHealth; }
        set { maxHealth = Math.Max(0, value); currentHealth = Math.Min(value, currentHealth); }
    }

    void Start()
    {
        if (HealthBar != null) HealthBarImage = HealthBar.GetComponent<Image>();
        HealthUpgrade();
        currentHealth = maxHealth;

        if(gameObject == Player.Instance.gameObject) Player.Instance.HealthUIUpdate();

        EnemyHealthbarUpdate();
    }

    void FixedUpdate()
    {
        if (HealthBar == null) return;
        var healthBarRotation = HealthBar.rotation;
        healthBarRotation.SetLookRotation(transform.forward * -1);
        HealthBar.rotation = healthBarRotation;

        var healthBarPosition = HealthBar.position;
        healthBarPosition.x = transform.position.x;
        healthBarPosition.y = transform.position.y + HealthBarOffset;
        healthBarPosition.z = transform.position.z;
        HealthBar.position = healthBarPosition;
    }

    public void TakeDamage(int amount)
    {
        if (amount == 0) return;

        Value -= amount;

        if (gameObject == Player.Instance.gameObject) Player.Instance.HealthUIUpdate();
        else
        {
            EnemyHealthbarUpdate();
        }

        if (Value <= 0)
        {
            StopAllCoroutines();
            dieEvent?.Invoke();
            Destroy(gameObject);
        }
    }
    public void Heal(int amount)
    {
        if (amount == 0) return;

        Value += amount;
        //Debug.Log(amount);

        if (gameObject == Player.Instance.gameObject) Player.Instance.HealthUIUpdate();
        else
        {
            EnemyHealthbarUpdate();
        }
    }
    public void DamageOverTime(int amount, int maxTicks, float tickInterval)
    {
        StartCoroutine(StartDamageOverTime(amount, maxTicks, tickInterval));
    }
    IEnumerator StartDamageOverTime(int amount,int maxTicks, float tickInterval)
    {
        int currentticks = 0;
        while(currentticks < maxTicks)
        {
            currentticks++;
            TakeDamage(amount);
            yield return new WaitForSeconds(tickInterval);
        }
    }
    private void EnemyHealthbarUpdate()
    {
        if (HealthBar != null)
        {
            if (!HealthBarPermanent && currentHealth == maxHealth)
            {
                HealthBar.sizeDelta = new Vector2(0, HealthBar.sizeDelta.y);
            }
            else
            {
                HealthBar.sizeDelta = new Vector2(2 * ((float)currentHealth / maxHealth), HealthBar.sizeDelta.y);
            }
        }
    }
    public void HealthUpgrade()
    {
        if (healthUpgrade.type == Upgrades.UpgradeType.Empty) return;

        if (healthUpgrade.type == Upgrades.UpgradeType.PlayerHealth)
        {
            MaxValue += Mathf.RoundToInt(Upgrades.Instance.GetUpgradeStat(healthUpgrade.type) * (healthUpgrade.percentage * 0.01f));
            if (Player.Instance != null) Player.Instance.HealthUIUpdate();
        }
        else if (healthUpgrade.type == Upgrades.UpgradeType.MinionHealth)
        {
            MaxValue += Mathf.RoundToInt(Upgrades.Instance.GetUpgradeStat(healthUpgrade.type) * (healthUpgrade.percentage * 0.01f));
        }
    }
}
