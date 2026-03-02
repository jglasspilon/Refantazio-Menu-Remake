using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class CharacterSelectionSection_Items: CharacterSelectionSection
{
    [SerializeField]
    private GameObject m_allSelectionSplotch;   

    [Header("Animation")][SerializeField]
    private Animator m_sectionAnim;

    [SerializeField]
    private AnimatedMover m_bodyMover;

    private bool m_selectedAll;

    public override UniTask EnterSection()
    {
        if(m_bodyMover != null)
            m_bodyMover.MoveIn();

        if(m_sectionAnim != null)
            m_sectionAnim.SetBool("CharSection", true);

        if (m_allSelectionSplotch != null)
            m_allSelectionSplotch.SetActive(m_selectedAll);

        return base.EnterSection();
    }

    public override UniTask ExitSection()
    {
        m_selectedAll = false;
        return base.ExitSection();
    }

    protected override void UpdateSelectedObject()
    {
        if (m_selectedAll)
            return;

        base.UpdateSelectedObject();
    }

    public void SelectAll()
    {
        m_selectedAll = true;
    }
}
