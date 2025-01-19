using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability")]
public class Abilities : ScriptableObject
{
    public string AbilityName;
    public int AbilityID;
    public int AbilityCost;
    public int AbilityCooldown;
    public float AbilityTime;
    public ProjectileObj projectileObj;

}


