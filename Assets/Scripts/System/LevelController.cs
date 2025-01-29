using System;
using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelSetting[] levelSettings;
    [SerializeField] private string finalMessage;

    private int currentLevel;
    void Start()
    {
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        levelSettings[currentLevel].spawnDirector.gameObject.SetActive(true);
        PlayerUI.Instance.gameTimerInfoText.text = levelSettings[currentLevel].levelText.ToString();
        PlayerUI.Instance.currentLevelTime = levelSettings[currentLevel].levelLength;
        yield return new WaitForSeconds(levelSettings[currentLevel].levelLength);

        levelSettings[currentLevel].spawnDirector.gameObject.SetActive(false);
        currentLevel++;

        if(currentLevel < levelSettings.Length)
        {
            StartCoroutine(NextLevel());
        }
        else
        {
            PlayerUI.Instance.gameTimerInfoText.text = finalMessage;
            PlayerUI.Instance.gameTimer.gameObject.SetActive(false);
        }
    }
    [Serializable]
    private struct LevelSetting
    {
        public SpawnDirector spawnDirector;
        public int levelLength;
        public string levelText;
    }
}
