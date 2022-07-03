using UnityEngine;

public class UIMediator : MonoBehaviour {
    [SerializeField] private Gameplay game;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject settingsMenu;

    private GameObject currentMenu = null;
    private bool isInGame = false;

    public void StartApp() {
        ShowMenu(mainMenu);
        HideAndCloseGame();
        pauseMenu.SetActive(false);
        loseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void StartGame() {
        isInGame = true;
        HideCurrentMenu();
        game.gameObject.SetActive(true);
        game.StartGame();
    }

    public void RestartGame() {
        HideCurrentMenu();
        game.RestartGame();
    }

    private void HideAndCloseGame() {
        game.EndGame();
        game.gameObject.SetActive(false);
        isInGame = false;
    }

    public void PauseGame() {
        game.PauseGame();
        ShowMenu(pauseMenu);
    }

    public void UnpauseGame() {
        HideCurrentMenu();
        game.UnpauseGame();
    }

    public void OpenLoseMenu() {
        HideAndCloseGame();
        ShowMenu(loseMenu);
    }

    public void GoToMainMenu() {
        if (isInGame) HideAndCloseGame();
        ShowMenu(mainMenu);
    }

    public void OpenSettings() => ShowMenu(settingsMenu);

    public void ToggleDarkTheme() {

    }

    public void ToggleSounds() {

    }

    public void OpenSkinsMenu() {

    }

    private void ShowMenu(GameObject menu) {
        HideCurrentMenu();
        currentMenu = menu;
        menu.SetActive(true);
    }

    private void HideCurrentMenu() {
        if (currentMenu) {
            currentMenu.SetActive(false);
            currentMenu = null;
        }
    }
}