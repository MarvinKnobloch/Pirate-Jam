using System.Linq;
using UnityEngine;

public class AbilityMenuController : MonoBehaviour
{
    public Abilities[] AbilityMapping;

    public void UnlockAbility(GameObject caller)
    {
        if (!caller.TryGetComponent<AbilityMenuEntry>(out var abilityEntry))
        {
            Debug.Log("You fucked up mate. Add a AbilityMenuEntry component to the button");
            return;
        }

        var ability = AbilityMapping.ElementAtOrDefault(abilityEntry.AbilityIndex);

        if (ability != null)
        {
            Player.Instance.abilities.Add(ability);
            PlayerUI.Instance.cooldownController.ActivateCooldownObj(Player.Instance.abilities.Count - 1);
        }

        Destroy(caller);
    }

    public void UpgradeAbility(GameObject caller)
    {
        if (!caller.TryGetComponent<AbilityMenuEntry>(out var abilityEntry))
        {
            Debug.Log("You fucked up mate. Add a AbilityMenuEntry component to the button");
            return;
        }

        var ability = AbilityMapping.ElementAtOrDefault(abilityEntry.AbilityIndex);

        if (ability != null)
        {
            AbilityUpgradeController.Instance.UpgradeAbility(abilityEntry.AbilityIndex);
        }
    }
}
