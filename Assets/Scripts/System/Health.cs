using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public RectTransform HealthBar;
    public float HealthBarOffset = 1f;

    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
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

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        if (HealthBar == null) return;
        if (currentHealth == maxHealth)
        {
            HealthBar.sizeDelta = new Vector2(0, HealthBar.sizeDelta.y);
        }
        else
        {
            HealthBar.sizeDelta = new Vector2(2 * ((float)currentHealth / maxHealth), HealthBar.sizeDelta.y);
        }

        var healthBarRotation = HealthBar.rotation;
        healthBarRotation.SetLookRotation(transform.forward * -1);
        HealthBar.rotation = healthBarRotation;

        var healthBarPosition = HealthBar.position;
        healthBarPosition.x = transform.position.x;
        healthBarPosition.y = transform.position.y + HealthBarOffset;
        healthBarPosition.z = transform.position.z;
        HealthBar.position = healthBarPosition;
    }

    public void TakeDamage(int damage)
    {
        if (damage == 0) return;

        Value -= damage;

        if (Value <= 0)
        {
            Destroy(gameObject);
        }

    }
}
