using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class CharacterSelectionSection_CasterSelect: CharacterSelectionSection
{
    [SerializeField]
    private SkillsMenuPage m_parentPage;

    public override UniTask EnterSection()
    {
        m_parentPage.ReleaseCaster();
        UpdateSelectabilityOfContent(x => false);
        return base.EnterSection();       
    }

    public override UniTask ExitSection()
    {
        m_selecter.UnselectAll();
        return default;
    }
}

public enum ECharacterSelectionProcedure
{
    SelectingCaster,
    SelectingTarget
}