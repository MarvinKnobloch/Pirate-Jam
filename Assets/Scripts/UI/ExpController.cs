using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;

public class ExpController : MonoBehaviour
{

    [SerializeField] private bool disableExpGain;

    private float uncalculatedExp;
    private float playerRequiredExp;
    private float playerCurrentExp;
    public int playerCurrentLvL = 1;

    [SerializeField] private Image expBarPreviewImage;
    [SerializeField] private Image expBarImage;
    [SerializeField] private TextMeshProUGUI expBarText;
    [SerializeField] private TextMeshProUGUI currentPlayerLvlText;

    private float currentpercentage;
    private float fillspeed = 0.02f;

    [Range(1f, 300f)]
    public float flatexpnumber = 220;                                     // erhöht die benötigte exp für jedes lvl gleich, um so niederiger die zahl um so weniger exp braucht man zum lvln
    [Range(1.1f, 4f)]
    public float expmultiplier = 2;                                       // je höher die zahl um so steiler die kurve
    [Range(7f, 14f)]
    public float expdivision = 14;                                        // je höher die zahl um so flacher die kurve

    [SerializeField] private UpgradeController upgradeController;
    private void Awake()
    {
        playerRequiredExp = calculaterequiredexp();
        playerExpUpdate();
    }
    public void PlayerGainExp(float expgained)
    {
        if (disableExpGain) return;

        float bonusExp = expgained * (Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.ExpGain) * 0.01f);

        uncalculatedExp += expgained + bonusExp;
        uncalculatedExp = Mathf.Floor(uncalculatedExp * 10) / 10;
        playerExpUpdate();
    }
    private void playerExpUpdate()
    {
        StopCoroutine("fillbar");
        StartCoroutine("fillbar");
        currentPlayerLvlText.text = "Level " + playerCurrentLvL;
    }
    IEnumerator fillbar()
    {
        if (playerCurrentExp + uncalculatedExp > playerRequiredExp) expBarPreviewImage.fillAmount = 1;
        currentpercentage = (playerCurrentExp + uncalculatedExp) / playerRequiredExp;

        if (currentpercentage > 1) expBarText.text = "EXP " + playerRequiredExp + "/" + playerRequiredExp;
        else expBarText.text = "EXP " + (playerCurrentExp + uncalculatedExp) + "/" + playerRequiredExp;

        //fillspeed = 0;
        while (true)
        {
            //if (fillspeed < 0.02f) fillspeed += 0.001f;
            if (expBarImage.fillAmount < 1)
            {
                expBarImage.fillAmount += fillspeed;
            }
            else
            {
                expBarImage.fillAmount = 0;
                levelup();
                playerExpUpdate();
            }
            if (currentpercentage < 1)
            {
                expBarPreviewImage.fillAmount = currentpercentage;
                if (expBarImage.fillAmount >= currentpercentage)
                {
                    expBarImage.fillAmount = currentpercentage;
                    playerCurrentExp += uncalculatedExp;
                    uncalculatedExp = 0;
                    StopCoroutine("fillbar");
                }
            }
            yield return new WaitForSeconds(0.03f);
        }
    }
    private void levelup()
    {
        if (Player.Instance == null) return;

        playerCurrentLvL++;

        Player.Instance.health.HealthUpgrade();
        upgradeController.SetUpgradeSelection();

        uncalculatedExp -= playerRequiredExp - playerCurrentExp;
        uncalculatedExp = Mathf.Floor(uncalculatedExp * 10) / 10;

        playerCurrentExp = 0;
        playerRequiredExp = calculaterequiredexp();
    }

    private int calculaterequiredexp()                                       // formel ist aus einem Video
    {
        int expperlevel = 0;
        for (int levelcycle = 1; levelcycle <= playerCurrentLvL; levelcycle++)
        {
            expperlevel += (int)Mathf.Floor(levelcycle + flatexpnumber * Mathf.Pow(expmultiplier, levelcycle / expdivision));
        }
        return expperlevel / 4;
    }
}
