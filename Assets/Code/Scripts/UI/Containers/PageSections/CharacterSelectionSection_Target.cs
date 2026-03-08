using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelectionSection_Target: CharacterSelectionSection
{
    [SerializeField]
    private GameObject m_allSelectionSplotch;   

    [Header("Animation")][SerializeField]
    private Animator m_sectionAnim;

    [Space][SerializeField]
    private UnityEvent OnEnter;

    public override UniTask EnterSection()
    {
        OnEnter?.Invoke();
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
