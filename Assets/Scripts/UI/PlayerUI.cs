using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    public CooldownController cooldownController;

    [Header("Health")]
    public Image healthBar;
    public TextMeshProUGUI healthText;

    [Header("Energy")]
    public Image energyBar;
    public TextMeshProUGUI energyText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void HealthUIUpdate(int current, int max)
    {
        healthBar.fillAmount = (float)current / max;
        healthText.text = current.ToString();
    }
    public void EnergyUIUpdate(int current, int max)
    {
        energyBar.fillAmount = (float)current / max;
        energyText.text = current.ToString();
    }
}
