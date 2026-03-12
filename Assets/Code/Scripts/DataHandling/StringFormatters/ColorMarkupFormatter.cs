using UnityEngine;

[CreateAssetMenu(menuName = "Data/String Formatter/Color Markup Formatter")]
public class ColorMarkupFormatter : StringFormatter
{
    [SerializeField]
    private string m_markupLookup;

    [SerializeField]
    private Color m_color;

    public override string Format(object value, out string message)
    {
        message = null;

        if(value is not string text)
        {
            message = $"ColorMarkupFormatter failed to format provided value of type {value.GetType()}. Only strings are supported.";
            return value.ToString();
        }

        if (!text.Contains(m_markupLookup))
        {
            return text;
        }

        string colorTag = ColorUtility.ToHtmlStringRGB(m_color);
        return text.Replace(m_markupLookup, $"<color=#{colorTag}>");
    }
}
