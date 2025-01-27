using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class CooldownController : MonoBehaviour
{
    [SerializeField] private GameObject cooldownPrefab;
    [SerializeField] private int cooldownPrefabCount;

    public List<GameObject> cooldownObj = new List<GameObject>();
    public List<Image> cooldownImage = new List<Image>();
    public List<bool> onCooldown = new List<bool>();
    public List<TextMeshProUGUI> cooldownText = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> cooldownHotkey = new List<TextMeshProUGUI>();

    private Controls controls;
    private void Awake()
    {
        controls = Keybindinputmanager.Controls;

        for (int i = 0; i < cooldownPrefabCount; i++)
        {
            GameObject prefab = Instantiate(cooldownPrefab, transform.position, Quaternion.identity, transform);

            //Set ability Image  prefab.GetComponent<Image>() = Player.Instance.abilities
            cooldownObj.Add(prefab);
            cooldownImage.Add(prefab.transform.GetChild(1).GetComponent<Image>());
            cooldownImage[i].enabled = false;

            onCooldown.Add(false);

            cooldownText.Add(cooldownImage[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
            cooldownText[i].text = string.Empty;

            cooldownHotkey.Add(prefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>());
            prefab.SetActive(false);
        }
        HotkeysUpdate();
    }
    private void Start()
    {
        for (int i = 0; i < Player.Instance.abilities.Count; i++)
        {
            ActivateCooldownObj(i);
        }
    }
    public void ActivateCooldownObj(int slot)
    {
        cooldownObj[slot].SetActive(true);
        cooldownObj[slot].GetComponent<TooltipWindow>().ability = Player.Instance.abilities[slot];
        cooldownObj[slot].GetComponent<TooltipWindow>().abilitySlot = slot;
    }
    public void HotkeysUpdate()
    {
        cooldownHotkey[0].text = controls.Player.Ability1.GetBindingDisplayString();
        cooldownHotkey[1].text = controls.Player.Ability2.GetBindingDisplayString();
        cooldownHotkey[2].text = controls.Player.Ability3.GetBindingDisplayString();
        cooldownHotkey[3].text = controls.Player.Ability4.GetBindingDisplayString();
        cooldownHotkey[4].text = controls.Player.Ability5.GetBindingDisplayString();
        cooldownHotkey[5].text = controls.Player.Ability6.GetBindingDisplayString();
        cooldownHotkey[6].text = controls.Player.Ability7.GetBindingDisplayString();
        cooldownHotkey[7].text = controls.Player.Ability8.GetBindingDisplayString();
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
