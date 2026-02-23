using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public class UIObjectSelecter<T> where T: MonoBehaviour, ISelectable 
{
    [SerializeField][ReadOnly]
    private T m_selectedObject;
    
    private T[] m_objects;

    public T SelectedObject => m_selectedObject;

    public int UpdateObjectsAndReturnIndex(T[] items, int selectedIndex)
    {
        m_objects = items;
        return GetSafeIndex(selectedIndex, false);
    }

    public int Select(int index, bool loop = true)
    {
        if (m_objects.Length == 0)
            return 0;
       
        if (m_selectedObject != null)
            m_selectedObject.SetAsSelected(false);

        index = GetSafeIndex(index, loop);
        m_selectedObject = m_objects[index];
        m_selectedObject.SetAsSelected(true);
        return index;
    }

    public void UnselectAll()
    {
        foreach(T obj in m_objects)
        {
            obj.SetAsSelected(false);
        }
    }

    private int GetSafeIndex(int index, bool loop)
    {
        if (loop)
        {
            if (index < 0)
                return index = m_objects.Length - 1;
            else if (index >= m_objects.Length)
                return index = 0;
            return index;
        }

        return Mathf.Clamp(index, 0, m_objects.Length - 1);
    }
}
