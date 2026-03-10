using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextBinder : PropertyBinder
{
    [Header("Optional Numerical Formatting:")]
    [SerializeField] private int m_minDigits;
    [SerializeField] private bool m_lowStrengthForLeadingZeros;

    private TextMeshProUGUI m_text;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    protected void Apply(int value)
    {
        m_text.text = Helper.StringFormatting.FormatIntForUI(value, m_minDigits, m_lowStrengthForLeadingZeros);
    }

    protected void Apply(float value)
    {
        int rounded = Mathf.RoundToInt(value);
        m_text.text = Helper.StringFormatting.FormatIntForUI(rounded, m_minDigits, m_lowStrengthForLeadingZeros);
    }

    protected void Apply(string value)
    {
        if (int.TryParse(value, out int number))
        {
            m_text.text = Helper.StringFormatting.FormatIntForUI(number, m_minDigits, m_lowStrengthForLeadingZeros);
            return;
        }

        m_text.text = value;
    }

    protected override void Apply(object value)
    {
        m_text.text = value?.ToString() ?? "";
    }
}