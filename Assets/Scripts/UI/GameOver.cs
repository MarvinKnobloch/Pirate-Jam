using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Image gameOverBackground;
    private Color backgroundColor;
    [SerializeField] private GameObject resetButton;
    private float timer;
    [SerializeField] private float timeUntilGameover;

    [SerializeField] private TextMeshProUGUI gameOvertext;
    [TextArea] [SerializeField] private string gameOverMessage;
    private Color gameOverTextColor;

    private void Awake()
    {
        gameOverBackground = GetComponent<Image>();
        backgroundColor = Color.black;
        backgroundColor.a = 0;
        gameOverBackground.color = backgroundColor;

        gameOverTextColor = Color.white;
        gameOverTextColor.a = 0 + 0.25f;
        gameOvertext.faceColor = gameOverTextColor;
        gameOvertext.text = gameOverMessage;

        resetButton.SetActive(false);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        float time = timer / timeUntilGameover;
        backgroundColor.a = time;
        gameOverBackground.color = backgroundColor;

        gameOverTextColor.a = time + 0.25f;
        gameOvertext.faceColor = gameOverTextColor;

        if (backgroundColor.a > 0.9)
        {
            resetButton.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
