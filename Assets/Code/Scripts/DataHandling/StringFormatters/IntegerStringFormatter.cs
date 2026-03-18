using UnityEngine;

[CreateAssetMenu(menuName = "Data/String Formatter/Integer String Formatter")]
public class IntegerStringFormatter : StringFormatter
{
    [SerializeField] private int m_minDigits;
    [SerializeField] private bool m_showSign;
    [Range(0, 1)]
    [SerializeField] private float m_leadingZeroOpacity;

    public override string Format(object value, out string message)
    {
        message = null;

        if (value is int integer)
        {
            string sign = integer > 0 ? "+" : integer < 0 ? "-" : "";
            sign = m_showSign ? sign : "";
            return sign + Helper.StringFormatting.FormatIntForUI(Mathf.Abs(integer), m_minDigits, m_leadingZeroOpacity);
        }
        if (value is float floating)
        {
            string sign = floating > 0 ? "+" : floating < 0 ? "-" : "";
            sign = m_showSign ? sign : "";
            int rounded = Mathf.RoundToInt(floating);
            return sign + Helper.StringFormatting.FormatIntForUI(Mathf.Abs(rounded), m_minDigits, m_leadingZeroOpacity);
        }
        if (value is string text)
        {
            if (int.TryParse(text, out int number))
            {
                string sign = number > 0 ? "+" : number < 0 ? "-" : "";
                sign = m_showSign ? sign : "";
                return sign + Helper.StringFormatting.FormatIntForUI(Mathf.Abs(number), m_minDigits, m_leadingZeroOpacity);
            }
            else
            {
                message = $"Provided string {text} is not integer format. Cannot format as integer.";
                return value.ToString();
            }
        }

        message = $"IntegerStringFormatter failed to format provided value of type {value.GetType()}. Only ints, floats or strings are supported.";
        return value.ToString();
    }
}
