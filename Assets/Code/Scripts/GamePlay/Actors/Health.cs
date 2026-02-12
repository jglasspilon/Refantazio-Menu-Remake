using System;
using UnityEngine;

public class Health
{
    public event Action OnHealthChange;
    
    private int m_currentHealth;
    private int m_maxHealth;

    public void SetMaxHealth(int newMaxHealth)
    {
        m_currentHealth = m_currentHealth.Map(0, m_maxHealth, 0, newMaxHealth);
        m_maxHealth = newMaxHealth;
    }
}
