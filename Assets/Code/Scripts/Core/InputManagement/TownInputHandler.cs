using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class TownInputHandler : InputHandler
{
    [SerializeField] private MenuManager m_menu;
    [SerializeField] private LoggingProfile m_logProfile;

    protected override void BindEvents()
    {
        m_input.Town.OpenMenu.performed += OnOpenMenu;
        m_input.Town.ItemsShortcut.performed += OnItemsShortcut;
        m_input.Town.SkillsShortcut.performed += OnSkillsShortcut;
        m_input.Town.PartyShortcut.performed += OnPartyShortcut;
        m_input.Town.FollowersShortcut.performed += OnFollowersShortcut;
    }

    protected override void UnBindEvents()
    {
        m_input.Town.OpenMenu.performed -= OnOpenMenu;
        m_input.Town.ItemsShortcut.performed -= OnItemsShortcut;
        m_input.Town.SkillsShortcut.performed -= OnSkillsShortcut;
        m_input.Town.PartyShortcut.performed -= OnPartyShortcut;
        m_input.Town.FollowersShortcut.performed -= OnFollowersShortcut;
    }

    private void OnOpenMenu(InputAction.CallbackContext context)
    {
        OpenMenuOnPage(EMenuPages.Main);
    }

    private void OnItemsShortcut(InputAction.CallbackContext context)
    {
        OpenMenuOnPage(EMenuPages.Item);
    }

    private void OnSkillsShortcut(InputAction.CallbackContext context)
    {
        OpenMenuOnPage(EMenuPages.Skill);
    }

    private void OnPartyShortcut(InputAction.CallbackContext context)
    {
        OpenMenuOnPage(EMenuPages.Party);
    }

    private void OnFollowersShortcut(InputAction.CallbackContext context)
    {
        OpenMenuOnPage(EMenuPages.Follower);
    }

    private void OpenMenuOnPage(EMenuPages launchPage)
    {
        if (!ObjectResolver.Instance.TryResolve(null, out IGameStateManagementService gameState))
        {
            Logger.LogError("Failed to open menu, no game state management service has been registered", gameObject, m_logProfile);
            return;
        }
      
        m_menu.SetLaunchPage(launchPage);
        gameState.ChangeState(EGameStates.Menu);
        Logger.Log($"Open Menu: {launchPage}", gameObject, m_logProfile);
    }
}
