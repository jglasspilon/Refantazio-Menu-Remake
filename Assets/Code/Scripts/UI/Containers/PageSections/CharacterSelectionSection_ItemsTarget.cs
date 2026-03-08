using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class CharacterSelectionSection_ItemsTarget: CharacterSelectionSection
{
    [SerializeField]
    private GameObject m_allSelectionSplotch;   

    [Header("Animation")][SerializeField]
    private Animator m_sectionAnim;

    [SerializeField]
    private AnimatedMover m_bodyMover;

    public override UniTask EnterSection()
    {
        m_bodyMover.MoveIn();
        m_sectionAnim.SetBool("CharSection", true);
        m_allSelectionSplotch.SetActive(m_selecter.AllSelected);
        return base.EnterSection();
    }

    public override UniTask ExitSection()
    {
        m_allSelectionSplotch.SetActive(false);
        m_selecter.UnselectAll();
        return base.ExitSection();
    }
}
