using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    public CooldownController cooldownController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
}
