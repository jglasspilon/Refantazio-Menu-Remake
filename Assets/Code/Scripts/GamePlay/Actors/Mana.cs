using System;
using UnityEngine;

public class Mana
{
    public event Action OnManaChange;

    private int m_currentMp;
    private int m_maxMp;

    public void SetMaxMana(int newMaxMana)
    {
        m_currentMp = m_currentMp.Map(0, m_maxMp, 0, newMaxMana);
        m_maxMp = newMaxMana;
    }
}
