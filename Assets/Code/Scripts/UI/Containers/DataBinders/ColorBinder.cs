using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ColorBinder : PropertyBinder
{
    [SerializeField] ColorOption[] m_colorOptions;

    private MaskableGraphic m_graphic;

    private void Awake()
    {
        Initialize();
    }

    protected void Apply(int value)
    {
        if (m_colorOptions.Length == 0)
        {
            Logger.Log($"Failed to bind property to Color of {gameObject.name}. No Color options set.", m_logProfile);
            return;
        }

        Initialize();
        foreach (ColorOption option in m_colorOptions)
        {
            if(option.Condition.IsMet(value, out string message))
            {
                m_graphic.color = option.Color;
                return;
            }
        }

        Logger.Log($"Failed to bind property to Color of {gameObject.name}. No condition was met.", m_logProfile);
    }

    protected void Apply(float value)
    {
        if (m_colorOptions.Length == 0)
        {
            Logger.Log($"Failed to bind property to Color of {gameObject.name}. No Color options set.", m_logProfile);
            return;
        }

        Initialize();
        foreach (ColorOption option in m_colorOptions)
        {
            if (option.Condition.IsMet(value, out string message))
            {
                m_graphic.color = option.Color;
                return;
            }
        }

        Logger.Log($"Failed to bind property to Color of {gameObject.name}. No condition was met.", m_logProfile);
    }

    protected void Apply(Color color)
    {
        Initialize();
        m_graphic.color = color;
    }

    protected override void Apply(object value)
    {
        Logger.Log($"Failed to bind property to Color of {gameObject.name}. Property type {value.GetType()} is not supported.", m_logProfile);
    }

    private void Initialize()
    {
        if(m_graphic == null)
            m_graphic = GetComponent<MaskableGraphic>();
    }
}

[Serializable]
public class ColorOption
{
    [SerializeField] public Color Color;
    [SerializeField] public Condition Condition;
}
