using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class CharacterSelectionSection_Skills: CharacterSelectionSection
{
    [SerializeField]
    private ECharacterSelectionProcedure m_currentProcedure;

    [SerializeField]
    private SkillsMenuPage m_parentPage;

    private bool m_selectAll;

    public override UniTask EnterSection()
    {

        return base.EnterSection();
    }

    public override UniTask ExitSection()
    {
        
        return base.ExitSection();
    }

    public override void OnConfirm()
    {
        base.OnConfirm();

    }

    protected override void UpdateSelectedObject()
    {
        base.UpdateSelectedObject();

        if (m_currentProcedure == ECharacterSelectionProcedure.SelectingCaster)
        {
            m_parentPage.ChangeCaster(SelectedObject.Character);
        }
    }
}

public enum ECharacterSelectionProcedure
{
    SelectingCaster,
    SelectingTarget
}