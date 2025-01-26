using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    private GameObject tooltipWindow;
    private RectTransform tooltipRect;
    [NonSerialized] public TextMeshProUGUI tooltipText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI cooldownText;

    private float rectHeightMultiplier = 0.25f;


    private void Awake()
    {
        tooltipWindow = transform.GetChild(0).gameObject;
        tooltipWindow.SetActive(false);

        tooltipText = tooltipWindow.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        tooltipText.text = string.Empty;

        tooltipRect = tooltipWindow.GetComponent<RectTransform>();

        energyText.text = string.Empty;
        cooldownText.text = string.Empty;

    }
    public void WindowEnable()
    {
        tooltipWindow.SetActive(true);
        tooltipWindow.transform.localScale = new Vector3(0, 0, 0);
    }
    public void SetWindowPosition()
    {
        //Braucht ein Frame um die Height vom ContenSizeFitter zu setzen. Scale wird für den einen Frame auf 0 gesetzt damit dann das FEnster nicht springen sieht
        StartCoroutine(SetWindowPostionAfterResize());
    }
    IEnumerator SetWindowPostionAfterResize()
    {
        yield return null;
        tooltipWindow.transform.localScale = new Vector3(1, 1, 1);
        //Ist keine Formel, einfach ein bisschen ausprobiert was passt
        float widthOffset = Screen.width / 7;

        float heigthoffset = ((Screen.height * 0.5f) - Input.mousePosition.y) / 90;              //Bestimmt ob das Fenster nach unten oder oben geht.

        if (heigthoffset > 0) heigthoffset += tooltipRect.rect.height * rectHeightMultiplier;    //Wie weit das Fenster nach unten/oben geht basierend auf der Größe
        else heigthoffset -= tooltipRect.rect.height * rectHeightMultiplier;

        //links oder rechts von mousePosition          1.35f = 35% vom rechten Bildschirmrand
        if (Screen.width / Input.mousePosition.x > 1.35f) tooltipWindow.transform.position = Input.mousePosition + new Vector3(widthOffset, heigthoffset, 0);
        else tooltipWindow.transform.position = Input.mousePosition + new Vector3(widthOffset * -1, heigthoffset, 0);
    }
    public void HideTooltip()
    {
        tooltipText.text = string.Empty;
        energyText.text = string.Empty;
        cooldownText.text = string.Empty;
        tooltipWindow.SetActive(false);
    }
}
