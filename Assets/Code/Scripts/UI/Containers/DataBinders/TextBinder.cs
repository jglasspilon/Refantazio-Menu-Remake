using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextBinder : PropertyBinder
{
    [Header("Text Formatting:")]
    [SerializeField] private StringFormatter[] m_formatters;
    private TextMeshProUGUI m_text;

    private void Awake()
    {
        Initialize();
    }

    protected override void Apply(object value)
    {
        Initialize();
        string formattedText = value.ToString();

        foreach (StringFormatter formatter in m_formatters)
        {
            formattedText = formatter.Format(formattedText, out string message);

            if (message != null)
                Logger.LogError(message, m_logProfile);
        }

        m_text.text = formattedText;
    }

    private void Initialize()
    {
        if(m_text == null)
            m_text = GetComponent<TextMeshProUGUI>();
    }
}