using System;
using UnityEngine;


public static class GameEvents
{
    public static event Action OnReturnToMainMenu;
    public static event Action OnActivateMenu;

    public static void TriggerReturnToMainMenu()
    {
        Debug.Log("RETURN TO MAIN MENU EVENT CALLED");
        OnReturnToMainMenu?.Invoke();
    }

    public static void TriggerActivateBaseMenu()
    {
        Debug.Log("ACTIVATE MENU EVENT CALLED");
        OnActivateMenu?.Invoke();
    }
}
