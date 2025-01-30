using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] public GameObject upgrades;
    [SerializeField] private GameObject upgradeSelectionGrid;
    private List<UpgradeSlot> upgradeSlots = new List<UpgradeSlot>();

    private int activeSlots;
    [SerializeField] private int maxSlots;

    private void Awake()
    {
        for (int i = 0; i < upgradeSelectionGrid.transform.childCount; i++)
        {
            upgradeSlots.Add(upgradeSelectionGrid.transform.GetChild(i).GetComponent<UpgradeSlot>());
            upgradeSlots[i].gameObject.SetActive(false);
        }
    }
    public void SetUpgradeSelection()
    {
        if (upgradeSlots.Count <= 0) return;

        for (int i = 0; i < upgradeSlots.Count; i++)
        {
            upgradeSlots[i].gameObject.SetActive(false);
            activeSlots = 0;

        }
        StartCoroutine(SelectSlots());
    }
    IEnumerator SelectSlots()
    {
        if (upgradeSlots.Count < maxSlots) maxSlots = upgradeSlots.Count;

        while (activeSlots < maxSlots)
        {
            int slotNumber = Random.Range(0, upgradeSlots.Count);
            if (upgradeSlots[slotNumber].gameObject.activeSelf == false)
            {
                upgradeSlots[slotNumber].gameObject.SetActive(true);
                upgradeSlots[slotNumber].gameObject.transform.SetAsFirstSibling();
                activeSlots++;
            }
            yield return null;
        }
        upgrades.SetActive(true);
        Time.timeScale = 0;
    }
    public void RemoveUpgrade(UpgradeSlot slot)
    {
        if (upgradeSlots.Contains(slot))
        {
            upgradeSlots.Remove(slot);
            Destroy(slot.gameObject);
            CloseController();
        }
    }
    public void CloseController()
    {
        Time.timeScale = 1;

        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.buy);
        upgrades.SetActive(false);
    }
}
