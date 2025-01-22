using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability")]
public class Abilities : ScriptableObject
{
    public string AbilityName;
    public int AbilityID;
    public int AbilityCost;
    public float AbilityCooldown;
    public float AbilityTime;
    public ProjectileObj projectileObj;
}


