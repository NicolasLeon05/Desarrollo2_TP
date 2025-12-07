using System;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    [SerializeField] private NavigationController navigationController;
    [SerializeField] private Menu victoryMenu;

    private Menu baseMenu;
    private SceneController sceneController;
    private GameManager gameManager;

    private void Start()
    {
        ServiceProvider.TryGetService(out sceneController);
        ServiceProvider.TryGetService(out gameManager);

        baseMenu = navigationController.baseMenu;
    }

    private void OnEnable()
    {
        GameEvents.OnReturnToMainMenu += HandleReturnToMainMenu;
        GameEvents.OnActivateBaseMenu += HandleActivateBaseMenu;
        GameEvents.OnActivateMenu += HandleActivateMenu;
        GameEvents.OnSetAllMenusInactive += HandleSetAllMenusInactive;
        GameEvents.OnVictory += HandleVictory;
    }


    private void OnDisable()
    {
        GameEvents.OnReturnToMainMenu -= HandleReturnToMainMenu;
        GameEvents.OnActivateBaseMenu -= HandleActivateBaseMenu;
        GameEvents.OnActivateMenu -= HandleActivateMenu;
        GameEvents.OnSetAllMenusInactive -= HandleSetAllMenusInactive;
        GameEvents.OnVictory -= HandleVictory;
    }

    private void HandleReturnToMainMenu()
    {

        gameManager.ResumeTime();
        gameManager.ShowCursor();
        sceneController.UnloadNonPersistentScenes();
        gameManager.SetState(GameManager.GameState.MainMenu);
    }

    private void HandleActivateBaseMenu()
    {
        navigationController.SetMenuActive(baseMenu);
        gameManager.SetState(GameManager.GameState.MainMenu);
    }

    private void HandleActivateMenu(Menu menu, GameManager.GameState state)
    {
        navigationController.SetMenuActive(menu);
        gameManager.SetState(state);
    }

    private void HandleSetAllMenusInactive()
    {
        navigationController.SetAllInactive();
    }

    private void HandleVictory()
    {
        gameManager.ResumeTime();
        gameManager.ShowCursor();
        sceneController.UnloadNonPersistentScenes();
        navigationController.SetMenuActive(victoryMenu);
        gameManager.SetState(GameManager.GameState.Victory);
    }
}
