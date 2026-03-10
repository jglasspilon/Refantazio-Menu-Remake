using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextBinder : PropertyBinder<int>
{
    [SerializeField]
    private int m_minDigits;

    [SerializeField]
    private bool m_lowStrengthForLeadingZeros;

    private TextMeshProUGUI m_text;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    protected override void Apply(int value)
    {
        m_text.text = Helper.StringFormatting.FormatIntForUI(value, m_minDigits, m_lowStrengthForLeadingZeros);
    }
}
