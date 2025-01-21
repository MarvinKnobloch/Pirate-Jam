using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FPSSlider : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        _slider.value = PlayerPrefs.GetInt("FPS", 60);
        Text.text = _slider.value.ToString();
    }

    public void FPSChanged(float value)
    {
        Text.text = value.ToString();
        PlayerPrefs.SetInt("FPS", (int)value);
        Application.targetFrameRate = (int)value;
    }
}
