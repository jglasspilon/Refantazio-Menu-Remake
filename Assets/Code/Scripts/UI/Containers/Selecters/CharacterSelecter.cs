using System;
using UnityEngine;

public class CharacterSelecter : UIObjectSelecter<CharacterBanner>
{
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
}
