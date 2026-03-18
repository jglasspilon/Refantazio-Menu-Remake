using UnityEngine;

[CreateAssetMenu(menuName = "Data/String Formatter/Appender String Formatter")]
public class AppenderStringFormatter : StringFormatter
{
    [SerializeField] private string m_prefix, m_suffix;

    public override string Format(object value, out string message)
    {
        message = null;
        string text = value.ToString();       
        return m_prefix + text + m_suffix;
    }
}
