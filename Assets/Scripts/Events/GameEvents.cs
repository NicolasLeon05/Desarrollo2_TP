using System;
using UnityEngine;


public static class GameEvents
{
    public static event Action OnReturnToMainMenu;
    public static event Action OnActivateBaseMenu;
    public static event Action<Menu, GameManager.GameState> OnActivateMenu;
    public static event Action OnSetAllMenusInactive;
    public static event Action OnVictory;

    public static void TriggerReturnToMainMenu()
    {
        OnReturnToMainMenu?.Invoke();
    }

    public static void TriggerActivateBaseMenu()
    {
        OnActivateBaseMenu?.Invoke();
    }

    public static void TriggerActivateMenu(Menu menu, GameManager.GameState state)
    {
        OnActivateMenu?.Invoke(menu, state);
    }

    public static void TriggerSetAllMenusInactive()
    {
        OnSetAllMenusInactive?.Invoke();
    }

    public static void TriggerVictory()
    {
        OnVictory?.Invoke();
    }
}
