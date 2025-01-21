using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MenuController : MonoBehaviour
{
    private Controls controls;

    private GameObject baseMenu;
    private GameObject currentOpenMenu;
    [NonSerialized] public bool gameIsPaused;

    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject ingameMenu;

    [SerializeField] private GameObject confirmController;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI confirmText;

    private void Awake()
    {
        controls = Keybindinputmanager.Controls;
        controls.Enable();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            baseMenu = titleMenu;
            baseMenu.SetActive(true);
        }
        else
        {
            baseMenu = ingameMenu;
        }
    }
    void Update()
    {
        if (controls.Menu.MenuEsc.WasPerformedThisFrame())
        {
            HandleMenu();
        }

    }
    public void HandleMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (titleMenu.activeSelf == true) return;
            else CloseSelectedMenu();
        }
        else
        {
            if (Player.Instance == null) return;

            //if (GameManager.Instance.playerUI.messageBox.activeSelf) GameManager.Instance.playerUI.CloseMessageBox();
            if (ingameMenu.activeSelf == false)
            {
                if (gameIsPaused == false)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    PauseGame();
                    ingameMenu.SetActive(true);

                }
                else CloseSelectedMenu();
            }
            else
            {
                ingameMenu.SetActive(false);
                EndPause();
            }
        }
    }

    public void OpenSelection(GameObject currentMenu)
    {
        {
            currentOpenMenu = currentMenu;
            currentMenu.SetActive(true);

            titleMenu.SetActive(false);
            ingameMenu.SetActive(false);

            AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
        }
    }

    public void StartGame()
    {
        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
        gameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void ResumeGame()
    {
        ingameMenu.SetActive(false);
        EndPause();
    }
    public void RestartGame()
    {
        OpenConfirmController(StartGame, "Restart Game?");
    }
    public void BackToMainMenuController()
    {
        OpenConfirmController(BackToMainMenu, "Back to main menu?");
    }

    private void BackToMainMenu()
    {
        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
        gameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void CloseSelectedMenu()
    {
        if (currentOpenMenu != null)
        {
            currentOpenMenu.SetActive(false);
            currentOpenMenu = null; // Clear previous menu after returning
            baseMenu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No previous menu to return to. Going back to inGameMenu.");
            baseMenu.SetActive(true);
        }
        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
    }

    private void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0;

        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
    }
    private void EndPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsPaused = false;
        Time.timeScale = 1;

        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
    }
    public void OpenConfirmController(UnityAction buttonEvent, string confirmText)
    {
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => buttonEvent());
        confirmController.SetActive(true);
    }
}
