using System;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    [SerializeField] private NavigationController navigationController;
    [SerializeField] private Menu victoryMenu;

    private Menu baseMenu;

    private void Start()
    {
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
        GameManager.Instance.ResumeTime();
        GameManager.Instance.ShowCursor();
        SceneController.Instance.UnloadNonPersistentScenes();
        GameManager.Instance.SetState(GameManager.GameState.MainMenu);
    }

    private void HandleActivateBaseMenu()
    {
        navigationController.SetMenuActive(baseMenu);
        GameManager.Instance.SetState(GameManager.GameState.MainMenu);
    }

    private void HandleActivateMenu(Menu menu, GameManager.GameState state)
    {
        navigationController.SetMenuActive(menu);
        GameManager.Instance.SetState(state);
    }

    private void HandleSetAllMenusInactive()
    {
        navigationController.SetAllInactive();
    }

    private void HandleVictory()
    {
        GameManager.Instance.ResumeTime();
        GameManager.Instance.ShowCursor();
        SceneController.Instance.UnloadNonPersistentScenes();
        navigationController.SetMenuActive(victoryMenu);
        GameManager.Instance.SetState(GameManager.GameState.Victory);
    }
}
