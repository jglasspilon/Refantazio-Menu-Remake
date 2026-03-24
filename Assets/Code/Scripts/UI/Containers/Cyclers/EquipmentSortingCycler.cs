using System;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentSortingCycler : MonoBehaviour
{
    [SerializeField] private PageSection m_parentSection;
    [Space]
    [SerializeField] private UnityEvent<EEquipmentSortType> OnSortChanged;
    
    private int m_index;

    public EEquipmentSortType SelectedSort => GetSortFromIndex(m_index);

    private void OnEnable()
    {
        m_parentSection.OnPageRightLv1.AddListener(NextCategory);
        m_parentSection.OnPageLeftLv1.AddListener(PreviousCategory);
    }

    private void OnDisable()
    {
        m_index = 0;
        m_parentSection.OnPageRightLv1.RemoveListener(NextCategory);
        m_parentSection.OnPageLeftLv1.RemoveListener(PreviousCategory);
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

    private EEquipmentSortType GetSortFromIndex(int index)
    {
        int count = Enum.GetValues(typeof(EEquipmentSortType)).Length;
        int wrapped = ((index % count) + count) % count;
        return (EEquipmentSortType)wrapped;
    }
}

public enum EEquipmentSortType
{
    MainStat,
    SecondaryStat
}
