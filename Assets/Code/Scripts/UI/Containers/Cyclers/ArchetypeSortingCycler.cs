using System;
using UnityEngine;
using UnityEngine.Events;

public class ArchetypeSortingCycler : MonoBehaviour
{
    [SerializeField] private PageSection m_parentSection;
    [Space]
    [SerializeField] private UnityEvent<EArchetypeSortType> OnSortChanged;
    
    private int m_index;

    public EArchetypeSortType SelectedSort => GetSortFromIndex(m_index);

    private void OnEnable()
    {
        m_parentSection.OnPageRightLv1 += NextCategory;
        m_parentSection.OnPageLeftLv1 += PreviousCategory;
    }

    private void OnDisable()
    {
        m_index = 0;
        m_parentSection.OnPageRightLv1 -= NextCategory;
        m_parentSection.OnPageLeftLv1 -= PreviousCategory;
    }

    private void PreviousCategory()
    {
        m_index--;
        OnSortChanged?.Invoke(GetSortFromIndex(m_index));
    }

    private void NextCategory()
    {
        m_index++;
        OnSortChanged?.Invoke(GetSortFromIndex(m_index));
    }

    private EArchetypeSortType GetSortFromIndex(int index)
    {
        int count = Enum.GetValues(typeof(EArchetypeSortType)).Length;
        int wrapped = ((index % count) + count) % count;
        return (EArchetypeSortType)wrapped;
    }
}

public enum EArchetypeSortType
{
    Archetype,
    Rank
}
