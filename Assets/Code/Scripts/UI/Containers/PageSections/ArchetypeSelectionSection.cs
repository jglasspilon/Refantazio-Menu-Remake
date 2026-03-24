using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ArchetypeSelectionSection: UIListSelectionSection<ArchetypeBanner, ArchetypeBannerGenerator, Archetype, Character>
{
    [SerializeField] private CharacterSelecter m_characterSelecter;

    private EArchetypeSortType m_sortType;

    public override UniTask EnterSection()
    {
        GenerateUIContent();
        m_selecter.SelectCurrent();
        m_onEnter?.Invoke();
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selecter.SetApplicableToSelectable(x => false);
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
        m_onExit?.Invoke();
        return default;
    }

    public override void ResetSection()
    {
        m_sortType = 0;
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
    }

    public void HandleOnSortChanged(EArchetypeSortType sortType)
    {
        m_selecter.ResetSelecter();
        m_sortType = sortType;
        GenerateUIContent();
    }

    protected override void GenerateUIContent()
    {
        m_dataModel = m_characterSelecter.SelectedObject.Character;

        Archetype[] archetypesToGenerate = GetSortedArchetyps(m_sortType, m_dataModel.Archetypes);
        ArchetypeBanner[] archetypeBanners = m_generater.GenerateContent(archetypesToGenerate);
        DelaySelectionOnGenerate(archetypeBanners);
    }

    private Archetype[] GetSortedArchetyps(EArchetypeSortType sortType, Archetype[] unsorted)
    {
        switch (sortType)
        {
            case EArchetypeSortType.Archetype: 
                return unsorted.OrderBy(x => x.SortOrder).ToArray();
            case EArchetypeSortType.Rank:
                return unsorted.OrderByDescending(x => x.Rank.Value).ThenBy(x => x.SortOrder).ToArray();
        }

        return unsorted;
    }

    private async UniTask DelaySelectionOnGenerate(ArchetypeBanner[] banners)
    {
        await Helper.Timing.DelaySeconds(0.1f);
        m_selecter.UpdateObjects(banners);
        m_selecter.SelectCurrent();
    }
}
