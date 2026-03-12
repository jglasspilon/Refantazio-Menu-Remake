using UnityEngine;

[CreateAssetMenu(menuName = "Data/String Formatter/Stat Type Formatter")]
public class StatTypeStringFormatter : StringFormatter
{
    public override string Format(object value, out string message)
    {
        message = null;

        if(value is not EStatType type)
        {
            message = $"PrettifyStatTypeFormatter failed to format provided value of type {value.GetType()}. Only EStatType is supported.";
            return value.ToString();
        }

        return Helper.StringFormatting.PrettifyStat(type);
    }
}
