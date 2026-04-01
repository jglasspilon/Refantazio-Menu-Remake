using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PageSection))]
[DisallowMultipleComponent]
public abstract class UIObjectSelecter<T>: MonoBehaviour where T: MonoBehaviour, ISelectable 
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

    /// <summary>
    /// Event invoked whenever the selected object changes, providing the newly selected instance of <typeparamref name="T"/>.
    /// </summary>
    public event Action<T> OnSelectedObjectChanged;

    /// <summary>
    /// Gets the currently selected object, or null if no selection is active.
    /// </summary>
    public T SelectedObject => m_selectedObject;

    /// <summary>
    /// Indicates whether the selector is currently in a state where all objects are considered selected, disabling individual selection logic.
    /// </summary>
    public bool AllSelected => m_allSelected;

    /// <summary>
    /// Initializes the selector by binding to the parent PageSection's navigation events. Supports optional reversed cycling order.
    /// </summary>
    protected virtual void Awake()
    {
        m_parentSection = GetComponent<PageSection>();

        if(m_reverseOrder)
        {
            m_parentSection.OnCycleDown.AddListener(SelectPrevious);
            m_parentSection.OnCycleUp.AddListener(SelectNext);
            return;
        }

        m_parentSection.OnCycleDown.AddListener(SelectNext);
        m_parentSection.OnCycleUp.AddListener(SelectPrevious);
    }

    /// <summary>
    /// Returns the array of selectable objects currently managed by the selector.
    /// </summary>
    public T[] GetSelectableObjects()
    {
        return m_objects;
    }

    /// <summary>
    /// Selects the object at the current index, if individual selection is enabled.
    /// </summary>
    public void SelectCurrent()
    {
        if (m_allSelected)
            return;

        Select(m_selectedIndex);
    }

    /// <summary>
    /// Selects the previous object in the list, respecting looping and the reverse‑order configuration.
    /// </summary>
    public void SelectPrevious()
    {
        if (m_allSelected)
            return;

        Select(m_selectedIndex - 1);
    }

    /// <summary>
    /// Selects the next object in the list, respecting looping and the reverse‑order configuration.
    /// </summary>
    public void SelectNext()
    {
        if (m_allSelected)
            return;

        Select(m_selectedIndex + 1);
    }

    /// <summary>
    /// Updates the internal list of selectable objects and clamps the current selection index to a valid range.
    /// </summary>
    public void UpdateObjects(params T[] items)
    {
        if (items == null || items.Length == 0)
            return;

        m_objects = items;
        m_selectedIndex = Helper.Arrays.GetSafeIndex(m_selectedIndex, m_objects.Length - 1, false);
    }

    /// <summary>
    /// Selects the object at the specified index. Supports looping behavior, updates selection state, triggers callbacks, and invokes Unity events.
    /// </summary>
    public void Select(int index, bool loop = true)
    {
        if (m_objects == null || m_objects.Length == 0)
            return;
       
        if (m_selectedObject != null)
            m_selectedObject.SetAsSelected(false);

        m_selectedIndex = Helper.Arrays.GetSafeIndex(index, m_objects.Length - 1, loop);
        m_selectedObject = m_objects[m_selectedIndex];
        m_selectedObject.SetAsSelected(true);
        VirtualInvoke();
        OnSelectedObjectChanged?.Invoke(m_selectedObject);
        m_onSelectedObjectChanged?.Invoke(m_selectedObject.gameObject);
    }

    /// <summary>
    /// Marks the selector as having all objects selected, disabling individual selection navigation.
    /// </summary>
    public void SelectAll()
    {
        m_allSelected = true;
    }

    /// <summary>
    /// Clears the "all selected" state and unselects all objects currently managed by the selector.
    /// </summary>
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

    /// <summary>
    /// Resets the selector to its initial state, restoring the selection index and selected object to the first available entry.
    /// </summary>
    public void ResetSelecter()
    {
        m_selectedIndex = 0;
        m_selectedObject = m_objects == null || m_objects.Length == 0 ? null : m_objects[0];
    }

    /// <summary>
    /// Applies a predicate to determine which objects are selectable. Each object is updated to reflect whether it can currently be selected.
    /// </summary>
    public void SetApplicableToSelectable(Func<T, bool> selectablePredicate)
    {
        if (m_objects == null)
            return;

        foreach (T obj in m_objects)
        {
            obj.SetAsSelectable(selectablePredicate(obj));
        }
    }

    /// <summary>
    /// Invoked whenever a new object is selected. Must be implemented by derived classes to provide custom selection behavior.
    /// </summary>
    protected abstract void VirtualInvoke();
}
