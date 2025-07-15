using System;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    [SerializeField] private NavigationController navigationController;
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
    }


    private void OnDisable()
    {
        GameEvents.OnReturnToMainMenu -= HandleReturnToMainMenu;
        GameEvents.OnActivateBaseMenu -= HandleActivateBaseMenu;
        GameEvents.OnActivateMenu -= HandleActivateMenu;
        GameEvents.OnSetAllMenusInactive -= HandleSetAllMenusInactive;
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
}
