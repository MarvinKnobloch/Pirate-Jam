using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class CooldownController : MonoBehaviour
{
    [SerializeField] private GameObject cooldownPrefab;
    [SerializeField] private int cooldownPrefabCount;

    public List<Image> cooldownImage = new List<Image>();
    public List<bool> onCooldown = new List<bool>();
    public List<TextMeshProUGUI> cooldownText = new List<TextMeshProUGUI>();
    private void Awake()
    {
        for (int i = 0; i < cooldownPrefabCount; i++)
        {
            GameObject prefab = Instantiate(cooldownPrefab, transform.position, Quaternion.identity, transform);

            //Set ability Image  prefab.GetComponent<Image>() = Player.Instance.abilities
            cooldownImage.Add(prefab.transform.GetChild(1).GetComponent<Image>());
            cooldownImage[i].enabled = false;

            onCooldown.Add(false);

            cooldownText.Add(cooldownImage[i].GetComponentInChildren<TextMeshProUGUI>());
            cooldownText[i].text = string.Empty;
        }
    }
    public void CooldownStart(int number, float time)
    {
        StartCoroutine(Cooldown(number, time));
    }
    private IEnumerator Cooldown(int number, float time)
    {
        float maxTime = time;
        onCooldown[number] = true;
        cooldownImage[number].enabled = true;
        cooldownImage[number].fillAmount = 1;
        cooldownText[number].text = Mathf.Ceil(time).ToString();

        while (time > 0)
        {
            time -= Time.deltaTime;
            cooldownImage[number].fillAmount = time / maxTime;
            cooldownText[number].text = Mathf.Ceil(time).ToString();
            yield return null;
        }
        onCooldown[number] = false;
        cooldownText[number].text = string.Empty;
        cooldownImage[number].enabled = false;
    }
}
