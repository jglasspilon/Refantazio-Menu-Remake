using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class UIObjectSelecter<T> where T: MonoBehaviour, ISelectable 
{
    private T m_selectedObject;    
    private T[] m_objects;

    public T SelectedObject => m_selectedObject;

    public T[] GetSelectableObjects()
    {
        return m_objects;
    }

    public int UpdateObjectsAndReturnIndex(T[] items, int selectedIndex)
    {
        m_objects = items;
        return Helper.Arrays.GetSafeIndex(selectedIndex, m_objects.Length - 1, false);
    }

    public int Select(int index, bool loop = true)
    {
        if (m_objects == null || m_objects.Length == 0)
            return 0;
       
        if (m_selectedObject != null)
            m_selectedObject.SetAsSelected(false);

        index = Helper.Arrays.GetSafeIndex(index, m_objects.Length - 1, loop);
        m_selectedObject = m_objects[index];
        m_selectedObject.SetAsSelected(true);
        return index;
    }

    public void UnselectAll()
    {
        if (m_objects == null)
            return;

        foreach(T obj in m_objects)
        {
            obj.SetAsSelected(false);
        }
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
