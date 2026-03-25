using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

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

        int equipedIndex = -99;
        Archetype equiped = m_characterSelecter.SelectedObject.Character.Equipment.Archetype;

        for (int i = 0; i < archetypeBanners.Length; i++)
        {
            if (equiped.ID == archetypeBanners[i].Archetype.ID)
            {
                equipedIndex = i;
            }

            archetypeBanners[i].ShowAsEquiped(equipedIndex == i);
        }

        DelaySelectionOnGenerate(archetypeBanners, equipedIndex);
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

    private async UniTask DelaySelectionOnGenerate(ArchetypeBanner[] banners, int index)
    {
        await Helper.Timing.DelaySeconds(0.1f);
        m_selecter.UpdateObjects(banners);
        m_selecter.Select(index, false);
    }
}
