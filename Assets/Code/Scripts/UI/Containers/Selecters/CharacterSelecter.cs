using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelecter : UIObjectSelecter<CharacterBanner>
{
    [SerializeField] private UnityEvent<Character> OnCharacterChanged; 
    public event Action<Character> OnCharacterSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectCharacter;
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
