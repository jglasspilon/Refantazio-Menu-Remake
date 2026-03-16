using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PageSection))]
[DisallowMultipleComponent]
public class UIObjectSelecter<T>: MonoBehaviour where T: MonoBehaviour, ISelectable 
{
    [SerializeField]
    private bool m_reverseOrder;

    [Space][SerializeField]
    private UnityEvent<GameObject> m_onSelectedObjectChanged;

    private T m_selectedObject;    
    private T[] m_objects;
    private int m_selectedIndex;
    private bool m_allSelected;
    protected PageSection m_parentSection;

    public event Action<T> OnSelectedObjectChanged;
    public T SelectedObject => m_selectedObject;
    public bool AllSelected => m_allSelected;

    protected virtual void Awake()
    {
        m_parentSection = GetComponent<PageSection>();

        if(m_reverseOrder)
        {
            m_parentSection.OnCycleDown += SelectPrevious;
            m_parentSection.OnCycleUp += SelectNext;
            return;
        }

        m_parentSection.OnCycleDown += SelectNext;
        m_parentSection.OnCycleUp += SelectPrevious;
    }

    public T[] GetSelectableObjects()
    {
        return m_objects;
    }

    public void SelectCurrent()
    {
        if (m_allSelected)
            return;

        Select(m_selectedIndex);
    }

    public void SelectPrevious()
    {
        if (m_allSelected)
            return;

        Select(m_selectedIndex - 1);
    }

    public void SelectNext()
    {
        if (m_allSelected)
            return;

        Select(m_selectedIndex + 1);
    }

    public void UpdateObjects(params T[] items)
    {
        if (items == null || items.Length == 0)
            return;

        m_objects = items;
        m_selectedIndex = Helper.Arrays.GetSafeIndex(m_selectedIndex, m_objects.Length - 1, false);
    }

    public void Select(int index, bool loop = true)
    {
        if (m_objects == null || m_objects.Length == 0)
            return;
       
        if (m_selectedObject != null)
            m_selectedObject.SetAsSelected(false);

        m_selectedIndex = Helper.Arrays.GetSafeIndex(index, m_objects.Length - 1, loop);
        m_selectedObject = m_objects[m_selectedIndex];
        m_selectedObject.SetAsSelected(true);
        OnSelectedObjectChanged?.Invoke(m_selectedObject);
        m_onSelectedObjectChanged?.Invoke(m_selectedObject.gameObject);
    }

    public void SelectAll()
    {
        m_allSelected = true;
    }

    public void UnselectAll()
    {
        m_allSelected = false;

        if (m_objects == null)
            return;

        foreach(T obj in m_objects)
        {
            obj.SetAsSelected(false);
        }
    }

    public void ResetSelecter()
    {
        m_selectedIndex = 0;
        m_selectedObject = m_objects == null || m_objects.Length == 0 ? null : m_objects[0];
    }

    public void SetApplicableToSelectable(Func<T, bool> selectablePredicate)
    {
        if (m_objects == null)
            return;

        foreach (T obj in m_objects)
        {
            obj.SetAsSelectable(selectablePredicate(obj));
        }
    }
}
