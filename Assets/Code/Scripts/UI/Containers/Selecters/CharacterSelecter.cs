using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelecter : UIObjectSelecter<CharacterBanner>
{
    [SerializeField] private UnityEvent<Character> OnCharacterChanged; 
    public event Action<Character> OnCharacterSelected;

    protected void OnEnable()
    {
        m_parentSection.OnConfirm.AddListener(SelectCharacter);
    }

    protected void OnDisable()
    {
        m_parentSection.OnConfirm.RemoveListener(SelectCharacter);
    }

    public void SelectCharacter()
    {
        OnCharacterSelected?.Invoke(SelectedObject == null ? null : SelectedObject.Character);
    }

    protected override void VirtualInvoke()
    {
        OnCharacterChanged?.Invoke(SelectedObject == null ? null : SelectedObject.Character);
    }
}
