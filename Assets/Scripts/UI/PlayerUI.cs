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

    [Header("Resources")]
    public TextMeshProUGUI ironText;
    public TextMeshProUGUI copperText;
    public TextMeshProUGUI woodText;

    [Header("Numbers")]
    public TextMeshProUGUI killCount;
    private int kills;

    public TextMeshProUGUI gameTimer;
    private float time;
    private int seconds;
    private int minutes;

    [Header("Upgrades")]
    public UpgradeController upgradeController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        killCount.text = 0 + " Kills";
    }
    private void Update()
    {
        time += Time.deltaTime;

        seconds = Mathf.FloorToInt(time % 60);
        minutes = Mathf.FloorToInt(time / 60 % 60);
        gameTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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

    public void ResourceUIUpdate(int iron, int copper, int wood)
    {
        ironText.text = iron.ToString();
        copperText.text = copper.ToString();
        woodText.text = wood.ToString();
    }
    public void KillCountUpdate()
    {
        kills++;
        killCount.text = kills + " Kills";
    }
}
