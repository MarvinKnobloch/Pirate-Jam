using System;
using UnityEngine;

public class GatherResource : MonoBehaviour
{
    public ResourceType Type;
    public int MaxAmount;
    private int _amount;

    void OnEnable()
    {
        _amount = MaxAmount;
    }

    public int Gather(int gatherAmount)
    {
        var collected = Math.Min(_amount, gatherAmount);
        _amount -= gatherAmount;

        if (_amount <= 0)
        {
            PlayerUI.Instance.allressources.Remove(this);
            Destroy(gameObject);
        }

        return collected;
    }
}

public enum ResourceType
{
    Wood,
    Copper,
    Gold
}
