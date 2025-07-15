using System;
using UnityEngine;


public static class GameEvents
{
    public static event Action OnReturnToMainMenu;
    public static event Action OnActivateBaseMenu;
    public static event Action<Menu, GameManager.GameState> OnActivateMenu;
    public static event Action OnSetAllMenusInactive;

    public static void TriggerReturnToMainMenu()
    {
        Debug.Log("RETURN TO MAIN MENU EVENT CALLED");
        OnReturnToMainMenu?.Invoke();
    }

    public static void TriggerActivateBaseMenu()
    {
        Debug.Log("ACTIVATE MENU EVENT CALLED");
        OnActivateBaseMenu?.Invoke();
    }

    public static void TriggerActivateMenu(Menu menu, GameManager.GameState state)
    {
        Debug.Log("REQUEST MENU TRANSITION EVENT CALLED");
        OnActivateMenu?.Invoke(menu, state);
    }

    public static void TriggerSetAllMenusInactive()
    {
        Debug.Log("SET ALL MENUS INACTIVE EVENT CALLED");
        OnSetAllMenusInactive?.Invoke();
    }
}
