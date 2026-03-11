using UnityEngine;

[CreateAssetMenu(menuName = "Data/String Formatter/Integer String Formatter")]
public class IntegerStringFormatter : StringFormatter
{
    [SerializeField] private int m_minDigits;
    [Range(0, 1)]
    [SerializeField] private float m_leadingZeroOpacity;

    public override string Format(object value, out string message)
    {
        message = null;

        if (value is int integer)
        {
            return Helper.StringFormatting.FormatIntForUI(integer, m_minDigits, m_leadingZeroOpacity);
        }
        if (value is float floating)
        {
            int rounded = Mathf.RoundToInt(floating);
            return Helper.StringFormatting.FormatIntForUI(rounded, m_minDigits, m_leadingZeroOpacity);
        }
        if (value is string text)
        {
            if(int.TryParse(text, out int number))
                return Helper.StringFormatting.FormatIntForUI(number, m_minDigits, m_leadingZeroOpacity);
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
