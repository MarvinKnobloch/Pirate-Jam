using UnityEngine;
using System;

public class Health : MonoBehaviour
{
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

    void Awake(){
        if(currentHealth <= 0){     
            currentHealth = maxHealth;
        }
    }

	public void TakeDamage(int damage){
        if(damage == 0) return;

        Value -= damage;

        if(Value <= 0){
            //Die
        }

    }
}
